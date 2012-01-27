using System;
using System.Collections.Generic;
using System.Text;

namespace Twitter
{
    public enum Env
    {
        Development = 1,
        Production = 2
    }

    public class Literals
    {
        public const string C_CONSUMER_KEY = "CDbmOkmyLj5PxNIOFTE4A";  //also known as the API key
        public const string C_CONSUMER_SECRET = "volNqTryOUkT7pznDeRryV9UP0ZnPdUqu774Vczps";
        public const int C_TWEET_MAX_CHARS = 140;
        public const int C_AVATAR_DIMENSIONS = 48;
        public const string C_ACCOUNT_FILE = "accounts.json";
        public const Env C_ENVIRONMENT = Env.Production;
    }
}
