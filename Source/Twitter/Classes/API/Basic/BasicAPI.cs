using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Windows.Forms;
using System.ComponentModel;
using Hammock;
using Hammock.Authentication.OAuth;
using Hammock.Web;
using Twitter.API.Json;

namespace Twitter.API.Basic
{
    public class BasicAPI : BaseAPI
    {
        protected const string C_BASE_URL = "http://api.twitter.com";

        public BasicAPI(string sConsumerKey, string sConsumerSecret) : base(sConsumerKey, sConsumerSecret) { }
        public BasicAPI(OAuthCredentials oaCredentials) : base(oaCredentials) { }

        private void DoRequest(string sEndpoint, WebMethod wmTransferType, Dictionary<string, string> dssParams, APIReturn aprReturn)
        {
            RestRequest rrRequest = new RestRequest();

            rrRequest.Path = sEndpoint;
            m_oaCredentials.Type = OAuthType.ProtectedResource;

            foreach (KeyValuePair<string, string> kvpCur in dssParams)
                rrRequest.AddParameter(kvpCur.Key, kvpCur.Value);

            RestClient rcClient = new RestClient
            {
                Authority = C_BASE_URL,
                VersionPath = "1",
                Credentials = m_oaCredentials,
                Method = wmTransferType
            };

            rcClient.BeginRequest(rrRequest, DoRequestCallback, aprReturn);
        }

        private void DoRequestCallback(RestRequest rrqRequest, RestResponse rrsResponse, object objUserState)
        {
            JsonDocument jsDoc = JsonParser.GetParser().ParseStream(new StreamReader(rrsResponse.ContentStream, Encoding.UTF8));
            APIReturn aprReturn = (APIReturn)objUserState;
            object objToReturn = null;
            string sErrorMessage = "";
            APICallbackArgs acArgs;

            if (jsDoc.Root.IsNode() && jsDoc.Root.ToNode().ContainsKey("error"))
                sErrorMessage = jsDoc.Root.ToNode()["error"].ToString();

            if (aprReturn.ReturnType != null)
            {
                if (jsDoc.Root.IsList())
                {
                    //if the return type is a generic list...
                    if (aprReturn.ReturnType.Name == "List`1")
                    {
                        //get an instance of the list<generic> that will have generics added to it and eventually be returned via the callback
                        //get the type of the generic (because we'll need to make specific instances of him later)
                        objToReturn = System.Activator.CreateInstance(aprReturn.ReturnType);
                        Type tListObjType = aprReturn.ReturnType.GetGenericArguments()[0];

                        //dynamically invoke the Add() method of the list
                        //what's being added: a newly minted instance of the generic class plus data via the constructor
                        foreach (JsonObject joCur in jsDoc.Root.ToList())
                            aprReturn.ReturnType.GetMethod("Add").Invoke(objToReturn, new Object[] { System.Activator.CreateInstance(tListObjType, joCur.ToNode()) });
                    }
                    else
                        objToReturn = System.Activator.CreateInstance(aprReturn.ReturnType, jsDoc.Root.ToList());
                }
                else if (jsDoc.Root.IsNode())
                    objToReturn = System.Activator.CreateInstance(aprReturn.ReturnType, jsDoc.Root.ToNode());
            }

            acArgs = new APICallbackArgs(rrsResponse.StatusCode == HttpStatusCode.OK, sErrorMessage, objToReturn);
            aprReturn.SynchronizeInvoke(new object[] { acArgs });  //synchronously invoke the return callback
        }

        public void UpdateStatus(APICallback apcCallback, object objCallbackArg, string sMessage)
        {
            CheckAuthenticated();

            Dictionary<string, string> dssParams = new Dictionary<string, string>();
            dssParams["status"] = sMessage;
            DoRequest("statuses/update.json", WebMethod.Post, dssParams, new APIReturn(apcCallback, null, objCallbackArg));
        }

        public void Retweet(APICallback apcCallback, object objCallbackArg, string sTweetId, bool bTrimUser = false, bool bIncludeEntities = false)
        {
            CheckAuthenticated();

            Dictionary<string, string> dssParams = new Dictionary<string, string>();

            AddParameter(ref dssParams, "trim_user", bTrimUser);
            AddParameter(ref dssParams, "include_entities", bIncludeEntities);

            DoRequest("statuses/retweet/" + sTweetId + ".json", WebMethod.Post, dssParams, new APIReturn(apcCallback, null, objCallbackArg));
        }

        public void GetUserTimeline(APICallback apcCallback, object objCallbackArg, string sScreenName = null, int iPage = 1, int iCount = 20, int iUserId = -1, 
                                    int iMaxId = -1, bool bTrimUser = false, bool bIncludeRts = true, 
                                    bool bIncludeEntities = false)
        {
            CheckAuthenticated();

            Dictionary<string, string> dssParams = new Dictionary<string, string>();

            AddParameter(ref dssParams, "screen_name", sScreenName);
            AddParameter(ref dssParams, "page", iPage);
            AddParameter(ref dssParams, "count", iCount);
            AddParameter(ref dssParams, "user_id", iUserId);
            AddParameter(ref dssParams, "max_id", iMaxId);
            AddParameter(ref dssParams, "trim_user", bTrimUser);
            AddParameter(ref dssParams, "include_rts", bIncludeRts);
            AddParameter(ref dssParams, "include_entities", bIncludeEntities);

            DoRequest("statuses/user_timeline.json", WebMethod.Get, dssParams, new APIReturn(apcCallback, typeof(UserTimeline), objCallbackArg));
        }

        public void GetHomeTimeline(APICallback apcCallback, object objCallbackArg, int iCount = 20, int iPage = 1, int iSinceId = -1,
                                    int iMaxId = -1, bool bTrimUser = false, bool bIncludeEntities = false)
        {
            CheckAuthenticated();

            Dictionary<string, string> dssParams = new Dictionary<string, string>();

            AddParameter(ref dssParams, "since_id", iSinceId);
            AddParameter(ref dssParams, "max_id", iMaxId);
            AddParameter(ref dssParams, "count", iCount);
            AddParameter(ref dssParams, "page", iPage);
            AddParameter(ref dssParams, "trim_user", bTrimUser);
            AddParameter(ref dssParams, "include_entities", bIncludeEntities);

            DoRequest("statuses/home_timeline.json", WebMethod.Get, dssParams, new APIReturn(apcCallback, typeof(UserTimeline), objCallbackArg));
        }

        public void LookupUser(APICallback apcCallback, object objCallbackArg, List<string> lsScreenNames = null, List<string> lsUserIds = null, bool bIncludeEntities = false)
        {
            CheckAuthenticated();

            Dictionary<string, string> dssParams = new Dictionary<string, string>();

            if (lsScreenNames != null)
                AddParameter(ref dssParams, "screen_name", lsScreenNames.ToArray());

            if (lsUserIds != null)
                AddParameter(ref dssParams, "user_id", lsUserIds.ToArray());

            AddParameter(ref dssParams, "include_entities", bIncludeEntities);

            DoRequest("users/lookup.json", WebMethod.Post, dssParams, new APIReturn(apcCallback, typeof(List<User>), objCallbackArg));
        }

        public void GetFavorites(APICallback apcCallback, object objCallbackArg, string sUserIdOrScreenName = "", int iPage = 1, bool bIncludeEntities = false)
        {
            CheckAuthenticated();

            Dictionary<string, string> dssParams = new Dictionary<string, string>();

            AddParameter(ref dssParams, "page", iPage);
            AddParameter(ref dssParams, "include_entities", bIncludeEntities);

            if (sUserIdOrScreenName == "")
                DoRequest("favorites.format", WebMethod.Get, dssParams, new APIReturn(apcCallback, typeof(Status), objCallbackArg));
            else
                DoRequest("favorites/" + sUserIdOrScreenName + ".json", WebMethod.Get, dssParams, new APIReturn(apcCallback, typeof(Status), objCallbackArg));
        }

        public void CreateFavorite(APICallback apcCallback, object objCallbackArg, string sStatusId, bool bIncludeEntities = false)
        {
            CheckAuthenticated();

            Dictionary<string, string> dssParams = new Dictionary<string, string>();

            AddParameter(ref dssParams, "id", sStatusId);
            AddParameter(ref dssParams, "include_entities", bIncludeEntities);

            DoRequest("favorites/create/" + sStatusId + ".json", WebMethod.Post, dssParams, new APIReturn(apcCallback, typeof(Status), objCallbackArg));
        }

        public void DestroyFavorite(APICallback apcCallback, object objCallbackArg, string sStatusId, bool bIncludeEntities = false)
        {
            CheckAuthenticated();

            Dictionary<string, string> dssParams = new Dictionary<string, string>();

            AddParameter(ref dssParams, "id", sStatusId);
            AddParameter(ref dssParams, "include_entities", bIncludeEntities);

            DoRequest("favorites/destroy/" + sStatusId + ".json", WebMethod.Delete, dssParams, new APIReturn(apcCallback, typeof(Status), objCallbackArg));
        }
    }
}