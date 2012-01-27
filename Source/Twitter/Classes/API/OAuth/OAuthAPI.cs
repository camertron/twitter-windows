using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using Hammock;
using Hammock.Authentication.OAuth;
using Hammock.Web;
using Twitter.API;
using Twitter.Json;
using System.ComponentModel;

namespace Twitter.API.OAuth
{
    public class OAuthAPI
    {
        protected const string C_OAUTH_BASE_URL = "https://api.twitter.com/oauth/";
        protected const string C_REQUEST_TOKEN_URL = "request_token";
        protected const string C_ACCESS_TOKEN_URL = "access_token";
        protected const string C_AUTHORIZE_URL = "authorize";

        public delegate void OAuthCallbackHandler(object sender, bool bSucceeded, string sErrorMessage, OAuthCredentials oaCredentials);
        public event OAuthCallbackHandler RequestTokenReceived;
        public event OAuthCallbackHandler AccessTokenReceived;

        public OAuthAPI()
        {
            //the blog that I looked at said this was a necessary line that he couldn't explain...
            ServicePointManager.Expect100Continue = false;
        }

        //protected void DoRequest(string sEndpoint, WebMethod wmTransferType, Dictionary<string, string> dssParams, APIReturn aprReturn)
        protected void DoRequest(RestRequest rrRequest, OAuthCredentials oaCredentials, APIReturn aprReturn)
        {
            //suggested by the smart guys at dev.twitter.com
            rrRequest.AddParameter("oauth_callback", "oob");

            RestClient rcClient = new RestClient
            {
                Authority = C_OAUTH_BASE_URL,
                Credentials = oaCredentials
            };

            //post request, update credentials object
            rcClient.BeginRequest(rrRequest, DoRequestCallback, aprReturn);
        }

        private void DoRequestCallback(RestRequest rrqRequest, RestResponse rrsResponse, object objUserState)
        {
            RestResponseHash rrhResponse = new RestResponseHash(rrsResponse);
            APIReturn aprReturn = (APIReturn)objUserState;
            OAuthCredentials oaCredentials = (OAuthCredentials)aprReturn.CallbackArg;
            string sErrorMessage = "";

            if (rrsResponse.InnerException != null)
                sErrorMessage = rrsResponse.InnerException.Message;

            APICallbackArgs acArgs = new APICallbackArgs(rrsResponse.StatusCode == HttpStatusCode.OK, sErrorMessage, new OAuthResponseObject(rrhResponse, oaCredentials));
            aprReturn.SynchronizeInvoke(new object[] { acArgs });
        }

        public void GetRequestToken(OAuthCredentials oaCredentials)
        {
            oaCredentials.Type = OAuthType.RequestToken;
            RestRequest rrRequest = new RestRequest
            {
                Path = C_REQUEST_TOKEN_URL
            };

            DoRequest(rrRequest, oaCredentials, new APIReturn(GetRequestTokenCallback, null, oaCredentials));
        }

        private void GetRequestTokenCallback(APICallbackArgs acArgs)
        {
            if (RequestTokenReceived != null)
            {
                OAuthResponseObject oroResponse = (OAuthResponseObject)acArgs.ResponseObject;

                if (acArgs.Succeeded)
                {
                    if (oroResponse.Response.ContainsKey("oauth_token") && oroResponse.Response.ContainsKey("oauth_token_secret"))
                    {
                        oroResponse.Credentials.Token = oroResponse.Response["oauth_token"];
                        oroResponse.Credentials.TokenSecret = oroResponse.Response["oauth_token_secret"];
                        APIReturn.SynchronizeInvoke(RequestTokenReceived, new object[4] { this, true, "", oroResponse.Credentials });
                    }
                    else
                        APIReturn.SynchronizeInvoke(RequestTokenReceived, new object[4] { this, false, "An error occurred retrieving a request token from Twitter.  Please try again.", null });
                }
                else
                    APIReturn.SynchronizeInvoke(RequestTokenReceived, new object[4] { this, false, acArgs.ErrorMessage, null });
            }
        }

        public void GetAccessToken(OAuthCredentials oaCredentials, string sPin = null)
        {
            oaCredentials.Type = OAuthType.AccessToken;
            RestRequest rrRequest = new RestRequest
            {
                Path = C_ACCESS_TOKEN_URL
            };

            if (sPin != null)
                oaCredentials.Verifier = sPin;

            DoRequest(rrRequest, oaCredentials, new APIReturn(GetAccessTokenCallback, null, oaCredentials));
        }

        private void GetAccessTokenCallback(APICallbackArgs acArgs)
        {
            if (AccessTokenReceived != null)
            {
                OAuthResponseObject oroResponse = (OAuthResponseObject)acArgs.ResponseObject;

                if (acArgs.Succeeded)
                {
                    if (oroResponse.Response.ContainsKey("oauth_token") && oroResponse.Response.ContainsKey("oauth_token_secret"))
                    {
                        oroResponse.Credentials.Token = oroResponse.Response["oauth_token"];
                        oroResponse.Credentials.TokenSecret = oroResponse.Response["oauth_token_secret"];
                        oroResponse.Credentials.ClientUsername = oroResponse.Response["screen_name"];
                        APIReturn.SynchronizeInvoke(AccessTokenReceived, new object[4] { this, true, "", oroResponse.Credentials });
                    }
                    else
                        APIReturn.SynchronizeInvoke(AccessTokenReceived, new object[4] { this, false, "An error occurred authenticating with the pin you supplied.  Please try again with the correct pin.", null });
                }
                else
                    APIReturn.SynchronizeInvoke(AccessTokenReceived, new object[4] { this, false, acArgs.ErrorMessage, null });
            }
        }

        /// <summary>
        /// Gets the authorization request URL that a user should visit via their browser.  If the request is being
        /// made via OAuth, the user will be given a pin that they will need to call OAuth() with.
        /// </summary>
        /// <returns></returns>
        public static string GetRequestURL(OAuthCredentials oaCredentials)
        {
            return C_OAUTH_BASE_URL + C_AUTHORIZE_URL + "?oauth_token=" + oaCredentials.Token;
        }

        /// <summary>
        /// Activates this instance using previously acquired access keys.  This function does not verify the
        /// authenticity of these keys - use OAuth() or XAuth() instead.  This function copies the supplied keys
        /// and uses them to sign requests.
        /// </summary>
        /// <param name="sAccessKey">The user's access key, initially provided by Twitter's auth system.</param>
        /// <param name="sAccessSecret">The user's access secret, initially provided by Twitter's auth system.</param>
        /// <param name="sUsername">The user's Twitter.com login screen name.</param>
        /// <param name="sPassword">The user's Twitter.com password.</param>
        public static OAuthCredentials GetCredentials(string sAccessKey, string sAccessSecret, string sUsername = "", string sPassword = "")
        {
            OAuthCredentials oaCredentials = new OAuthCredentials();
            oaCredentials.ConsumerKey = Literals.C_CONSUMER_KEY;
            oaCredentials.ConsumerSecret = Literals.C_CONSUMER_SECRET;
            oaCredentials.Token = sAccessKey;
            oaCredentials.TokenSecret = sAccessSecret;
            oaCredentials.ClientUsername = sUsername;
            oaCredentials.ClientPassword = sPassword;
            oaCredentials.Type = OAuthType.ProtectedResource;
            return oaCredentials;
        }

        /// <summary>
        /// Activates this API instance using previously acquired access keys.  This function does not verify the
        /// authenticity of these keys - use OAuth() or XAuth() instead.  This function copies the supplied keys
        /// and uses them to sign requests.
        /// </summary>
        /// <param name="oaCredentials">The user's credentials, initially provided by Twitter's auth system.</param>
        public OAuthCredentials GetCredentials(OAuthCredentials oaCredentials)
        {
            oaCredentials.Type = OAuthType.ProtectedResource;
            return oaCredentials;
        }
    }

    public class OAuthResponseObject
    {
        private RestResponseHash m_rrhResponse;
        private OAuthCredentials m_oaCredentials;

        public OAuthResponseObject(RestResponseHash rrhResponse, OAuthCredentials oaCredentials)
        {
            m_rrhResponse = rrhResponse;
            m_oaCredentials = oaCredentials;
        }

        public RestResponseHash Response
        {
            get { return m_rrhResponse; }
            set { m_rrhResponse = value; }
        }

        public OAuthCredentials Credentials
        {
            get { return m_oaCredentials; }
            set { m_oaCredentials = value; }
        }
    }
}
