﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using Hammock;
using Hammock.Web;
using Hammock.Streaming;
using Hammock.Authentication.OAuth;
using Twitter.Json;
using Twitter.API.Basic;

namespace Twitter.API.Streaming
{
    public class UserStream
    {
        public enum ReceiveType
        {
            FriendsList = 0,
            Tweet = 1,
            Reply = 2,
            DirectMessage = 3,
            Delete = 4
        }

        public const string C_BASE_URL = "https://userstream.twitter.com";
        public const string C_USER_STREAM_URL = "user.json";
        public const string C_VERSION_PATH = "2";

        public delegate void ReceiveHandler(object sender, JsonDocument jdData, ReceiveType rtRecvType);
        public event ReceiveHandler Receive;

        private OAuthCredentials m_oaCredentials;
        private RestClient m_rcClient;
        private IAsyncResult m_iaConnectionAsync = null;
        private BasicAPI m_bscAPI;

        public UserStream(OAuthCredentials oaCredentials)
        {
            m_oaCredentials = oaCredentials;
            m_bscAPI = new BasicAPI(m_oaCredentials);

            m_rcClient = new RestClient
            {
                Authority = C_BASE_URL,
                VersionPath = C_VERSION_PATH,
                Credentials = m_oaCredentials,
                Method = WebMethod.Get,
                StreamOptions = new StreamOptions
                {
                    ResultsPerCallback = 1
                }
            };
        }

        public void Connect()
        {
            Thread thdConnect = new Thread(new ThreadStart(HomeTimeline));
            thdConnect.Start();
        }

        private void HomeTimeline()
        {
            //first get initial timeline from the basic API
            m_bscAPI.GetHomeTimeline(HomeTimelineCallback, null, 20, 1, -1, -1, false, true);
        }

        private void HomeTimelineCallback(APICallbackArgs acArgs)
        {
            UserTimeline utInitial = (UserTimeline)acArgs.ResponseObject;

            for (int i = utInitial.Statuses.Count - 1; i >= 0; i--)
                APIReturn.SynchronizeInvoke(Receive, new object[] { this, new JsonDocument(utInitial.Statuses[i].Object), ReceiveType.Tweet });

            m_bscAPI.GetMentions(MentionsCallback, null);
        }

        private void MentionsCallback(APICallbackArgs acArgs)
        {
            UserTimeline utInitial = (UserTimeline)acArgs.ResponseObject;

            for (int i = utInitial.Statuses.Count - 1; i >= 0; i--)
                APIReturn.SynchronizeInvoke(Receive, new object[] { this, new JsonDocument(utInitial.Statuses[i].Object), ReceiveType.Reply });

            EstablishStream();
        }

        private void EstablishStream()
        {
            m_oaCredentials.Verifier = null;
            m_oaCredentials.Type = OAuthType.ProtectedResource;

            //construct and open streaming request
            RestRequest rrqRequest = new RestRequest
            {
                Path = C_USER_STREAM_URL,
            };

            m_rcClient.AddHeader("User-Agent", "Twitter/1.0");

            //@TODO: uncomment these for production
            m_iaConnectionAsync = m_rcClient.BeginRequest(rrqRequest, RequestCallback);
            m_rcClient.CancelStreaming();  //don't know why this is necessary - maybe it isn't?
        }

        private void RequestCallback(RestRequest rrqRequest, RestResponse rrsResponse, object objUserState)
        {
            try
            {
                StreamReader srReader = new StreamReader(rrsResponse.ContentStream, Encoding.UTF8);
                string sCurLine;

                do
                {
                    sCurLine = srReader.ReadLine().Trim();
                } while ((sCurLine == "") && (!srReader.EndOfStream));

                JsonDocument jdFinal = JsonParser.GetParser().ParseString(sCurLine);

                if (jdFinal != null)
                {
                    if (jdFinal.Root.IsNode())
                    {
                        if (jdFinal.Root.ToNode().ContainsKey("friends"))
                        {
                            //this is the friends list that's sent at the beginning of each userstream connection
                            APIReturn.SynchronizeInvoke(Receive, this, jdFinal, ReceiveType.FriendsList);
                        }
                        else if (jdFinal.Root.ToNode().ContainsKey("retweeted"))
                        {
                            Status stNewStatus = new Status(jdFinal.Root.ToNode());

                            if (stNewStatus.IsReply && stNewStatus.ReplyNames.Contains(m_oaCredentials.ClientUsername))
                                APIReturn.SynchronizeInvoke(Receive, this, jdFinal, ReceiveType.Reply);
                            else
                                APIReturn.SynchronizeInvoke(Receive, this, jdFinal, ReceiveType.Tweet);
                        }
                        else if (jdFinal.Root.ToNode().ContainsKey("recipient_id") && jdFinal.Root.ToNode().ContainsKey("sender_id"))
                        {
                            DirectMessage dmNewMessage = new DirectMessage(jdFinal.Root.ToNode());
                            APIReturn.SynchronizeInvoke(Receive, this, jdFinal, ReceiveType.DirectMessage);
                        }

                        //also need to add OnDelete for when a tweet gets deleted
                    }
                }
            }
            catch(Exception e)
            {
                MessageBox.Show("An unknown Twitter API error has occurred (user streams). " + e.Message, "API Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        public void Disconnect()
        {
            if (m_iaConnectionAsync != null)
                m_rcClient.EndRequest(m_iaConnectionAsync, new TimeSpan(0, 0, 6));
        }

        public bool Connected
        {
            get { return (m_iaConnectionAsync != null); }
        }

        ~UserStream()
        {
            Disconnect();
        }
    }
}
