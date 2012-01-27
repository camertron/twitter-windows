using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Twitter
{
    public partial class FrmTweet : Form
    {
        private Size szDefaultSize = new Size(400, 180);
        public delegate void TweetClickedEventHandler(object sender, string sTweetText);
        public event TweetClickedEventHandler TweetClicked;
        public event EventHandler CancelClicked;

        private Color m_clrWithinLimit = Color.FromArgb(224, 224, 224);
        private Color m_clrExceededLimit = Color.Red;
        private Bitmap m_bmpAvatar;
        private Form m_frmParent;

        public FrmTweet(Form frmParent)
        {
            InitializeComponent();
            m_frmParent = frmParent;
            ttfTextField.TextChanged += new EventHandler(ttfTextField_TextChanged);
        }

        public Bitmap Avatar
        {
            get { return (Bitmap)m_bmpAvatar; }
            set
            {
                m_bmpAvatar = value;

                if (m_bmpAvatar != null)
                    pbAvatar.Image = (Image)Imaging.RoundAvatarCorners(value);
            }
        }

        protected override void OnActivated(EventArgs e)
        {
            ttfTextField.Focus();
        }

        private void ttfTextField_TextChanged(object sender, EventArgs e)
        {
            UpdateLetterCount();
        }

        private void UpdateLetterCount()
        {
            int iCharsRemaining = Literals.C_TWEET_MAX_CHARS - ttfTextField.TextLength;

            lblCharsLeft.Text = iCharsRemaining.ToString();

            if (iCharsRemaining >= 0)
                lblCharsLeft.ForeColor = m_clrWithinLimit;
            else
                lblCharsLeft.ForeColor = m_clrExceededLimit;

            btnTweet.Enabled = (ttfTextField.Text.Length > 0);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                //prevent the form from actually closing - hide it instead
                //this helps save a bit of memory so the parent doesn't have to 
                //create a new form every time the user want's to tweet\
                this.Hide();
                e.Cancel = true;
            }
        }

        public DialogResult ShowDialog(string sInitText)
        {
            this.Size = szDefaultSize;
            Rectangle rectScreenArea = Screen.FromControl(this).WorkingArea;
            Point ptDisplayCoords = new Point();

            if (rectScreenArea.Width < (m_frmParent.Width + this.Width))
            {
                //show tweet box in the center of the parent timeline form
                ptDisplayCoords.X = (m_frmParent.Width / 2) - (this.Width / 2);
                ptDisplayCoords.Y = (rectScreenArea.Height / 2) - (this.Height / 2);
            }
            else if (m_frmParent.Left < this.Width)
            {
                //show tweet box at the upper right
                ptDisplayCoords.X = m_frmParent.Right + 10;
                ptDisplayCoords.Y = m_frmParent.Top + 10;
            }
            else
            {
                //show tweet box at the upper left
                ptDisplayCoords.X = m_frmParent.Left - (this.Width + 10);
                ptDisplayCoords.Y = m_frmParent.Top + 10;
            }

            this.Location = ptDisplayCoords;
            ttfTextField.Text = sInitText;
            ttfTextField.SelectionStart = sInitText.Length;
            ttfTextField.UpdateText();
            UpdateLetterCount();

            return base.ShowDialog();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Hide();

            if (CancelClicked != null)
                CancelClicked(this, e);

            ttfTextField.Text = "";
        }

        private void btnTweet_Click(object sender, EventArgs e)
        {
            Tweet();
        }

        protected void Tweet()
        {
            //do some validation
            if (ttfTextField.TextLength > Literals.C_TWEET_MAX_CHARS)
                MessageBox.Show("Message must be fewer than " + Literals.C_TWEET_MAX_CHARS.ToString() + " characters.", "Tweet too long", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
            {
                this.Hide();

                if (TweetClicked != null)
                    TweetClicked(this, ttfTextField.Text);

                ttfTextField.Text = "";
            }
        }

        protected override void OnResize(EventArgs e)
        {
            pnlTweet.Width = this.ClientRectangle.Width;
            pnlTweet.Height = this.ClientRectangle.Height - pbButtonBar.Height;
            pbButtonBar.Width = this.Width;
            pbButtonBar.Top = this.ClientRectangle.Height - pbButtonBar.Height;
            ttfTextField.Width = this.ClientRectangle.Width - (ttfTextField.Left + 5);
            ttfTextField.Height = pnlTweet.Height - (ttfTextField.Top * 2);
            btnCancel.Top = pbButtonBar.Top + 8;
            btnTweet.Top = btnCancel.Top;
            btnTweet.Left = this.ClientRectangle.Width - (btnTweet.Width + btnCancel.Left);
            lblCharsLeft.Top = pbButtonBar.Top;
            lblCharsLeft.Left = btnTweet.Left - (lblCharsLeft.Width + 6);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Escape:
                    this.Hide(); break;
            }

            if (e.KeyValue == 13 && e.Control)
                Tweet();

            base.OnKeyDown(e);
        }
    }
}
