using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Hammock.Authentication.OAuth;

namespace Twitter
{
    public partial class FrmPreferences : Form
    {
        public delegate void CredentialsRemovedHandler(object sender, string sScreenName);

        public event FrmAddAccount.OAuthDanceHandler CredentialsAdded;
        public event CredentialsRemovedHandler CredentialsRemoved;

        private FrmAddAccount m_faAddForm;
        private AccountList m_alAccountList;
        public event EventHandler CancelClicked;
        public event EventHandler OkClicked;

        public FrmPreferences()
        {
            InitializeComponent();
            m_faAddForm = new FrmAddAccount();

            lbAccounts.SelectedValueChanged += new EventHandler(lbAccounts_SelectedValueChanged);
        }

        private void lbAccounts_SelectedValueChanged(object sender, EventArgs e)
        {
            btnRemove.Enabled = true;
        }

        public AccountList AccountList
        {
            get { return m_alAccountList; }
            set { m_alAccountList = value; }
        }

        public new DialogResult ShowDialog()
        {
            lbAccounts.Items.Clear();

            for (int i = 0; i < m_alAccountList.Count; i++)
                lbAccounts.Items.Add(m_alAccountList[i].Credentials.ClientUsername);

            return base.ShowDialog();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                this.Hide();
                e.Cancel = true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Hide();

            if (CancelClicked != null)
                CancelClicked(this, e);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Hide();

            if (OkClicked != null)
                OkClicked(this, e);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            OAuthCredentials oaCredentials = m_faAddForm.GetCredentials();

            //GetCredentials() can return null if the user didn't complete the OAuth flow successfully
            if (oaCredentials != null)
            {
                lbAccounts.Items.Add(oaCredentials.ClientUsername);

                if (CredentialsAdded != null)
                    CredentialsAdded(this, oaCredentials);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (lbAccounts.SelectedIndex > -1)
            {
                if (CredentialsRemoved != null)
                    CredentialsRemoved(this, lbAccounts.SelectedItem.ToString());

                lbAccounts.Items.RemoveAt(lbAccounts.SelectedIndex);
            }
        }
    }
}
