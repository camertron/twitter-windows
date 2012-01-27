using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.ComponentModel;
using Hammock.Authentication.OAuth;

namespace Twitter.API
{
    /// <summary>
    /// The base class for all Twitter API classes.  Provides shared functionality that includes authentication over OAuth.
    /// </summary>
    public abstract class BaseAPI
    {
        protected OAuthCredentials m_oaCredentials;

        public BaseAPI(OAuthCredentials oaCredentials)
        {
            m_oaCredentials = oaCredentials;
        }

        public OAuthCredentials Credentials
        {
            get { return m_oaCredentials; }
        }

        /// <summary>
        /// Generic delegate that all API callback functions should conform to.
        /// </summary>
        /// <param name="acArgs">Information about this callback, including whether or not the request succeeded and the response object.</param>
        public delegate void APICallback(APICallbackArgs acArgs);

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
