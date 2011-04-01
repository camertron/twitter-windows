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
        public delegate void TweetClickedEventHandler(object sender, string sTweetText);
        public event TweetClickedEventHandler TweetClicked;
        public event EventHandler CancelClicked;

        private Color m_clrWithinLimit = Color.Black;
        private Color m_clrExceededLimit = Color.Red;

        public FrmTweet()
        {
            InitializeComponent();

            this.FormClosing += new FormClosingEventHandler(FrmTweet_FormClosing);
            ttfTextField.TextChanged += new EventHandler(ttfTextField_TextChanged);
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
            int iCharsRemaining = Literals.C_TWEET_MAX_CHARS - ttfTextField.Text.Length;

            lblCharsLeft.Text = iCharsRemaining.ToString();

            if (iCharsRemaining >= 0)
                lblCharsLeft.ForeColor = m_clrWithinLimit;
            else
                lblCharsLeft.ForeColor = m_clrExceededLimit;

            btnTweet.Enabled = (ttfTextField.Text.Length > 0);
        }

        private void FrmTweet_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                //prevent the form from actually closing
                this.Hide();
                e.Cancel = true;
            }
        }

        public DialogResult ShowDialog(string sInitText)
        {
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
            //do some validation
            if (ttfTextField.Text.Length > Literals.C_TWEET_MAX_CHARS)
                MessageBox.Show("Message must be fewer than " + Literals.C_TWEET_MAX_CHARS.ToString() + " characters.", "Tweet too long", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
            {
                this.Hide();

                if (TweetClicked != null)
                    TweetClicked(this, ttfTextField.Text);

                ttfTextField.Text = "";
            }
        }
    }
}
