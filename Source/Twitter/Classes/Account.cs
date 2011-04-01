using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Twitter.API.Streaming;
using Twitter.API.Basic;

namespace Twitter
{
    public class Account
    {
        private UserStream m_usUserStream = null;
        private StreamingAPI m_sAPI;
        private BasicAPI m_bAPI;
        private List<Status> m_lstStatuses;
        private List<DirectMessage> m_ldmDirectMessages;

        public Account(string sAccessToken, string sAccessSecret, string sUsername = "", string sPassword = "")
        {
            //construct and authenticate streaming/basic APIs
            m_bAPI = new BasicAPI(Literals.C_CONSUMER_KEY, Literals.C_CONSUMER_SECRET);
            m_sAPI = new StreamingAPI(Literals.C_CONSUMER_KEY, Literals.C_CONSUMER_SECRET);
            m_bAPI.Authenticate(sAccessToken, sAccessSecret, sUsername, sPassword);
            m_sAPI.Authenticate(sAccessToken, sAccessSecret, sUsername, sPassword);

            m_usUserStream = m_sAPI.GetUserStream();
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

        public List<Status> Statuses
        {
            get { return m_lstStatuses; }
        }

        public List<DirectMessage> DirectMessages
        {
            get { return m_ldmDirectMessages; }
        }

        public void ToFile(ref StreamWriter swWriter)
        {
            //write certain number of tweets and DMs out to disk, wrapped in account envelope
        }
    }

    public class AccountList : List<Account>
    {
        public delegate void AccountHandler(object sender, Account actSubject);

        public event EventHandler AccountSwitched;
        //public event AccountHandler AccountConnected;
        public event AccountHandler AccountAdded;
        public event AccountHandler AccountRemoved;

        private const int C_NULL_ACCOUNT_INDEX = -1;
        private int m_iActiveAccountIndex = C_NULL_ACCOUNT_INDEX;

        public AccountList() : base() { }

        public void Add(string sAccessToken, string sAccessSecret, string sUsername = "", string sPassword = "")
        {
            Account actNew = new Account(sAccessToken, sAccessSecret, sUsername, sPassword);
            base.Add(actNew);

            //if there weren't any accounts in here before, set the newly created account as the active account
            if (base.Count == 1)
                SwitchAccount(0);

            if (AccountAdded != null)
                AccountAdded(this, actNew);
        }

        public new void Add(Account actNew)
        {
            base.Add(actNew);

            if (AccountAdded != null)
                AccountAdded(this, actNew);
        }

        public new void RemoveAt(int iIndex)
        {
            Account actReturn = base[iIndex];
            base.RemoveAt(iIndex);

            if (AccountRemoved != null)
                AccountRemoved(this, actReturn);
        }

        public new void Remove(Account actToRemove)
        {
            Account actReturn = base[base.IndexOf(actToRemove)];
            base.Remove(actToRemove);

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
        public void LoadFromFile(string sFile)
        {
            if (File.Exists(sFile))
            {
                StreamReader srReader = new StreamReader(sFile);
                string sAccessToken = srReader.ReadLine();
                string sAccessSecret = srReader.ReadLine();

                this.Add(sAccessToken, sAccessSecret, "camertron");
                srReader.Close();
            }
        }

        //@TODO
        public void ToFile(string sFile)
        {
            StreamWriter swWriter = new StreamWriter(sFile);

            //write data members for AccountList here (there might not be any)

            for (int i = 0; i < base.Count; i ++)
                base[i].ToFile(ref swWriter);

            swWriter.Close();
        }
    }
}
