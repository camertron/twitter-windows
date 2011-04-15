using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Twitter.API;
using Twitter.API.Basic;
using Twitter.Controls;

namespace Twitter
{
    //overrides a similar directive in MainView that prevents this class from being opened in the forms designer when it's double-clicked
    [System.ComponentModel.DesignerCategory("Form")]
    public partial class FrmMain : MainController
    {
        private SidebarButton m_sbbSelected;
        private FrmTweet m_ftTweetForm;
        private FrmPreferences m_fpPrefForm;

        public FrmMain()
        {
            InitializeComponent();

            //assignments
            m_sbbSelected = sbbTimeline;

            //object creation
            m_ftTweetForm = new FrmTweet();
            m_fpPrefForm = new FrmPreferences();

            //event hookups
            tmlTimeline.StatusTextClicked += new Timeline.StatusTextClickedHandler(tmlTimeline_StatusTextClicked);
            tmlTimeline.RetweetClicked += new Timeline.StatusOptionClickedHandler(tmlTimeline_RetweetClicked);
            tmlTimeline.FavoriteClicked += new Timeline.StatusOptionClickedHandler(tmlTimeline_FavoriteClicked);
            tmlTimeline.QuoteTweetClicked += new Timeline.StatusOptionClickedHandler(tmlTimeline_QuoteTweetClicked);
            tmlTimeline.ReplyClicked += new Timeline.StatusOptionClickedHandler(tmlTimeline_ReplyClicked);

            m_ftTweetForm.TweetClicked += new FrmTweet.TweetClickedEventHandler(m_ftTweetForm_TweetClicked);
        }

        private void tmlTimeline_ReplyClicked(object sender, TimelineStatus tsControl, Status stStatus)
        {
            m_ftTweetForm.ShowDialog("@" + stStatus.User["screen_name"].ToString() + ": ");
        }

        private void tmlTimeline_QuoteTweetClicked(object sender, TimelineStatus tsControl, Status stStatus)
        {
            m_ftTweetForm.ShowDialog("\"@" + stStatus.User["screen_name"].ToString() + ": " + stStatus["text"].ToString() + "\"");
        }

        private void tmlTimeline_FavoriteClicked(object sender, TimelineStatus tsControl, Status stStatus)
        {
            tsControl.Favorite = !tsControl.Favorite;

            if (tsControl.Favorite)
                Accounts.ActiveAccount.BasicAPI.CreateFavorite(FavoriteCreateCallback, null, stStatus["id"].ToString());
            else
                Accounts.ActiveAccount.BasicAPI.DestroyFavorite(FavoriteDestroyCallback, null, stStatus["id"].ToString());
        }

        private void tmlTimeline_RetweetClicked(object sender, TimelineStatus tsControl, Status stStatus)
        {
            DialogResult drAnswer = MessageBox.Show("Retweet to your followers?\n\n" + stStatus["text"], "Retweet", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

            if (drAnswer == System.Windows.Forms.DialogResult.Yes)
                Accounts.ActiveAccount.BasicAPI.Retweet(RetweetCallback, null, stStatus["id"].ToString());
        }

        #region API Callbacks

        private void UpdateStatusCallback(APICallbackArgs acArgs)
        {
            CallbackCheckSucceeded("There was an error posting your tweet.", "Tweet Error", acArgs);
        }

        private void RetweetCallback(APICallbackArgs acArgs)
        {
            CallbackCheckSucceeded("There was an error posting your retweet.", "Retweet Error", acArgs);
        }

        private void FavoriteCreateCallback(APICallbackArgs acArgs)
        {
            CallbackCheckSucceeded("There was an error favoriting that tweet.", "Favorite Error", acArgs);
        }

        private void FavoriteDestroyCallback(APICallbackArgs acArgs)
        {
            CallbackCheckSucceeded("There was an error un-favoriting that tweet.", "Un-favorite Error", acArgs);
        }

        #endregion

        #region Streaming Events

        protected override void OnTweetReceived(Status stReceived)
        {
            tmlTimeline.Push(stReceived);
            UpdateScrollBar();
        }

        protected override void OnDirectMessageReceived(DirectMessage dmReceived)
        {
        }

        #endregion

        private void tmlTimeline_StatusTextClicked(object sender, Status stStatus, TweetTextElement tstElement)
        {
            switch (tstElement.Type)
            {
                case TweetTextElement.TextElementType.URL:
                    //open browser - let the windows shell handle it
                    System.Diagnostics.Process.Start(tstElement.Text);
                    break;

                case TweetTextElement.TextElementType.Hashtag:
                    //show the view for that hashtag
                    break;

                case TweetTextElement.TextElementType.ScreenName:
                    //show the view for that screen name
                    break;
            }
        }

        protected override void OnResize(EventArgs e)
        {
            tsbTimelineScroller.Left = this.ClientSize.Width - tsbTimelineScroller.Width;
            tsbTimelineScroller.Top = 0;
            tsbTimelineScroller.Height = this.ClientSize.Height;

            UpdateScrollBar();

            pnlSidebar.Height = this.ClientSize.Height;
            tmlTimeline.Left = pnlSidebar.Right;
            tmlTimeline.Height = this.ClientSize.Height;

            //if (tsbTimelineScroller.Visible)
                tmlTimeline.Width = this.ClientSize.Width - (pnlSidebar.Width + tsbTimelineScroller.Width);
            //else
                //tmlTimeline.Width = this.ClientSize.Width - pnlSidebar.Width;

            pbLarry.Top = pnlSidebar.Height - (pbLarry.Height + 10);
        }

        private void sbbTimeline_Click(object sender, EventArgs e)
        {
            m_sbbSelected.Selected = false;
            sbbTimeline.Selected = true;
            m_sbbSelected = sbbTimeline;
        }

        private void sbbReplies_Click(object sender, EventArgs e)
        {
            m_sbbSelected.Selected = false;
            sbbReplies.Selected = true;
            m_sbbSelected = sbbReplies;
        }

        private void sbbMessages_Click(object sender, EventArgs e)
        {
            m_sbbSelected.Selected = false;
            sbbMessages.Selected = true;
            m_sbbSelected = sbbMessages;
        }

        private void sbbLists_Click(object sender, EventArgs e)
        {
            m_sbbSelected.Selected = false;
            sbbLists.Selected = true;
            m_sbbSelected = sbbLists;
        }

        private void sbbPeople_Click(object sender, EventArgs e)
        {
            m_sbbSelected.Selected = false;
            sbbPeople.Selected = true;
            m_sbbSelected = sbbPeople;
        }

        private void sbbSearch_Click(object sender, EventArgs e)
        {
            m_sbbSelected.Selected = false;
            sbbSearch.Selected = true;
            m_sbbSelected = sbbSearch;
        }

        private void pbLarry_Click(object sender, EventArgs e)
        {
            cmsLarry.Show(Cursor.Position);
        }

        private void tsmiNewTweet_Click(object sender, EventArgs e)
        {
            m_ftTweetForm.ShowDialog("");
        }

        private void m_ftTweetForm_TweetClicked(object sender, string sTweetText)
        {
            Accounts.ActiveAccount.BasicAPI.UpdateStatus(UpdateStatusCallback, null, sTweetText);
        }

        private void tsmiPreferences_Click(object sender, EventArgs e)
        {
            m_fpPrefForm.ShowDialog();
        }

        protected void UpdateScrollBar()
        {
            if (tmlTimeline.Height > this.ClientSize.Height)
            {
                int iLargeChange = (int)(((float)this.ClientSize.Height / (float)tmlTimeline.Height) * 100.0f);
                tsbTimelineScroller.LargeChange = iLargeChange;
                //tsbTimelineScroller.Visible = true;
            }
            else
            {
                //tsbTimelineScroller.Visible = false;
            }
        }
    }
}
