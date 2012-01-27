using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hammock.Authentication.OAuth;

namespace Twitter
{
    public class TwitterController
    {
        private static TwitterController c_objInstance = null;
        private static object c_lock = typeof(TwitterController);

        private AccountList m_aclAccounts;

        private TwitterController()
        {
            m_aclAccounts = new AccountList();
        }

        public static TwitterController GetController()
        {
            lock (c_lock)
            {
                if (c_objInstance == null)
                    c_objInstance = new TwitterController();
            }

            return c_objInstance;
        }

        public AccountList Accounts
        {
            get { return m_aclAccounts; }
        }

        public Account ActiveAccount
        {
            get { return m_aclAccounts.ActiveAccount; }
        }

        public void ConnectActiveAccount()
        {
            //connect default account (which will populate the timeline)
            //this null check is necessary because this method is used by the forms designer
            //as well as during runtime
            if (m_aclAccounts.ActiveAccount != null)
            {
                if (Literals.C_ENVIRONMENT == Env.Production)
                    m_aclAccounts.ActiveAccount.Connect();
            }
        }

        public void ConnectAllAccounts()
        {
            for (int i = 0; i < m_aclAccounts.Count; i++)
                m_aclAccounts[i].Connect();
        }

        public void AddNewAccount(OAuthCredentials oaCredentials)
        {
            AddNewAccount(new Account(oaCredentials));
        }

        public void AddNewAccount(Account actNewAccount)
        {
            m_aclAccounts.Add(actNewAccount);
            Save();
            actNewAccount.Connect();
        }

        public void Save()
        {
            //save accounts
            m_aclAccounts.Save();
            //save anything else...
        }
    }
}
