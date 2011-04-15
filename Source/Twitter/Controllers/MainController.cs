using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Twitter.API;
using Twitter.API.Basic;

namespace Twitter
{
    //prevents this class from being opened in the forms designer when it's double-clicked
    [System.ComponentModel.DesignerCategory("")]
    public class MainController : Form
    {
        private AccountList m_aclAccounts;

        public MainController() : base()
        {
            m_aclAccounts = new AccountList();

            m_aclAccounts.AccountSwitched += new EventHandler(m_aclAccounts_AccountSwitched);
            m_aclAccounts.AccountAdded += new AccountList.AccountHandler(m_aclAccounts_AccountAdded);
            m_aclAccounts.AccountRemoved += new AccountList.AccountHandler(m_aclAccounts_AccountRemoved);

            m_aclAccounts.LoadFromFile(Literals.C_ACCOUNT_FILE);
        }

        #region Local Events

        private void m_aclAccounts_AccountSwitched(object sender, EventArgs e) { OnAccountSwitched(); }

        private void m_aclAccounts_AccountRemoved(object sender, Account actSubject)
        {
            actSubject.UserStream.Receive -= new API.Streaming.UserStream.ReceiveHandler(Account_UserStream_Receive);
        }

        private void m_aclAccounts_AccountAdded(object sender, Account actSubject)
        {
            actSubject.UserStream.Receive += new API.Streaming.UserStream.ReceiveHandler(Account_UserStream_Receive);
        }

        private void Account_UserStream_Receive(object sender, API.Json.JsonDocument jdData)
        {
            if (jdData.Root.IsNode())
            {
                if (jdData.Root.ToNode().ContainsKey("friends"))
                {
                    //this is the friends list that's sent at the beginning of each userstream connection
                }
                else if (jdData.Root.ToNode().ContainsKey("retweeted"))
                {
                    //it's a tweet!
                    Status stNewStatus = new Status(jdData.Root.ToNode());
                    OnTweetReceived(stNewStatus);
                    m_aclAccounts.ActiveAccount.Statuses.Add(stNewStatus);
                }
                else if (jdData.Root.ToNode().ContainsKey("recipient_id") && jdData.Root.ToNode().ContainsKey("sender_id"))
                {
                    DirectMessage dmNewMessage = new DirectMessage(jdData.Root.ToNode());
                    OnDirectMessageReceived(dmNewMessage);
                    m_aclAccounts.ActiveAccount.DirectMessages.Add(dmNewMessage);
                }
            }
        }

        #endregion

        #region Event-based view functions

        protected virtual void OnAccountSwitched() { }
        protected virtual void OnTweetReceived(Status stReceived) { }
        protected virtual void OnDirectMessageReceived(DirectMessage dmReceived) { }

        #endregion

        protected AccountList Accounts
        {
            get { return m_aclAccounts; }
        }

        protected void CallbackCheckSucceeded(string sMessage, string sTitle, APICallbackArgs acArgs)
        {
            if (!acArgs.Succeeded)
                MessageBox.Show(sMessage + "\n\nTwitter said: \"" + acArgs.ErrorMessage + "\"", sTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
    }
}
