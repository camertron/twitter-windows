using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;
using Twitter.Json;
using Twitter.API;
using Twitter.API.Basic;
using Twitter.API.Streaming;

namespace Twitter
{
    //prevents this class from being opened in the forms designer when it's double-clicked
    [System.ComponentModel.DesignerCategory("")]
    public class MainController : Form
    {
        protected TwitterController m_twController;

        public MainController() : base()
        {
            m_twController = TwitterController.GetController();

            m_twController.Accounts.AccountSwitched += new EventHandler(m_aclAccounts_AccountSwitched);
            m_twController.Accounts.AccountAdded += new AccountList.AccountHandler(m_aclAccounts_AccountAdded);
            m_twController.Accounts.AccountRemoved += new AccountList.AccountHandler(m_aclAccounts_AccountRemoved);
            m_twController.Accounts.UserObjectReceived += new AccountList.AccountHandler(m_aclAccounts_UserObjectReceived);

            m_twController.Accounts.Load();
            m_twController.ConnectActiveAccount();
        }

        #region Local Events

        private void m_aclAccounts_UserObjectReceived(object sender, Account actSubject)
        {
            AsyncContentManager.GetManager().RequestImage(actSubject.UserObject["profile_image_url"].ToString(), AccountGetAvatarCallback, actSubject);
        }

        private void AccountGetAvatarCallback(object sender, Bitmap bmpImage, object objContext)
        {
            if (objContext != null)
            {
                ((Account)objContext).Avatar = bmpImage;
                OnAccountSetAvatar(m_twController.Accounts.IndexOf((Account)objContext), Imaging.RoundAvatarCorners(bmpImage));
            }
        }

        private void m_aclAccounts_AccountSwitched(object sender, EventArgs e) { OnAccountSwitched(); }

        private void m_aclAccounts_AccountRemoved(object sender, Account actSubject)
        {
            actSubject.UserStream.Receive -= new API.Streaming.UserStream.ReceiveHandler(Account_UserStream_Receive);
        }

        private void m_aclAccounts_AccountAdded(object sender, Account actSubject)
        {
            actSubject.UserStream.Receive += new API.Streaming.UserStream.ReceiveHandler(Account_UserStream_Receive);
        }

        protected void Account_UserStream_Receive(object sender, JsonDocument jdData, UserStream.ReceiveType rtRcvType)
        {
            switch (rtRcvType)
            {
                case UserStream.ReceiveType.Tweet:
                case UserStream.ReceiveType.Reply:
                    Status stNewTweet = new Status(jdData.Root.ToNode());

                    if (!m_twController.ActiveAccount.Statuses.Contains(stNewTweet))
                    {
                        m_twController.ActiveAccount.Statuses.Add(stNewTweet);

                        if (rtRcvType == UserStream.ReceiveType.Tweet)
                            OnTweetReceived(stNewTweet);
                        else
                            OnReplyReceived(stNewTweet);
                    }

                    break;

                case UserStream.ReceiveType.DirectMessage:
                    DirectMessage dmNewMessage = new DirectMessage(jdData.Root.ToNode());
                    m_twController.ActiveAccount.DirectMessages.Add(dmNewMessage);
                    OnDirectMessageReceived(dmNewMessage);
                    break;
            }
        }

        #endregion

        #region Event-based view functions

        protected virtual void OnAccountSwitched() { }
        protected virtual void OnAccountSetAvatar(int iAccountIndex, Bitmap bmpAvatar) { }
        protected virtual void OnTweetReceived(Status stReceived) { }
        protected virtual void OnDirectMessageReceived(DirectMessage dmReceived) { }
        protected virtual void OnReplyReceived(Status stReceived) { }
        protected virtual void OnDelete(int iStatusId) { }

        #endregion

        protected AccountList Accounts
        {
            get { return m_twController.Accounts; }
        }

        protected void CallbackCheckSucceeded(string sMessage, string sTitle, APICallbackArgs acArgs)
        {
            if (!acArgs.Succeeded)
                MessageBox.Show(sMessage + "\n\nTwitter said: \"" + acArgs.ErrorMessage + "\"", sTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
    }
}
