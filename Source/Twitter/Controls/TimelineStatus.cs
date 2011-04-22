using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Twitter.API;
using Twitter.API.Basic;

namespace Twitter.Controls
{
    public partial class TimelineStatus : UserControl
    {
        private const int C_AVATAR_DIMENSIONS = 48;
        private const int C_CONTROL_MARGIN = 10;
        private const int C_AVATAR_TEXT_MARGIN = 10;
        private const int C_AVATAR_HEIGHT = 48;
        private const int C_ACTION_BUTTON_SHADE = 50;
        private const int C_FROM_USER_HEIGHT = 15;

        private string m_sFromUser;
        private Status m_stStatusObj;
        private bool m_bDisplayConversationButton;

        private Bitmap m_bmpAvatar = null;
        private Font m_fntFont;
        private Font m_fntFromUser;
        private Pen m_pnBorderPen;
        private SolidBrush m_sbFromUser;

        public event TweetTextField.TextElementClickHandler TextElementClicked;
        public event EventHandler RetweetClicked;
        public event EventHandler QuoteTweetClicked;
        public event EventHandler FavoriteClicked;
        public event EventHandler ReplyClicked;
        public event EventHandler ConversationClicked;

        public TimelineStatus(Status stFrom, BasicAPI bAPI)
        {
            InitializeComponent();

            m_bDisplayConversationButton = false;
            m_fntFont = new Font("Arial", 10);
            m_fntFromUser = new Font("Arial", 10, FontStyle.Bold);
            m_sbFromUser = new SolidBrush(Color.Black);
            m_pnBorderPen = new Pen(Color.FromArgb(220, 220, 220));

            m_stStatusObj = stFrom;
            ttfTextField.UpdateFromStatus(m_stStatusObj);
            Favorite = Boolean.Parse(stFrom["favorited"].ToString());

            if (m_stStatusObj.IsRetweet)
            {
                if (m_stStatusObj.RetweetedStatus == null)
                {
                    m_sFromUser = "";
                    bAPI.LookupUser(UserLookupCallback, null, new List<string>(new string[] { "" }));
                }
                else
                {
                    m_sFromUser = m_stStatusObj.RetweetedStatus.User["screen_name"].ToString();
                    AsyncContentManager.GetManager().RequestImage(m_stStatusObj.RetweetedStatus["profile_image_url"].ToString(), AvatarCallback);
                }
            }
            else
            {
                m_sFromUser = m_stStatusObj.User["screen_name"].ToString();
                AsyncContentManager.GetManager().RequestImage(m_stStatusObj["profile_image_url"].ToString(), AvatarCallback);
            }

            ttfTextField.TextElementClicked += new TweetTextField.TextElementClickHandler(ttfTextField_TextElementClicked);

            abRetweet.Click += new EventHandler(abRetweet_Click);
            abFavorite.Click += new EventHandler(abFavorite_Click);
            abReply.Click += new EventHandler(abReply_Click);
            abConversation.Click += new EventHandler(abConversation_Click);
        }

        private void UserLookupCallback(APICallbackArgs acArgs)
        {
        }

        private void AvatarCallback(object sender, Bitmap bmpAvatar, object objContext)
        {
            //for some reason, a few of the avatars are weird sizes (I'm looking at you, TechCrunch)
            //this code resizes the avatar before its displayed so nothing overlaps and looks weird
            Bitmap bmpResized = new Bitmap(C_AVATAR_DIMENSIONS, C_AVATAR_DIMENSIONS);

            using (Graphics gCanvas = Graphics.FromImage(bmpResized))
                gCanvas.DrawImage(bmpAvatar, 0, 0, C_AVATAR_DIMENSIONS, C_AVATAR_DIMENSIONS);

            m_bmpAvatar = Imaging.RoundAvatarCorners(bmpResized);
            this.Invalidate();  //force redraw
        }

        private void abConversation_Click(object sender, EventArgs e)
        {
            if (ConversationClicked != null)
                ConversationClicked(this, e);
        }

        private void abReply_Click(object sender, EventArgs e)
        {
            if (ReplyClicked != null)
                ReplyClicked(this, e);
        }

        private void abFavorite_Click(object sender, EventArgs e)
        {
            if (FavoriteClicked != null)
                FavoriteClicked(this, e);

            UpdateStatusButtons();
        }

        private void abRetweet_Click(object sender, EventArgs e)
        {
            cmsRetweet.Show(Cursor.Position);
        }

        private void tsmiRetweet_Click(object sender, EventArgs e)
        {
            if (RetweetClicked != null)
                RetweetClicked(this, e);
        }

        private void tsmiQuoteTweet_Click(object sender, EventArgs e)
        {
            if (QuoteTweetClicked != null)
                QuoteTweetClicked(this, e);
        }

        private void ttfTextField_TextElementClicked(object sender, StatusTextElement stElement)
        {
            if (TextElementClicked != null)
                TextElementClicked(this, stElement);
        }

        public bool Favorite
        {
            get { return pbFavoriteBadge.Visible; }
            set { pbFavoriteBadge.Visible = value; }
        }

        public override Color BackColor
        {
            get { return base.BackColor; }
            set
            {
                base.BackColor = value;
                ttfTextField.BackColor = value;
            }
        }

        public Status StatusObj
        {
            get { return m_stStatusObj; }
        }

        public override string Text
        {
            get { return ttfTextField.Text; }
            set { ttfTextField.Text = value; }
        }

        [EditorBrowsable]
        public Bitmap Avatar
        {
            get { return m_bmpAvatar; }
            set { m_bmpAvatar = value; }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            abConversation.Visible = m_bDisplayConversationButton;
            abFavorite.Visible = true;
            abReply.Visible = true;
            abRetweet.Visible = true;
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            Point ptMouse = this.PointToClient(Cursor.Position);

            if (!(((ptMouse.X > 0) && (ptMouse.X < this.Width)) && ((ptMouse.Y > 0) && (ptMouse.Y < this.Height))))
            {
                abConversation.Visible = m_bDisplayConversationButton;
                abFavorite.Visible = false;
                abReply.Visible = false;
                abRetweet.Visible = false;
            }
        }

        protected override void OnResize(EventArgs e)
        {
            UpdateStatusButtons();

            ttfTextField.Left = C_AVATAR_DIMENSIONS + (C_CONTROL_MARGIN * 2);
            ttfTextField.Top = C_CONTROL_MARGIN + C_FROM_USER_HEIGHT;
            ttfTextField.Width = this.Width - (ttfTextField.Left + C_CONTROL_MARGIN);
            //ttfTextField.Height set automatically

            //set height based on height of TweetTextField
            if (ttfTextField.Bottom > (C_AVATAR_HEIGHT + (C_CONTROL_MARGIN * 2)))
                this.Height = ttfTextField.Bottom + C_CONTROL_MARGIN;
            else
                this.Height = C_AVATAR_HEIGHT + (C_CONTROL_MARGIN * 2);

            pbFavoriteBadge.Left = this.Width - pbFavoriteBadge.Width;
            pbFavoriteBadge.Top = 0;

            base.OnResize(e);
        }

        private void UpdateStatusButtons()
        {
            int iRtOffset = 10;

            if (pbFavoriteBadge.Visible)
                iRtOffset += 10;

            abRetweet.Left = this.Width - (abRetweet.Width + iRtOffset);
            abFavorite.Left = abRetweet.Left - (abFavorite.Width + 5);
            abReply.Left = abFavorite.Left - (abReply.Width + 5);
            abConversation.Left = abReply.Left - (abConversation.Width + 5);
        }

        public void UpdateLayout()
        {
            this.OnPaint(new PaintEventArgs(this.CreateGraphics(), new Rectangle(0, 0, this.Width, this.Height)));
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            int iX = C_CONTROL_MARGIN + C_AVATAR_DIMENSIONS + C_AVATAR_TEXT_MARGIN;
            int iY = C_CONTROL_MARGIN;

            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            if (m_bmpAvatar != null)
                e.Graphics.DrawImage(m_bmpAvatar, C_CONTROL_MARGIN, C_CONTROL_MARGIN);

            //tweet came from this user
            e.Graphics.DrawString(m_sFromUser, m_fntFromUser, m_sbFromUser, iX, iY - 2);

            //aesthetic line that divides tweet controls
            e.Graphics.DrawLine(m_pnBorderPen, 0, this.Height - 1, this.Width, this.Height - 1);

            base.OnPaint(e);
        }
    }
}
