using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hammock;
using Hammock.Authentication.OAuth;
using Hammock.Web;
using Twitter.API.Basic;

namespace Twitter.API.Streaming
{
    public class StreamingAPI : BaseAPI
    {
        public StreamingAPI(string sConsumerKey, string sConsumerSecret) : base(sConsumerKey, sConsumerSecret) { }

        public UserStream GetUserStream()
        {
            CheckAuthenticated();
            return new UserStream(m_oaCredentials);
        }
    }
}