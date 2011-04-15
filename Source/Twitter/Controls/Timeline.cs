using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Twitter.API.Basic;

namespace Twitter.Controls
{
    public partial class Timeline : UserControl
    {
        public delegate void StatusTextClickedHandler(object sender, Status stStatus, TweetTextElement tstElement);
        public delegate void StatusOptionClickedHandler(object sender, TimelineStatus tsControl, Status stStatus);

        public event StatusTextClickedHandler StatusTextClicked;
        public event StatusOptionClickedHandler RetweetClicked;
        public event StatusOptionClickedHandler QuoteTweetClicked;
        public event StatusOptionClickedHandler FavoriteClicked;
        public event StatusOptionClickedHandler ReplyClicked;
        public event StatusOptionClickedHandler ConversationClicked;

        private Stack<TimelineStatus> m_stsControls;
        private int m_iTotalControlHeight = 0;

        public Timeline()
        {
            InitializeComponent();

            m_stsControls = new Stack<TimelineStatus>();
        }

        public void Push(Status stToAdd)
        {
            TimelineStatus tsNewStatus = new TimelineStatus(stToAdd);

            m_stsControls.Push(tsNewStatus);
            this.Controls.Add(tsNewStatus);
            tsNewStatus.Width = this.Width;
            tsNewStatus.UpdateLayout();
            tsNewStatus.BackColor = this.BackColor;
            tsNewStatus.Visible = true;
            HookupEvents(tsNewStatus);

            UpdateLayout();
        }

        private void HookupEvents(TimelineStatus tsNewStatus)
        {
            tsNewStatus.TextElementClicked += new TweetTextField.TextElementClickHandler(this.Status_TextElementClicked);
            tsNewStatus.RetweetClicked += new EventHandler(Status_RetweetClick);
            tsNewStatus.QuoteTweetClicked += new EventHandler(Status_QuoteTweetClick);
            tsNewStatus.FavoriteClicked += new EventHandler(Status_FavoriteClick);
            tsNewStatus.ReplyClicked += new EventHandler(Status_ReplyClick);
            tsNewStatus.ConversationClicked += new EventHandler(Status_ConversationClick);
        }

        private void Status_ConversationClick(object sender, EventArgs e)
        {
            if (ConversationClicked != null)
                ConversationClicked(this, (TimelineStatus)sender, ((TimelineStatus)sender).StatusObj);
        }

        private void Status_ReplyClick(object sender, EventArgs e)
        {
            if (ReplyClicked != null)
                ReplyClicked(this, (TimelineStatus)sender, ((TimelineStatus)sender).StatusObj);
        }

        private void Status_FavoriteClick(object sender, EventArgs e)
        {
            if (FavoriteClicked != null)
                FavoriteClicked(this, (TimelineStatus)sender, ((TimelineStatus)sender).StatusObj);
        }

        private void Status_QuoteTweetClick(object sender, EventArgs e)
        {
            if (QuoteTweetClicked != null)
                QuoteTweetClicked(this, (TimelineStatus)sender, ((TimelineStatus)sender).StatusObj);
        }

        private void Status_RetweetClick(object sender, EventArgs e)
        {
            if (RetweetClicked != null)
                RetweetClicked(this, (TimelineStatus)sender, ((TimelineStatus)sender).StatusObj);
        }

        private void Status_TextElementClicked(object sender, TweetTextElement tstElement)
        {
            if (StatusTextClicked != null)
                StatusTextClicked(this, ((TimelineStatus)sender).StatusObj, tstElement);
        }   

        //returns final control height
        public void UpdateLayout()
        {
            if (m_stsControls != null)
            {
                Stack<TimelineStatus>.Enumerator stsEnum = m_stsControls.GetEnumerator();
                int iY = 0;

                while (stsEnum.MoveNext())
                {
                    stsEnum.Current.Top = iY;
                    stsEnum.Current.Width = this.Width;
                    stsEnum.Current.Left = 0;
                    stsEnum.Current.Invalidate();
                    iY += stsEnum.Current.Height;
                }

                m_iTotalControlHeight = iY;
                this.Height = iY;
            }
        }

        protected override void OnResize(EventArgs e)
        {
            //don't change the height - UpdateLayout will do that every time a tweet is added/removed
            this.Height = m_iTotalControlHeight;
        }
    }
}
