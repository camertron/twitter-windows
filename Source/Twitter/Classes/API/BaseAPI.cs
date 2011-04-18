using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.ComponentModel;
using Hammock;
using Hammock.Authentication.OAuth;
using Hammock.Web;

namespace Twitter.API
{
    public abstract class BaseAPI
    {
        public delegate void APICallback(APICallbackArgs acArgs);

        protected const string C_OAUTH_BASE_URL = "https://api.twitter.com/oauth/";
        protected const string C_REQUEST_TOKEN_URL = "request_token";
        protected const string C_ACCESS_TOKEN_URL = "access_token";
        protected const string C_AUTHORIZE_URL = "authorize";

        protected OAuthCredentials m_oaCredentials;
        protected RestResponseHash m_rrhRequestToken = null;
        protected RestResponseHash m_rrhAccessToken = null;
        protected bool m_bAuthenticated = false;

        public OAuthCredentials Credentials
        {
            get { return m_oaCredentials; }
        }

        public RestResponseHash RequestToken
        {
            get { return m_rrhRequestToken; }
        }

        public RestResponseHash AccessToken
        {
            get { return m_rrhAccessToken; }
        }

        public bool Authenticated
        {
            get { return m_bAuthenticated; }
        }

        public BaseAPI(string sConsumerKey, string sConsumerSecret)
        {
            Initialize();

            //set locals
            m_oaCredentials = new OAuthCredentials
            {
                SignatureMethod = OAuthSignatureMethod.HmacSha1,
                ParameterHandling = OAuthParameterHandling.HttpAuthorizationHeader,
                ConsumerKey = sConsumerKey,
                ConsumerSecret = sConsumerSecret
            };
        }

        public BaseAPI(OAuthCredentials oaCredentials)
        {
            Initialize();
            m_oaCredentials = oaCredentials;
        }

        protected void Initialize()
        {
            //the blog that I looked at said this was a necessary line that he couldn't explain...
            ServicePointManager.Expect100Continue = false;
        }

        public void Authenticate(string sAccessKey, string sAccessSecret, string sUsername = "", string sPassword = "")
        {
            m_oaCredentials.Token = sAccessKey;
            m_oaCredentials.TokenSecret = sAccessSecret;
            m_oaCredentials.ClientUsername = sUsername;
            m_oaCredentials.ClientPassword = sPassword;
            m_oaCredentials.Type = OAuthType.ProtectedResource;
            m_bAuthenticated = true;
        }

        public void Authenticate(OAuthCredentials oaCredentials)
        {
            m_oaCredentials = oaCredentials;
            m_oaCredentials.Type = OAuthType.ProtectedResource;
            m_bAuthenticated = true;
        }

        private bool GetRequestToken()
        {
            m_oaCredentials.Type = OAuthType.RequestToken;

            RestRequest rrRequest = new RestRequest
            {
                Path = C_REQUEST_TOKEN_URL
            };

            //suggested by the smart guys at dev.twitter.com
            rrRequest.AddParameter("oauth_callback", "oob");

            RestClient rcClient = new RestClient
            {
                Authority = C_OAUTH_BASE_URL,
                Credentials = m_oaCredentials
            };

            //post request, update credentials object
            m_rrhRequestToken = new RestResponseHash(rcClient.Request(rrRequest));

            if (m_rrhRequestToken.ContainsKey("oauth_token") && m_rrhRequestToken.ContainsKey("oauth_token_secret"))
            {
                m_oaCredentials.Token = m_rrhRequestToken["oauth_token"];
                m_oaCredentials.TokenSecret = m_rrhRequestToken["oauth_token_secret"];
                return true;
            }
            else
                return false;
        }

        public string GetRequestURL()
        {
            if (m_rrhRequestToken == null)
            {
                if (!GetRequestToken())
                    throw new UnavailableRequestTokenException();
            }

            return C_OAUTH_BASE_URL + C_AUTHORIZE_URL + "?oauth_token=" + m_oaCredentials.Token;
        }

        public bool OAuth(string sPin = null)
        {
            m_oaCredentials.Type = OAuthType.AccessToken;

            RestRequest rrRequest = new RestRequest
            {
                Path = C_ACCESS_TOKEN_URL
            };

            if (sPin != null)
                m_oaCredentials.Verifier = sPin;

            RestClient rcClient = new RestClient
            {
                Authority = C_OAUTH_BASE_URL,
                Credentials = m_oaCredentials
            };

            m_rrhAccessToken = new RestResponseHash(rcClient.Request(rrRequest));

            if (m_rrhAccessToken.ContainsKey("oauth_token") && m_rrhAccessToken.ContainsKey("oauth_token_secret"))
            {
                m_oaCredentials.Token = m_rrhAccessToken["oauth_token"];
                m_oaCredentials.TokenSecret = m_rrhAccessToken["oauth_token_secret"];
                m_bAuthenticated = true;
                return true;
            }
            else
                return false;
        }

        public bool XAuth(string sUsername, string sPassword)
        {
            //handles user/pass authentication using Twitter's XAuth coolness
            //(this currently doesn't work)
            //you need to have XAuth turned on for your app - special treatment
            m_oaCredentials.ClientUsername = sUsername;
            m_oaCredentials.ClientPassword = sPassword;
            return OAuth();
        }

        protected void CheckAuthenticated()
        {
            if (!m_bAuthenticated)
                throw new NotAuthenticatedException();
        }

        protected void AddParameter(ref Dictionary<string, string> dssParams, string sParamKey, object objNewParam)
        {
            if (objNewParam != null)
            {
                if (objNewParam.GetType() == typeof(string))
                {
                    if (((string)objNewParam) != "")
                        dssParams[sParamKey] = objNewParam.ToString();
                }
                else if (objNewParam.GetType() == typeof(int))
                {
                    if (((int)objNewParam) != -1)
                        dssParams[sParamKey] = objNewParam.ToString();
                }
                else if (objNewParam.GetType() == typeof(bool))
                {
                    if (((bool)objNewParam))
                        dssParams[sParamKey] = "1";
                }
                else if ((objNewParam.GetType() == typeof(string[])) && (((string[])objNewParam).Length > 0))
                    dssParams[sParamKey] = String.Join(",", (string[])objNewParam);
            }
        }
    }

    public class APIReturn
    {
        private BaseAPI.APICallback m_apcCallback;
        private Type m_tReturnType;
        private object m_objCallbackArg;

        public APIReturn(BaseAPI.APICallback apcCallback, Type tReturnType, object objCallbackArg)
        {
            m_apcCallback = apcCallback;
            m_tReturnType = tReturnType;
            m_objCallbackArg = objCallbackArg;
        }

        public BaseAPI.APICallback Callback
        {
            get { return m_apcCallback; }
        }

        public Type ReturnType
        {
            get { return m_tReturnType; }
        }

        public object CallbackArg
        {
            get { return m_objCallbackArg; }
        }

        public void SynchronizeInvoke(object[] oaArgs)
        {
            APIReturn.SynchronizeInvoke(m_apcCallback, oaArgs);
        }

        public static void SynchronizeInvoke(Delegate dlHandler, params object[] oaArgs)
        {
            foreach (Delegate dlReceiver in dlHandler.GetInvocationList())
            {
                ISynchronizeInvoke isSyncInvoke = dlReceiver.Target as ISynchronizeInvoke;

                try
                {
                    if (isSyncInvoke != null && isSyncInvoke.InvokeRequired)
                        isSyncInvoke.Invoke(dlHandler, oaArgs);
                    else
                        dlReceiver.DynamicInvoke(oaArgs);
                }
                catch (Exception e) { }
            }
        }
    }

    public class APICallbackArgs
    {
        private bool m_bSucceeded;
        private string m_sErrorMessage;
        private object m_objResponseObject;

        public APICallbackArgs(bool bSucceeded, string sErrorMessage, object objResponseObject)
        {
            m_bSucceeded = bSucceeded;
            m_sErrorMessage = sErrorMessage;
            m_objResponseObject = objResponseObject;
        }

        public bool Succeeded
        {
            get { return m_bSucceeded; }
        }

        public string ErrorMessage
        {
            get { return m_sErrorMessage; }
        }

        public object ResponseObject
        {
            get { return m_objResponseObject; }
        }
    }
}
