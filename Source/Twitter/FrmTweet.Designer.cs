namespace Twitter
{
    partial class FrmTweet
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmTweet));
            this.btnTweet = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblCharsLeft = new System.Windows.Forms.Label();
            this.ttfTextField = new Twitter.Controls.TweetTextField();
            this.SuspendLayout();
            // 
            // btnTweet
            // 
            this.btnTweet.Enabled = false;
            this.btnTweet.Location = new System.Drawing.Point(351, 128);
            this.btnTweet.Name = "btnTweet";
            this.btnTweet.Size = new System.Drawing.Size(75, 23);
            this.btnTweet.TabIndex = 1;
            this.btnTweet.Text = "Tweet";
            this.btnTweet.UseVisualStyleBackColor = true;
            this.btnTweet.Click += new System.EventHandler(this.btnTweet_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(12, 128);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblCharsLeft
            // 
            this.lblCharsLeft.Location = new System.Drawing.Point(291, 132);
            this.lblCharsLeft.Name = "lblCharsLeft";
            this.lblCharsLeft.Size = new System.Drawing.Size(57, 15);
            this.lblCharsLeft.TabIndex = 3;
            this.lblCharsLeft.Text = "140";
            this.lblCharsLeft.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ttfTextField
            // 
            this.ttfTextField.BackColor = System.Drawing.Color.White;
            this.ttfTextField.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.ttfTextField.ConstrictHeight = false;
            this.ttfTextField.ControlBorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ttfTextField.ControlMargin = 5;
            this.ttfTextField.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.ttfTextField.Location = new System.Drawing.Point(12, 12);
            this.ttfTextField.Name = "ttfTextField";
            this.ttfTextField.ReadOnly = false;
            this.ttfTextField.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.ttfTextField.Size = new System.Drawing.Size(414, 110);
            this.ttfTextField.TabIndex = 0;
            // 
            // FrmTweet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(438, 163);
            this.Controls.Add(this.lblCharsLeft);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnTweet);
            this.Controls.Add(this.ttfTextField);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmTweet";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "New Tweet";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnTweet;
        private System.Windows.Forms.Button btnCancel;
        private Controls.TweetTextField ttfTextField;
        private System.Windows.Forms.Label lblCharsLeft;

    }
}