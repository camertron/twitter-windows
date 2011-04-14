using System;
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
using Twitter.API.Json;
using Twitter.API.Basic;

namespace Twitter.API.Streaming
{
    public class UserStream
    {
        public const string C_BASE_URL = "https://userstream.twitter.com";
        public const string C_USER_STREAM_URL = "user.json";
        public const string C_VERSION_PATH = "2";

        public delegate void ReceiveHandler(object sender, JsonDocument jdData);
        public event ReceiveHandler Receive;

        private OAuthCredentials m_oaCredentials;
        private RestClient m_rcClient;
        private IAsyncResult m_iaConnectionAsync = null;
        private BasicAPI m_bscAPI;

        public UserStream(OAuthCredentials oaCredentials)
        {
            m_oaCredentials = oaCredentials;
            m_bscAPI = new BasicAPI(m_oaCredentials);
            m_bscAPI.Authenticate(m_oaCredentials);

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
            Thread thdConnect = new Thread(new ThreadStart(Initialize));
            thdConnect.Start();
        }

        private void Initialize()
        {
            //first get initial timeline from the basic API
            m_bscAPI.GetHomeTimeline(Start, null);
        }

        private void Start(APICallbackArgs acArgs)
        {
            UserTimeline utInitial = (UserTimeline)acArgs.ResponseObject;

            for (int i = utInitial.Statuses.Count - 1; i >= 0; i--)
            {
                foreach (Delegate dlReceiver in Receive.GetInvocationList())
                {
                    ISynchronizeInvoke isSyncInvoke = dlReceiver.Target as ISynchronizeInvoke;

                    try
                    {
                        if (isSyncInvoke != null && isSyncInvoke.InvokeRequired)
                            isSyncInvoke.Invoke(Receive, new object[] { this, new JsonDocument(utInitial.Statuses[i].Object) });
                        else
                            dlReceiver.DynamicInvoke(new object[] { this, new JsonDocument(utInitial.Statuses[i].Object) });
                    }
                    catch (Exception e) { }
                }
            }

            //construct and open streaming request
            RestRequest rrqRequest = new RestRequest
            {
                Path = C_USER_STREAM_URL,
            };

            m_rcClient.AddHeader("User-Agent", "Twitter/1.0");

            //@TODO: uncomment these for production
            //m_iaConnectionAsync = m_rcClient.BeginRequest(rrqRequest, RequestCallback);
            //m_rcClient.CancelStreaming();  //don't know why this is necessary - maybe it isn't?
        }

        private void RequestCallback(RestRequest rrqRequest, RestResponse rrsResponse, object objUserState)
        {
            StreamReader srReader = new StreamReader(rrsResponse.ContentStream, Encoding.UTF8);
            string sCurLine;

            do
            {
                sCurLine = srReader.ReadLine().Trim();
            } while ((sCurLine == "") && (! srReader.EndOfStream));

            foreach (Delegate dlReceiver in Receive.GetInvocationList())
            {
                ISynchronizeInvoke isSyncInvoke = dlReceiver.Target as ISynchronizeInvoke;

                try
                {
                    //StreamWriter swWriter = new StreamWriter("C:/Users/le grand fromage/Desktop/tweetdump.json", true);
                    //swWriter.WriteLine(sCurLine);
                    //swWriter.Close();

                    JsonDocument jdFinal = JsonParser.GetParser().ParseString(sCurLine);

                    if (isSyncInvoke != null && isSyncInvoke.InvokeRequired)
                        isSyncInvoke.Invoke(Receive, new object[] { this, jdFinal });
                    else
                        dlReceiver.DynamicInvoke(new object[] { this, jdFinal });
                }
                catch(Exception e) {}
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
