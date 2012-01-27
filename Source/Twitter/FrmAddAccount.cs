using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Twitter.API;
using Twitter.API.OAuth;
using Hammock.Authentication.OAuth;

namespace Twitter
{
    public partial class FrmAddAccount : Form
    {
        private const int C_MAX_ACCOUNT_STATUS = 3;

        public delegate void OAuthDanceHandler(object sender, OAuthCredentials oaCredentials);
        public event OAuthDanceHandler OAuthDanceComplete;

        private enum AddAccountStatus
        {
            Welcome = 0,
            URL = 1,
            Pin = 2,
            Done = 3
        }

        private OAuthAPI m_oaApi;
        private OAuthCredentials m_oaCredentials;
        private AddAccountStatus m_aaStatus;
        private List<Panel> m_alStatusPanels;

        public FrmAddAccount()
        {
            InitializeComponent();

            m_oaApi = new OAuthAPI();
            m_alStatusPanels = new List<Panel>();
            m_alStatusPanels.Add(pnlWelcome);
            m_alStatusPanels.Add(pnlUrl);
            m_alStatusPanels.Add(pnlPin);
            m_alStatusPanels.Add(pnlDone);

            m_oaApi.RequestTokenReceived += new OAuthAPI.OAuthCallbackHandler(m_oaApi_RequestTokenReceived);
            m_oaApi.AccessTokenReceived += new OAuthAPI.OAuthCallbackHandler(m_oaApi_AccessTokenReceived);
        }

        private void m_oaApi_AccessTokenReceived(object sender, bool bSucceeded, string sErrorMessage, OAuthCredentials oaCredentials)
        {
            if (bSucceeded)
            {
                m_oaCredentials = oaCredentials;
                ToggleLoadingAnimation(false);
                ToggleButtons(true);
                m_aaStatus = (AddAccountStatus)((int)m_aaStatus + 1);
                UpdateFromStatus();
            }
            else
            {
                MessageBox.Show(sErrorMessage, "OAuth Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                m_aaStatus = AddAccountStatus.Pin;
                UpdateFromStatus();
                ToggleButtons(true);
                ToggleLoadingAnimation(false);
            }
        }

        private void m_oaApi_RequestTokenReceived(object sender, bool bSucceeded, string sErrorMessage, OAuthCredentials oaCredentials)
        {
            if (bSucceeded)
            {
                m_oaCredentials = oaCredentials;
                txtUrl.Text = OAuthAPI.GetRequestURL(m_oaCredentials);
                ToggleLoadingAnimation(false);
                ToggleButtons(true);
                m_aaStatus = (AddAccountStatus)((int)m_aaStatus + 1);
                UpdateFromStatus();
            }
            else
            {
                MessageBox.Show(sErrorMessage, "OAuth Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                m_aaStatus = AddAccountStatus.Pin;
                UpdateFromStatus();
                ToggleButtons(true);
                ToggleLoadingAnimation(false);
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            switch (m_aaStatus)
            {
                case AddAccountStatus.Welcome:
                    ToggleLoadingAnimation(true);
                    ToggleButtons(false);
                    Application.DoEvents();
                    m_oaApi.GetRequestToken(m_oaCredentials); 
                    break;

                case AddAccountStatus.URL:
                    m_aaStatus = (AddAccountStatus)((int)m_aaStatus + 1);
                    UpdateFromStatus();
                    break;

                case AddAccountStatus.Pin:
                    ToggleLoadingAnimation(true);
                    ToggleButtons(false);
                    Application.DoEvents();

                    try { m_oaApi.GetAccessToken(m_oaCredentials, txtPin.Text); }
                    catch (UnavailableAccessTokenException)
                    {
                        MessageBox.Show("An error occurred authenticating with the pin you supplied.  Please enter the correct pin and try again.", "OAuth Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        m_aaStatus = AddAccountStatus.Pin;
                        UpdateFromStatus();
                        ToggleButtons(true);
                        ToggleLoadingAnimation(false);
                    }

                    break;

                case AddAccountStatus.Done:
                    if (OAuthDanceComplete != null)
                        OAuthDanceComplete(this, m_oaCredentials);

                    this.Hide();
                    break;
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                this.Hide();
                e.Cancel = true;
            }
        }

        public OAuthCredentials GetCredentials()
        {
            Reset();
            base.ShowDialog();

            if (m_aaStatus == AddAccountStatus.Done)
                return m_oaCredentials;
            else
                return null;
        }

        private void Reset()
        {
            ToggleButtons(true);

            //get empty set of credentials
            m_oaCredentials = OAuthAPI.GetCredentials("", "");
            m_aaStatus = (AddAccountStatus)0;
            UpdateFromStatus();

            txtPin.Text = "";
            txtUrl.Text = "";
        }

        private void UpdateFromStatus()
        {
            for (int i = 0; i < m_alStatusPanels.Count; i++)
            {
                if (i == (int)m_aaStatus)
                    m_alStatusPanels[i].Visible = true;
                else
                    m_alStatusPanels[i].Visible = false;
            }

            if ((int)m_aaStatus == 0)
            {
                btnBack.Enabled = false;
                btnNext.Enabled = true;
                btnNext.Text = "Next ->";
            }
            else if ((int)m_aaStatus >= C_MAX_ACCOUNT_STATUS)
            {
                btnBack.Enabled = true;
                btnNext.Enabled = true;
                btnNext.Text = "Close";
            }
            else
            {
                btnBack.Enabled = true;
                btnNext.Enabled = true;
                btnNext.Text = "Next ->";
            }
        }

        private void ToggleLoadingAnimation(bool bShow)
        {
            if (bShow)
                pbLoadingAnimation.Image = ResourceManager.GetManager().GetBitmap("loading-animation.gif");
            else
                pbLoadingAnimation.Image = null;
        }

        private void ToggleButtons(bool bShow)
        {
            btnNext.Enabled = bShow;
            btnBack.Enabled = bShow;
            btnCancel.Enabled = bShow;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void btnOpenBrowser_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(txtUrl.Text);
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            if ((int)m_aaStatus > 0)
            {
                m_aaStatus = (AddAccountStatus)((int)m_aaStatus - 1);
                UpdateFromStatus();
            }
        }
    }
}
