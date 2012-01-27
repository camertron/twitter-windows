namespace Twitter
{
    partial class FrmAddAccount
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAddAccount));
            this.btnNext = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.pnlWelcome = new System.Windows.Forms.Panel();
            this.pnlUrl = new System.Windows.Forms.Panel();
            this.btnOpenBrowser = new System.Windows.Forms.Button();
            this.txtUrl = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pbLoadingAnimation = new System.Windows.Forms.PictureBox();
            this.pnlPin = new System.Windows.Forms.Panel();
            this.lblPin = new System.Windows.Forms.Label();
            this.txtPin = new System.Windows.Forms.TextBox();
            this.btnBack = new System.Windows.Forms.Button();
            this.pnlDone = new System.Windows.Forms.Panel();
            this.lblDoneMessage = new System.Windows.Forms.Label();
            this.lblWelcome = new System.Windows.Forms.Label();
            this.pnlWelcome.SuspendLayout();
            this.pnlUrl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLoadingAnimation)).BeginInit();
            this.pnlPin.SuspendLayout();
            this.pnlDone.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(333, 88);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(75, 23);
            this.btnNext.TabIndex = 2;
            this.btnNext.Text = "Next ->";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(171, 88);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // pnlWelcome
            // 
            this.pnlWelcome.Controls.Add(this.lblWelcome);
            this.pnlWelcome.Location = new System.Drawing.Point(12, 9);
            this.pnlWelcome.Name = "pnlWelcome";
            this.pnlWelcome.Size = new System.Drawing.Size(396, 73);
            this.pnlWelcome.TabIndex = 4;
            // 
            // pnlUrl
            // 
            this.pnlUrl.Controls.Add(this.btnOpenBrowser);
            this.pnlUrl.Controls.Add(this.txtUrl);
            this.pnlUrl.Controls.Add(this.label1);
            this.pnlUrl.Location = new System.Drawing.Point(12, 9);
            this.pnlUrl.Name = "pnlUrl";
            this.pnlUrl.Size = new System.Drawing.Size(396, 73);
            this.pnlUrl.TabIndex = 2;
            this.pnlUrl.Visible = false;
            // 
            // btnOpenBrowser
            // 
            this.btnOpenBrowser.Location = new System.Drawing.Point(293, 27);
            this.btnOpenBrowser.Name = "btnOpenBrowser";
            this.btnOpenBrowser.Size = new System.Drawing.Size(95, 33);
            this.btnOpenBrowser.TabIndex = 2;
            this.btnOpenBrowser.Text = "Open Browser";
            this.btnOpenBrowser.UseVisualStyleBackColor = true;
            this.btnOpenBrowser.Click += new System.EventHandler(this.btnOpenBrowser_Click);
            // 
            // txtUrl
            // 
            this.txtUrl.BackColor = System.Drawing.Color.White;
            this.txtUrl.Location = new System.Drawing.Point(8, 27);
            this.txtUrl.Multiline = true;
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.ReadOnly = true;
            this.txtUrl.Size = new System.Drawing.Size(279, 33);
            this.txtUrl.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(5, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(383, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Copy and paste this into your web browser:";
            // 
            // pbLoadingAnimation
            // 
            this.pbLoadingAnimation.Location = new System.Drawing.Point(145, 92);
            this.pbLoadingAnimation.Name = "pbLoadingAnimation";
            this.pbLoadingAnimation.Size = new System.Drawing.Size(16, 16);
            this.pbLoadingAnimation.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbLoadingAnimation.TabIndex = 5;
            this.pbLoadingAnimation.TabStop = false;
            // 
            // pnlPin
            // 
            this.pnlPin.Controls.Add(this.lblPin);
            this.pnlPin.Controls.Add(this.txtPin);
            this.pnlPin.Location = new System.Drawing.Point(12, 9);
            this.pnlPin.Name = "pnlPin";
            this.pnlPin.Size = new System.Drawing.Size(396, 73);
            this.pnlPin.TabIndex = 6;
            // 
            // lblPin
            // 
            this.lblPin.Location = new System.Drawing.Point(95, 12);
            this.lblPin.Name = "lblPin";
            this.lblPin.Size = new System.Drawing.Size(200, 16);
            this.lblPin.TabIndex = 1;
            this.lblPin.Text = "Enter the pin shown in your browser:";
            this.lblPin.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtPin
            // 
            this.txtPin.Location = new System.Drawing.Point(115, 35);
            this.txtPin.Name = "txtPin";
            this.txtPin.Size = new System.Drawing.Size(158, 20);
            this.txtPin.TabIndex = 0;
            this.txtPin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnBack
            // 
            this.btnBack.Location = new System.Drawing.Point(252, 88);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(75, 23);
            this.btnBack.TabIndex = 7;
            this.btnBack.Text = "<- Back";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // pnlDone
            // 
            this.pnlDone.Controls.Add(this.lblDoneMessage);
            this.pnlDone.Location = new System.Drawing.Point(12, 9);
            this.pnlDone.Name = "pnlDone";
            this.pnlDone.Size = new System.Drawing.Size(396, 73);
            this.pnlDone.TabIndex = 8;
            // 
            // lblDoneMessage
            // 
            this.lblDoneMessage.Location = new System.Drawing.Point(21, 14);
            this.lblDoneMessage.Name = "lblDoneMessage";
            this.lblDoneMessage.Size = new System.Drawing.Size(356, 41);
            this.lblDoneMessage.TabIndex = 0;
            this.lblDoneMessage.Text = "You\'re all set!  You can now see your timeline, send tweets, and much more from T" +
                "witter for Windows.";
            this.lblDoneMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblWelcome
            // 
            this.lblWelcome.Location = new System.Drawing.Point(39, 20);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(317, 33);
            this.lblWelcome.TabIndex = 0;
            this.lblWelcome.Text = "Let\'s add an accunt!  You\'ll need to have access to the Internet.  Click \"Next\" t" +
                "o continue.";
            this.lblWelcome.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FrmAddAccount
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(420, 122);
            this.Controls.Add(this.pnlWelcome);
            this.Controls.Add(this.pnlDone);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.pnlUrl);
            this.Controls.Add(this.pnlPin);
            this.Controls.Add(this.pbLoadingAnimation);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnNext);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmAddAccount";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Add Account";
            this.pnlWelcome.ResumeLayout(false);
            this.pnlUrl.ResumeLayout(false);
            this.pnlUrl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLoadingAnimation)).EndInit();
            this.pnlPin.ResumeLayout(false);
            this.pnlPin.PerformLayout();
            this.pnlDone.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel pnlWelcome;
        private System.Windows.Forms.Panel pnlUrl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtUrl;
        private System.Windows.Forms.PictureBox pbLoadingAnimation;
        private System.Windows.Forms.Panel pnlPin;
        private System.Windows.Forms.TextBox txtPin;
        private System.Windows.Forms.Label lblPin;
        private System.Windows.Forms.Button btnOpenBrowser;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Panel pnlDone;
        private System.Windows.Forms.Label lblDoneMessage;
        private System.Windows.Forms.Label lblWelcome;
    }
}