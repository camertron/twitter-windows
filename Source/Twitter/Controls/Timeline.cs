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
        private bool m_bScrolledToTop = true;  //whether or not the user has scrolled to the top of the timeline
        private LinearMotionAnimation m_lmaMotion = null;
        private int m_iAnimateTimeElapsed = 0;

        public Timeline()
        {
            InitializeComponent();

            m_stsControls = new Stack<TimelineStatus>();
        }

        public void Push(Status stToAdd)
        {
            TimelineStatus tsNewStatus = new TimelineStatus(stToAdd);
            TimelineStatus tsOldTop = null;

            if (m_stsControls.Count > 0)
                tsOldTop = m_stsControls.Peek();

            this.Controls.Add(tsNewStatus);
            m_stsControls.Push(tsNewStatus);
            tsNewStatus.UpdateLayout();
            tsNewStatus.BackColor = this.BackColor;
            HookupEvents(tsNewStatus);

            OnResize(EventArgs.Empty);
            m_iTotalControlHeight += tsNewStatus.Height;

            if (m_bScrolledToTop)
            {
                if (tsOldTop != null)
                    tsNewStatus.Top = tsOldTop.Top - tsNewStatus.Height;
                else
                    tsNewStatus.Top = -(tsNewStatus.Height);
            }

            tsNewStatus.Visible = true;
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
            if ((m_stsControls != null) && (m_bScrolledToTop))
            {
                TimelineStatus tsTop = m_stsControls.Peek();
                m_lmaMotion = new LinearMotionAnimation(new Point(0, tsTop.Top), new Point(0, 0), 40, LinearMotionAnimation.MotionType.EaseIn);
                m_iAnimateTimeElapsed = 0;
                tmrTweetAnimate.Enabled = true;
            }
        }

        protected override void OnResize(EventArgs e)
        {
            //don't change the height - UpdateLayout will do that every time a tweet is added/removed
            this.Height = m_iTotalControlHeight;

            if (m_stsControls != null)
            {
                Stack<TimelineStatus>.Enumerator stsEnum = m_stsControls.GetEnumerator();

                while (stsEnum.MoveNext())
                    stsEnum.Current.Width = this.Width;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Always)]
        public bool ScrolledToTop
        {
            get { return m_bScrolledToTop; }
            set { m_bScrolledToTop = value; }
        }

        private void tmrTweetAnimate_Tick(object sender, EventArgs e)
        {
            if (m_lmaMotion != null)
            {
                Stack<TimelineStatus>.Enumerator stsEnum = m_stsControls.GetEnumerator();
                TimelineStatus tsPrev = null;
  
                stsEnum.MoveNext();
                tsPrev = stsEnum.Current;
                stsEnum.Current.Top = m_lmaMotion.PositionForTime(m_iAnimateTimeElapsed).Y;

                while (stsEnum.MoveNext())
                {
                    stsEnum.Current.Top = tsPrev.Bottom;
                    stsEnum.Current.Width = this.Width;
                    stsEnum.Current.Left = 0;
                    stsEnum.Current.Invalidate();
                    tsPrev = stsEnum.Current;
                }
            }

            m_iAnimateTimeElapsed ++;

            if (m_iAnimateTimeElapsed > m_lmaMotion.Duration)
                tmrTweetAnimate.Enabled = false;
        }
    }
}
