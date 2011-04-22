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
    /// <summary>
    /// The base class for all Twitter API classes.  Provides shared functionality that includes authentication over OAuth.
    /// </summary>
    public abstract class BaseAPI
    {
        /// <summary>
        /// Generic delegate that all API callback functions should conform to.
        /// </summary>
        /// <param name="acArgs">Information about this callback, including whether or not the request succeeded and the response object.</param>
        public delegate void APICallback(APICallbackArgs acArgs);

        protected const string C_OAUTH_BASE_URL = "https://api.twitter.com/oauth/";
        protected const string C_REQUEST_TOKEN_URL = "request_token";
        protected const string C_ACCESS_TOKEN_URL = "access_token";
        protected const string C_AUTHORIZE_URL = "authorize";

        protected OAuthCredentials m_oaCredentials;
        protected RestResponseHash m_rrhRequestToken = null;
        protected RestResponseHash m_rrhAccessToken = null;
        protected bool m_bAuthenticated = false;

        /// <summary>
        /// Gets the credentials used to authenticate with the Twitter API.
        /// </summary>
        public OAuthCredentials Credentials
        {
            get { return m_oaCredentials; }
        }

        /// <summary>
        /// Gets the request token sent to Twitter during the OAuth cycle.
        /// </summary>
        public RestResponseHash RequestToken
        {
            get { return m_rrhRequestToken; }
        }

        /// <summary>
        /// Gets the access token returned by Twitter during the OAuth cycle.
        /// </summary>
        public RestResponseHash AccessToken
        {
            get { return m_rrhAccessToken; }
        }

        /// <summary>
        /// Gets whether or not this API instance has been authenticated.
        /// Returns true if authenticated, false otherwise.
        /// </summary>
        public bool Authenticated
        {
            get { return m_bAuthenticated; }
        }

        /// <summary>
        /// Overloaded.  Called only by derived classes.  Instantiates a new API instance with the given credentials.
        /// </summary>
        /// <param name="sConsumerKey">The consumer key that will be used to authenticate with the Twitter API.
        /// This key is app-specific and is provided by Twitter.</param>
        /// <param name="sConsumerSecret">The consumer secret that will be used to authenticate with the Twitter API.
        /// This key is app-specific and is provided by Twitter.</param>
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

        /// <summary>
        /// Overloaded.  Called only by derived classes.  Instantiates a new API instance with the given credentials.
        /// </summary>
        /// <param name="oaCredentials">The credentials that will be used to authenticate with the Twitter API.</param>
        public BaseAPI(OAuthCredentials oaCredentials)
        {
            Initialize();
            m_oaCredentials = oaCredentials;
        }

        //common constructor logic
        protected void Initialize()
        {
            //the blog that I looked at said this was a necessary line that he couldn't explain...
            ServicePointManager.Expect100Continue = false;
        }

        /// <summary>
        /// Activates this API instance using previously acquired access keys.  This function does not verify the
        /// authenticity of these keys - use OAuth() or XAuth() instead.  This function copies the supplied keys
        /// and uses them to sign requests.
        /// </summary>
        /// <param name="sAccessKey">The user's access key, initially provided by Twitter's auth system.</param>
        /// <param name="sAccessSecret">The user's access secret, initially provided by Twitter's auth system.</param>
        /// <param name="sUsername">The user's Twitter.com login screen name.</param>
        /// <param name="sPassword">The user's Twitter.com password.</param>
        public void Authenticate(string sAccessKey, string sAccessSecret, string sUsername = "", string sPassword = "")
        {
            m_oaCredentials.Token = sAccessKey;
            m_oaCredentials.TokenSecret = sAccessSecret;
            m_oaCredentials.ClientUsername = sUsername;
            m_oaCredentials.ClientPassword = sPassword;
            m_oaCredentials.Type = OAuthType.ProtectedResource;
            m_bAuthenticated = true;
        }

        /// <summary>
        /// Activates this API instance using previously acquired access keys.  This function does not verify the
        /// authenticity of these keys - use OAuth() or XAuth() instead.  This function copies the supplied keys
        /// and uses them to sign requests.
        /// </summary>
        /// <param name="oaCredentials">The user's credentials, initially provided by Twitter's auth system.</param>
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

        /// <summary>
        /// Gets the authorization request URL that a user should visit via their browser.  If the request is being
        /// made via OAuth, the user will be given a pin that they will need to call OAuth() with.
        /// </summary>
        /// <returns></returns>
        public string GetRequestURL()
        {
            if (m_rrhRequestToken == null)
            {
                if (!GetRequestToken())
                    throw new UnavailableRequestTokenException();
            }

            return C_OAUTH_BASE_URL + C_AUTHORIZE_URL + "?oauth_token=" + m_oaCredentials.Token;
        }

        /// <summary>
        /// Authenticates a user with the Twitter OAuth system.  This function should only need to be called once
        /// per user, since you should store the resulting access key and access secret for later use, and not
        /// irritate the auth API with unnecessary requests.
        /// </summary>
        /// <param name="sPin">The pin number given to the user through their browser when they visited the request 
        /// URL available via GetRequestURL().</param>
        /// <returns></returns>
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

        /// <summary>
        /// Authenticates a user with the Twitter XAuth system.  This function should only need to be called once
        /// per user, since you should store the resulting access key and access secret for later use, and not
        /// irritate the auth API with unnecessary requests.
        /// </summary>
        /// <param name="sUsername">The user's Twitter.com screen name.</param>
        /// <param name="sPassword">The user's Twitter.com password.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Formats and adds a parameter to a parameter dictionary.
        /// If the new item is a string, it's added to the dictionary with no formatting.
        /// If the new item is an int, the int is converted to a string.
        /// If the new item is a bool, true becomes "1", false becomes "0".
        /// If the new item is a string array, it is turned into a comma separated single string.
        /// </summary>
        /// <param name="dssParams">The dictionary to add the new parameter to.</param>
        /// <param name="sParamKey">The dictionary key to map to objNewParam.</param>
        /// <param name="objNewParam">The new parameter being added to the dictionary.</param>
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

        public void SynchronizeInvoke(params object[] oaArgs)
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
