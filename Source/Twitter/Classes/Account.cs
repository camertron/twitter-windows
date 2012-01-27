using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Threading;
using System.IO;
using System.IO.IsolatedStorage;
using Twitter.API;
using Twitter.API.Streaming;
using Twitter.API.Basic;
using Twitter.API.OAuth;
using Twitter.Json;
using Hammock.Authentication.OAuth;

namespace Twitter
{
    public class Account
    {
        public event EventHandler UserObjectReceived;

        private UserStream m_usUserStream = null;
        private StreamingAPI m_sAPI;
        private BasicAPI m_bAPI;
        private StatusList m_slStatuses;
        private List<DirectMessage> m_ldmDirectMessages;
        private User m_uUserObject = null;
        private Bitmap m_bmpAvatar;
        private OAuthCredentials m_oaCredentials;

        public Account(OAuthCredentials oaCredentials)
        {
            m_oaCredentials = oaCredentials;
            Initialize();
        }

        public Account(string sAccessKey, string sAccessSecret, string sUsername, string sPassword)
        {
            m_oaCredentials = OAuthAPI.GetCredentials(sAccessKey, sAccessSecret, sUsername, sPassword);
            Initialize();
        }

        private void Initialize()
        {
            //construct and authenticate streaming/basic APIs
            m_bAPI = new BasicAPI(m_oaCredentials);
            m_sAPI = new StreamingAPI(m_oaCredentials);

            m_slStatuses = new StatusList();
            m_ldmDirectMessages = new List<DirectMessage>();

            m_usUserStream = m_sAPI.GetUserStream();
        }

        public void Connect()
        {
            m_usUserStream.Connect();

            Thread thUserObj = new Thread(new ThreadStart(GetUserObject));
            thUserObj.Start();
        }

        private void GetUserObject()
        {
            m_bAPI.LookupUser(GetUserObjectCallback, null, new List<string>(new string[] { m_bAPI.Credentials.ClientUsername }));
        }

        private void GetUserObjectCallback(APICallbackArgs acArgs)
        {
            List<User> luResponseList = (List<User>)acArgs.ResponseObject;

            if (luResponseList.Count > 0)
            {
                m_uUserObject = luResponseList[0];

                if (UserObjectReceived != null)
                    APIReturn.SynchronizeInvoke(UserObjectReceived, this, EventArgs.Empty);
            }
        }

        public User UserObject
        {
            get { return m_uUserObject; }
        }

        public UserStream UserStream
        {
            get { return m_usUserStream; }
        }

        public BasicAPI BasicAPI
        {
            get { return m_bAPI; }
        }

        public StreamingAPI StreamingAPI
        {
            get { return m_sAPI; }
        }

        public StatusList Statuses
        {
            get { return m_slStatuses; }
        }

        public List<DirectMessage> DirectMessages
        {
            get { return m_ldmDirectMessages; }
        }

        public Bitmap Avatar
        {
            get { return m_bmpAvatar; }
            set { m_bmpAvatar = value; }
        }

        public OAuthCredentials Credentials
        {
            get { return m_oaCredentials; }
        }

        public string ToJson()
        {
            string sScreenName;

            if (m_uUserObject == null)
                sScreenName = m_oaCredentials.ClientUsername;
            else
                sScreenName = m_uUserObject["screen_name"].ToString();

            string sJsonStr = "{access_token: '" + Security.ProtectString(m_oaCredentials.Token) + "', ";
            sJsonStr += "access_secret: '" + Security.ProtectString(m_oaCredentials.TokenSecret) + "', ";
            sJsonStr += "screen_name: '" + sScreenName + "'}";
            return sJsonStr;
        }
    }

    public class AccountList : List<Account>
    {
        public delegate void AccountHandler(object sender, Account actSubject);

        public event EventHandler AccountSwitched;
        public event AccountHandler AccountAdded;
        public event AccountHandler AccountRemoved;
        public event AccountHandler UserObjectReceived;

        private const int C_NULL_ACCOUNT_INDEX = -1;
        private int m_iActiveAccountIndex = C_NULL_ACCOUNT_INDEX;

        public AccountList() : base() { }

        public void Add(string sAccessToken, string sAccessSecret, string sUsername = "", string sPassword = "")
        {
            Account actNew = new Account(sAccessToken, sAccessSecret, sUsername, sPassword);
            base.Add(actNew);
            HookupNewAccount(actNew);

            //if there weren't any accounts in here before, set the newly created account as the active account
            if (base.Count == 1)
                m_iActiveAccountIndex = 0;

            if (AccountAdded != null)
                AccountAdded(this, actNew);
        }

        public new void Add(Account actNew)
        {
            base.Add(actNew);
            HookupNewAccount(actNew);

            if (base.Count == 1)
                m_iActiveAccountIndex = 0;

            if (AccountAdded != null)
                AccountAdded(this, actNew);
        }

        //use this function to add events to each account
        private void HookupNewAccount(Account actNew)
        {
            actNew.UserObjectReceived += new EventHandler(Account_UserObjectReceived);
        }

        //use this function to remove events from each account
        private void UnhookAccount(Account actNew)
        {
            actNew.UserObjectReceived -= new EventHandler(Account_UserObjectReceived);
        }

        private void Account_UserObjectReceived(object sender, EventArgs e)
        {
            if (UserObjectReceived != null)
                UserObjectReceived(this, (Account)sender);
        }

        public new void RemoveAt(int iIndex)
        {
            Account actReturn = base[iIndex];

            base.RemoveAt(iIndex);
            UnhookAccount(actReturn);

            if (AccountRemoved != null)
                AccountRemoved(this, actReturn);
        }

        public new void Remove(Account actToRemove)
        {
            Account actReturn = base[base.IndexOf(actToRemove)];

            base.Remove(actToRemove);
            UnhookAccount(actToRemove);

            if (AccountRemoved != null)
                AccountRemoved(this, actReturn);
        }

        public bool SwitchAccount(int iNewActiveIndex)
        {
            m_iActiveAccountIndex = iNewActiveIndex;

            if (AccountSwitched != null)
                AccountSwitched(this, EventArgs.Empty);

            if (! ActiveAccount.UserStream.Connected)
                ActiveAccount.UserStream.Connect();

            return true;
        }

        public bool SwitchAccount(string sNewActiveUsername)
        {
            for (int i = 0; i < base.Count; i++)
            {
                if (base[i].BasicAPI.Credentials.ClientUsername == sNewActiveUsername)
                    return SwitchAccount(i);
            }

            return false;
        }

        public Account ActiveAccount
        {
            get
            {
                if (m_iActiveAccountIndex != C_NULL_ACCOUNT_INDEX)
                    return base[m_iActiveAccountIndex];
                else
                    return null;
            }
        }

        //@TODO
        //this will eventually read a much more complicated file
        public void LoadFromStream(Stream stmReader)
        {
            JsonDocument jsDoc = JsonParser.GetParser().ParseStream(new StreamReader(stmReader));

            if (jsDoc != null)
            {
                List<JsonObject> ljoUsers = jsDoc.Root.ToList();

                for (int i = 0; i < ljoUsers.Count; i++)
                {
                    JsonNode jnCredentials = ljoUsers[i].ToNode();
                    this.Add(new Account(Security.UnprotectString(jnCredentials["access_token"].ToString()), Security.UnprotectString(jnCredentials["access_secret"].ToString()), jnCredentials["screen_name"].ToString(), ""));
                }
            }
        }

        public string ToJson()
        {
            StringBuilder sbJsonStr = new StringBuilder("[");

            for (int i = 0; i < base.Count; i ++)
                sbJsonStr.Append(base[i].ToJson());

            sbJsonStr.Append("]");
            return sbJsonStr.ToString();
        }

        public void WriteToStream(Stream stmWriter)
        {
            byte[] baBytes = Encoding.UTF8.GetBytes(ToJson());
            stmWriter.Write(baBytes, 0, baBytes.Length);
        }

        public void Save()
        {
            IsolatedStorageFileStream isfStream = IsolatedStorageManager.GetManager().Store.OpenFile(Literals.C_ACCOUNT_FILE, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write);
            WriteToStream(isfStream);
            isfStream.Close();
        }

        public void Load()
        {
            IsolatedStorageManager isManager = IsolatedStorageManager.GetManager();

            if (isManager.Store.FileExists(Literals.C_ACCOUNT_FILE))
            {
                IsolatedStorageFileStream isfStream = IsolatedStorageManager.GetManager().Store.OpenFile(Literals.C_ACCOUNT_FILE, FileMode.OpenOrCreate, FileAccess.Read);
                LoadFromStream(isfStream);
                isfStream.Close();
            }
        }
    }
}
