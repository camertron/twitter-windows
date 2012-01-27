using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using Twitter.API;
using Twitter.API.Basic;

namespace Twitter.Controls
{
    public partial class TimelineStatus : UserControl
    {
        private const int C_CONTROL_MARGIN = 10;
        private const int C_AVATAR_TEXT_MARGIN = 10;
        private const int C_AVATAR_HEIGHT = 48;
        private const int C_ACTION_BUTTON_SHADE = 50;
        private const int C_FROM_USER_HEIGHT = 15;
        private const int C_RETWEET_HEIGHT = 10;
        private const int C_RETWEET_VERT_SPACING = 7;
        private const int C_RETWEET_TEXT_VERT_SPACING = 5;
        private const int C_RETWEEET_TEXT_HORIZ_SPACING = 17;

        private string m_sFromUser;
        private Status m_stStatusObj;
        private bool m_bDisplayConversationButton;
        private bool m_bFavorite;
        private bool m_bRetweet;

        private Bitmap m_bmpAvatar = null;
        private Font m_fntScreenNameFont;
        private Font m_fntRetweetedFont;
        private Font m_fntFromUser;
        private Pen m_pnBorderPen;
        private SolidBrush m_sbFromUser;
        private SolidBrush m_sbRetweet;
        private BasicAPI m_bAPI;

        public event TweetTextField.TextElementClickHandler TextElementClicked;
        public event EventHandler RetweetClicked;
        public event EventHandler QuoteTweetClicked;
        public event EventHandler FavoriteClicked;
        public event EventHandler ReplyClicked;
        public event EventHandler ConversationClicked;

        public TimelineStatus(Status stFrom, BasicAPI bAPI)
        {
            InitializeComponent();

            m_bAPI = bAPI;
            m_bDisplayConversationButton = false;
            m_fntScreenNameFont = new Font("Arial", 10);
            m_fntRetweetedFont = new Font("Arial", 8);
            m_fntFromUser = new Font("Arial", 10, FontStyle.Bold);
            m_sbFromUser = new SolidBrush(Color.Black);
            m_sbRetweet = new SolidBrush(Color.FromArgb(70, 70, 70));
            m_pnBorderPen = new Pen(Color.FromArgb(220, 220, 220));

            m_stStatusObj = stFrom;
            ttfTextField.UpdateFromStatus(m_stStatusObj);
            Favorite = Boolean.Parse(m_stStatusObj["favorited"].ToString());

            if (m_stStatusObj.IsRetweet)
            {
                if (m_stStatusObj.RetweetedStatus == null)
                {
                    if ((m_stStatusObj.StatusText.Words.Count > 1) && (m_stStatusObj.StatusText.Words[1].Type == StatusTextElement.StatusTextElementType.ScreenName))
                    {
                        m_sFromUser = m_stStatusObj.StatusText.Words[1].Text.Substring(1);
                        new Thread(new ThreadStart(UserLookup)).Start();
                    }
                }
                else
                {
                    m_sFromUser = m_stStatusObj.RetweetedStatus.User["screen_name"].ToString();
                    AsyncContentManager.GetManager().RequestImage(m_stStatusObj.RetweetedStatus.User["profile_image_url"].ToString(), AvatarCallback);
                }
            }
            else
            {
                m_sFromUser = m_stStatusObj.User["screen_name"].ToString();
                AsyncContentManager.GetManager().RequestImage(m_stStatusObj.User["profile_image_url"].ToString(), AvatarCallback);
            }

            User uActiveUser = TwitterController.GetController().ActiveAccount.UserObject;

            if (uActiveUser != null)
                Retweet = m_stStatusObj.IsRetweet && (m_sFromUser == TwitterController.GetController().ActiveAccount.UserObject["screen_name"].ToString());

            ttfTextField.TextElementClicked += new TweetTextField.TextElementClickHandler(ttfTextField_TextElementClicked);

            abRetweet.Click += new EventHandler(abRetweet_Click);
            abFavorite.Click += new EventHandler(abFavorite_Click);
            abReply.Click += new EventHandler(abReply_Click);
            abConversation.Click += new EventHandler(abConversation_Click);
        }

        private void UserLookup()
        {
            m_bAPI.LookupUser(UserLookupCallback, null, new List<string>(new string[] { m_sFromUser }));  //remove the @ from the beginning of the screen name
        }

        private void UserLookupCallback(APICallbackArgs acArgs)
        {
            //make a dummy status, give it the user object we have
            m_stStatusObj.RetweetedStatus = new Status(new Json.JsonNode());
            m_stStatusObj.RetweetedStatus.User = ((List<User>)acArgs.ResponseObject)[0];
            AsyncContentManager.GetManager().RequestImage(m_stStatusObj.RetweetedStatus.User["profile_image_url"].ToString(), AvatarCallback);
        }

        private void AvatarCallback(object sender, Bitmap bmpAvatar, object objContext)
        {
            //for some reason, a few of the avatars are weird sizes (I'm looking at you, TechCrunch)
            //this code resizes the avatar before its displayed so nothing overlaps and looks weird
            Bitmap bmpResized = new Bitmap(Literals.C_AVATAR_DIMENSIONS, Literals.C_AVATAR_DIMENSIONS);

            using (Graphics gCanvas = Graphics.FromImage(bmpResized))
                gCanvas.DrawImage(bmpAvatar, 0, 0, Literals.C_AVATAR_DIMENSIONS, Literals.C_AVATAR_DIMENSIONS);

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

            Favorite = !Favorite;
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

            Retweet = true;
            UpdateStatusIcons();
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
            get { return m_bFavorite; }
            set
            {
                m_bFavorite = value;
                UpdateStatusIcons();
                UpdateStatusButtons();
            }
        }

        public bool Retweet
        {
            get { return m_bRetweet; }
            set
            {
                m_bRetweet = value;
                UpdateStatusIcons();
                UpdateStatusButtons();
            }
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

            //disable action buttons for other timeline statuses in the parent control
            //this is definitely a little brute-force, but I can't think of any other way to do it :(
            for (int i = 0; i < this.Parent.Controls.Count; i ++)
            {
                if ((this.Parent.Controls[i].GetType() == typeof(TimelineStatus)) && (this.Parent.Controls[i] != this))
                    ((TimelineStatus)this.Parent.Controls[i]).HideActionButtons();
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            Point ptMouse = this.PointToClient(Cursor.Position);

            if (!(((ptMouse.X > 0) && (ptMouse.X < this.Width)) && ((ptMouse.Y > 0) && (ptMouse.Y < this.Height))))
                HideActionButtons();
        }

        public void HideActionButtons()
        {
            abConversation.Visible = m_bDisplayConversationButton;
            abFavorite.Visible = false;
            abReply.Visible = false;
            abRetweet.Visible = false;
        }

        protected override void OnResize(EventArgs e)
        {
            UpdateStatusButtons();

            ttfTextField.Left = Literals.C_AVATAR_DIMENSIONS + (C_CONTROL_MARGIN * 2);
            ttfTextField.Top = C_CONTROL_MARGIN + C_FROM_USER_HEIGHT;
            ttfTextField.Width = this.Width - (ttfTextField.Left + C_CONTROL_MARGIN);
            //ttfTextField.Height set automatically

            int iExtraRetweetHeight = 0;

            if ((m_stStatusObj != null) && m_stStatusObj.IsRetweet)
                iExtraRetweetHeight = C_RETWEET_VERT_SPACING + C_RETWEET_HEIGHT;

            //set height based on height of TweetTextField
            if ((ttfTextField.Bottom + iExtraRetweetHeight) > (C_AVATAR_HEIGHT + (C_CONTROL_MARGIN * 2)))
                this.Height = ttfTextField.Bottom + C_CONTROL_MARGIN + iExtraRetweetHeight;
            else
                this.Height = C_AVATAR_HEIGHT + (C_CONTROL_MARGIN * 2) + iExtraRetweetHeight;

            pbDogear.Left = this.Width - pbDogear.Width;
            pbDogear.Top = 0;

            base.OnResize(e);
        }

        private void UpdateStatusButtons()
        {
            int iRtOffset = 10;

            if (pbDogear.Visible)
                iRtOffset += 10;

            abRetweet.Left = this.Width - (abRetweet.Width + iRtOffset);
            abFavorite.Left = abRetweet.Left - (abFavorite.Width + 5);
            abReply.Left = abFavorite.Left - (abReply.Width + 5);
            abConversation.Left = abReply.Left - (abConversation.Width + 5);
        }

        private void UpdateStatusIcons()
        {
            string sImage = null;

            if (m_bFavorite)
            {
                if (m_bRetweet)
                    sImage = "tweet-dogear-favorite-retweet.png";
                else
                    sImage = "tweet-dogear-favorite.png";
            }
            else
            {
                if (m_bRetweet)
                    sImage = "tweet-dogear-retweet.png";
            }

            if (sImage != null)
            {
                pbDogear.Image = ResourceManager.GetManager().GetBitmap(sImage);
                pbDogear.Visible = true;
            }
            else
                pbDogear.Visible = false;
        }

        public void UpdateLayout()
        {
            this.OnPaint(new PaintEventArgs(this.CreateGraphics(), new Rectangle(0, 0, this.Width, this.Height)));
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            int iX = C_CONTROL_MARGIN + Literals.C_AVATAR_DIMENSIONS + C_AVATAR_TEXT_MARGIN;
            int iY = C_CONTROL_MARGIN;

            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            if (m_bmpAvatar != null)
                e.Graphics.DrawImage(m_bmpAvatar, C_CONTROL_MARGIN, C_CONTROL_MARGIN);

            //tweet came from this user
            e.Graphics.DrawString(m_sFromUser, m_fntFromUser, m_sbFromUser, iX, iY - 2);

            //draw retweet text if necessary
            if (m_stStatusObj.IsRetweet)
            {
                e.Graphics.DrawImage((Image)ResourceManager.GetManager().GetBitmap("retweet-indicator-small.png"), ttfTextField.Left, ttfTextField.Bottom + C_RETWEET_VERT_SPACING);
                e.Graphics.DrawString("by " + m_stStatusObj.User["screen_name"], m_fntRetweetedFont, m_sbRetweet, ttfTextField.Left + C_RETWEEET_TEXT_HORIZ_SPACING, ttfTextField.Bottom + C_RETWEET_TEXT_VERT_SPACING);
            }

            //aesthetic line that divides tweet controls
            e.Graphics.DrawLine(m_pnBorderPen, 0, this.Height - 1, this.Width, this.Height - 1);

            base.OnPaint(e);
        }
    }
}
