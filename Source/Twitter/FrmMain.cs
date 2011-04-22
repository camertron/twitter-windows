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
using Twitter.Json;
using System.IO;

namespace Twitter
{
    //overrides a similar directive in MainView that prevents this class from being opened in the forms designer when it's double-clicked
    [System.ComponentModel.DesignerCategory("Form")]
    public partial class FrmMain : MainController
    {
        private enum SidebarButtonEnum
        {
            Timeline = 1,
            Replies = 2,
            DirectMessages = 3,
            Lists = 4,
            People = 5,
            Search = 6
        }

        private SidebarButton m_sbbSelected;
        private SidebarButtonEnum m_sbeSelected = SidebarButtonEnum.Timeline;
        private FrmTweet m_ftTweetForm;
        private FrmPreferences m_fpPrefForm;
        private int m_iTimelineChangeElapsed = 0;
        private LinearMotionAnimation m_lmaTimelineChangeAnim = null;

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
            tsbTimelineScroller.Scroll += new ScrollEventHandler(tsbTimelineScroller_Scroll);
            tmlTimeline.ScrolledToTop = true;

            //@TODO: do this for testing purposes only
            //UserTimeline utLine = new UserTimeline(JsonParser.GetParser().ParseFile("../../../../Documents/test/tweets/tweets_short.json").Root.ToList());
            //for (int i = 0; i < utLine.Statuses.Count; i++)
               // Account_UserStream_Receive(this, new JsonDocument(utLine.Statuses[i].Object), API.Streaming.UserStream.ReceiveType.Tweet);
                //tmlTimeline.Push(utLine.Statuses[i]);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            int iNewTop = tmlTimeline.Top + (e.Delta / (SystemInformation.MouseWheelScrollDelta / 20));

            if (iNewTop > 0)
            {
                tmlTimeline.Top = 0;
                tmlTimeline.ScrolledToTop = true;
            }
            else if (iNewTop < (this.ClientSize.Height - tmlTimeline.InternalHeight))
            {
                tmlTimeline.Top = this.ClientSize.Height - tmlTimeline.InternalHeight;
                tmlTimeline.ScrolledToTop = false;
            }
            else
            {
                tmlTimeline.Top = iNewTop;
                tmlTimeline.ScrolledToTop = false;
            }

            UpdateScrollBar();
        }

        private void tsbTimelineScroller_Scroll(object sender, ScrollEventArgs e)
        {
            float fPercentScrolled = (float)e.NewValue / 100.0f;
            tmlTimeline.Top = -(int)((tmlTimeline.InternalHeight - this.ClientSize.Height) * fPercentScrolled);
            tmlTimeline.ScrolledToTop = (e.Type == ScrollEventType.First);
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

        #region View Events

        protected override void OnTweetReceived(Status stReceived)
        {
            tmlTimeline.Push(stReceived, Accounts.ActiveAccount.BasicAPI);
            UpdateScrollBar();
            OnResize(EventArgs.Empty);
        }

        protected override void OnDirectMessageReceived(DirectMessage dmReceived)
        {
        }

        protected override void OnAccountSetAvatar(int iAccountIndex, Bitmap bmpAvatar)
        {
            //eventually this function will use iAccountIndex to set the right avatar
            pbAvatar.Image = bmpAvatar;
        }

        protected override void OnReplyReceived(Status stReceived)
        {
            tmlReplyTimeline.Push(stReceived, Accounts.ActiveAccount.BasicAPI);
            UpdateScrollBar();
            OnResize(EventArgs.Empty);
        }

        #endregion

        private void tmlTimeline_StatusTextClicked(object sender, Status stStatus, StatusTextElement stElement)
        {
            switch (stElement.Type)
            {
                case StatusTextElement.StatusTextElementType.URL:
                    //open browser - let the windows shell handle it
                    System.Diagnostics.Process.Start(stElement.Text);
                    break;

                case StatusTextElement.StatusTextElementType.Hashtag:
                    //show the view for that hashtag
                    break;

                case StatusTextElement.StatusTextElementType.ScreenName:
                    //show the view for that screen name
                    break;
            }
        }

        protected override void OnResize(EventArgs e)
        {
            tsbTimelineScroller.Left = this.ClientSize.Width - tsbTimelineScroller.Width;
            tsbTimelineScroller.Top = 0;
            tsbTimelineScroller.Height = this.ClientSize.Height;

            //timeline should be at least as tall as the form
            if (tmlTimeline.InternalHeight > this.ClientSize.Height)
                tmlTimeline.Height = tmlTimeline.InternalHeight;
            else
                tmlTimeline.Height = this.ClientSize.Height;

            tmlTimeline.Left = pnlSidebar.Right;
            UpdateScrollBar();
            
            if (tsbTimelineScroller.Visible)
                tmlTimeline.Width = this.ClientSize.Width - (pnlSidebar.Width + tsbTimelineScroller.Width);
            else
                tmlTimeline.Width = this.ClientSize.Width - pnlSidebar.Width;

            if (tmlReplyTimeline.InternalHeight > this.ClientSize.Height)
                tmlReplyTimeline.Height = tmlReplyTimeline.InternalHeight;
            else
                tmlReplyTimeline.Height = this.ClientSize.Height;

            tmlReplyTimeline.Width = tmlTimeline.Width;
            tmlReplyTimeline.Location = tmlTimeline.Location;

            pbLarry.Top = pnlSidebar.Height - (pbLarry.Height + 10);
            pnlSidebar.Height = this.ClientSize.Height;
        }

        private void sbbTimeline_Click(object sender, EventArgs e)
        {
            m_sbbSelected.Selected = false;
            sbbTimeline.Selected = true;
            m_sbbSelected = sbbTimeline;
            m_sbeSelected = SidebarButtonEnum.Timeline;
            SidebarChanged();
        }

        private void sbbReplies_Click(object sender, EventArgs e)
        {
            m_sbbSelected.Selected = false;
            sbbReplies.Selected = true;
            m_sbbSelected = sbbReplies;
            m_sbeSelected = SidebarButtonEnum.Replies;
            SidebarChanged();
        }

        private void sbbMessages_Click(object sender, EventArgs e)
        {
            m_sbbSelected.Selected = false;
            sbbMessages.Selected = true;
            m_sbbSelected = sbbMessages;
            m_sbeSelected = SidebarButtonEnum.DirectMessages;
            SidebarChanged();
        }

        private void sbbLists_Click(object sender, EventArgs e)
        {
            m_sbbSelected.Selected = false;
            sbbLists.Selected = true;
            m_sbbSelected = sbbLists;
            SidebarChanged();
        }

        private void sbbPeople_Click(object sender, EventArgs e)
        {
            m_sbbSelected.Selected = false;
            sbbPeople.Selected = true;
            m_sbbSelected = sbbPeople;
            m_sbeSelected = SidebarButtonEnum.People;
            SidebarChanged();
        }

        private void sbbSearch_Click(object sender, EventArgs e)
        {
            m_sbbSelected.Selected = false;
            sbbSearch.Selected = true;
            m_sbbSelected = sbbSearch;
            m_sbeSelected = SidebarButtonEnum.Search;
            SidebarChanged();
        }

        private void SidebarChanged()
        {
            Timeline tmlCur = TimelineForSelectedSidebarButton();

            tmlCur.BringToFront();
            pnlSidebar.BringToFront();
            tmlCur.Visible = true;
            tmlCur.Left = this.ClientSize.Width;
            m_iTimelineChangeElapsed = 0;
            m_lmaTimelineChangeAnim = new LinearMotionAnimation(new Point(this.ClientSize.Width, 0), new Point(pnlSidebar.Right, 0), 30, LinearMotionAnimation.MotionType.EaseIn);
            UpdateScrollBar();
            tmrTimelineChange.Enabled = true;
        }

        private Timeline TimelineForSelectedSidebarButton()
        {
            switch (m_sbeSelected)
            {
                case SidebarButtonEnum.Timeline:
                    return tmlTimeline;
                case SidebarButtonEnum.Replies:
                    return tmlReplyTimeline;
                default:
                    return null;
            }
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
            Timeline tmlCur = TimelineForSelectedSidebarButton();

            if (tmlCur.InternalHeight > this.ClientSize.Height)
            {
                int iLargeChange = (int)(((float)this.ClientSize.Height / (float)tmlCur.InternalHeight) * 100.0f);
                tsbTimelineScroller.LargeChange = iLargeChange;

                float fPercent = Math.Abs((float)tmlCur.Top) / (float)(tmlCur.InternalHeight - this.ClientSize.Height);
                tsbTimelineScroller.Value = (int)(tsbTimelineScroller.Max * fPercent);

                tsbTimelineScroller.Visible = true;
            }
            else
            {
                tsbTimelineScroller.Visible = false;
            }
        }

        private void tmrTimelineChange_Tick(object sender, EventArgs e)
        {
            Timeline tmlCur = TimelineForSelectedSidebarButton();

            if (m_lmaTimelineChangeAnim == null)
                tmrTimelineChange.Enabled = false;
            else
            {
                tmlCur.Left = m_lmaTimelineChangeAnim.PositionForTime(m_iTimelineChangeElapsed).X;
                m_iTimelineChangeElapsed++;
            }

            if (m_iTimelineChangeElapsed > m_lmaTimelineChangeAnim.Duration)
            {
                tsbTimelineScroller.BringToFront();
                tmrTimelineChange.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*UserTimeline utLine = new UserTimeline(JsonParser.GetParser().ParseFile("../../../../Documents/test/tweets/tweets_single.json").Root.ToList());
            for (int i = 0; i < utLine.Statuses.Count; i++)
                Account_UserStream_Receive(this, new JsonDocument(utLine.Statuses[i].Object), API.Streaming.UserStream.ReceiveType.Tweet);*/

            StreamReader srReader = new StreamReader("../../../../Documents/test/tweets/tweets2.json");
            while (!srReader.EndOfStream)
            {
                UserTimeline utLine = new UserTimeline(JsonParser.GetParser().ParseString(srReader.ReadLine()).Root.ToList());
                for (int i = 0; i < utLine.Statuses.Count; i++)
                    Account_UserStream_Receive(this, new JsonDocument(utLine.Statuses[i].Object), API.Streaming.UserStream.ReceiveType.Tweet);
            }
        }
    }
}
