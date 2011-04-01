using System;
using System.Collections.Generic;
using System.Text;

namespace Twitter.API
{
    public class NotAuthenticatedException : ApplicationException
    {
        public override string Message
        {
            get { return "Not authenticated with the Twitter API.  You must authenticate before accessing protected resources."; }
        }
    }

    public class UnavailableRequestTokenException : ApplicationException
    {
        public override string Message
        {
            get { return "Could not get a request token from Twitter."; }
        }
    }
}
