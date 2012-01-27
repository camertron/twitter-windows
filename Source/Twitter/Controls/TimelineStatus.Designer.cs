namespace Twitter.Controls
{
    partial class TimelineStatus
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TimelineStatus));
            this.ttfTextField = new Twitter.Controls.TweetTextField();
            this.cmsRetweet = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiRetweet = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiQuoteTweet = new System.Windows.Forms.ToolStripMenuItem();
            this.abConversation = new Twitter.Controls.ActionButton();
            this.abReply = new Twitter.Controls.ActionButton();
            this.abFavorite = new Twitter.Controls.ActionButton();
            this.abRetweet = new Twitter.Controls.ActionButton();
            this.pbDogear = new System.Windows.Forms.PictureBox();
            this.cmsRetweet.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbDogear)).BeginInit();
            this.SuspendLayout();
            // 
            // ttfTextField
            // 
            this.ttfTextField.BackColor = System.Drawing.SystemColors.Control;
            this.ttfTextField.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.ttfTextField.ConstrictHeight = true;
            this.ttfTextField.ControlBorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ttfTextField.ControlMargin = 1;
            this.ttfTextField.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.ttfTextField.Location = new System.Drawing.Point(63, 27);
            this.ttfTextField.Name = "ttfTextField";
            this.ttfTextField.ReadOnly = true;
            this.ttfTextField.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.ttfTextField.SelectionLength = 0;
            this.ttfTextField.SelectionStart = 0;
            this.ttfTextField.Size = new System.Drawing.Size(300, 16);
            this.ttfTextField.TabIndex = 5;
            // 
            // cmsRetweet
            // 
            this.cmsRetweet.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiRetweet,
            this.tsmiQuoteTweet});
            this.cmsRetweet.Name = "cmsRetweet";
            this.cmsRetweet.Size = new System.Drawing.Size(152, 48);
            // 
            // tsmiRetweet
            // 
            this.tsmiRetweet.Name = "tsmiRetweet";
            this.tsmiRetweet.Size = new System.Drawing.Size(151, 22);
            this.tsmiRetweet.Text = "Retweet...";
            this.tsmiRetweet.Click += new System.EventHandler(this.tsmiRetweet_Click);
            // 
            // tsmiQuoteTweet
            // 
            this.tsmiQuoteTweet.Name = "tsmiQuoteTweet";
            this.tsmiQuoteTweet.Size = new System.Drawing.Size(151, 22);
            this.tsmiQuoteTweet.Text = "Quote Tweet...";
            this.tsmiQuoteTweet.Click += new System.EventHandler(this.tsmiQuoteTweet_Click);
            // 
            // abConversation
            // 
            this.abConversation.BackColor = System.Drawing.Color.Transparent;
            this.abConversation.Image = ((System.Drawing.Bitmap)(resources.GetObject("abConversation.Image")));
            this.abConversation.Location = new System.Drawing.Point(247, 6);
            this.abConversation.Name = "abConversation";
            this.abConversation.Size = new System.Drawing.Size(17, 15);
            this.abConversation.TabIndex = 4;
            this.abConversation.ToolTipText = "View Conversation";
            this.abConversation.Visible = false;
            // 
            // abReply
            // 
            this.abReply.BackColor = System.Drawing.Color.Transparent;
            this.abReply.Image = ((System.Drawing.Bitmap)(resources.GetObject("abReply.Image")));
            this.abReply.Location = new System.Drawing.Point(269, 5);
            this.abReply.Name = "abReply";
            this.abReply.Size = new System.Drawing.Size(22, 14);
            this.abReply.TabIndex = 3;
            this.abReply.ToolTipText = "Reply";
            this.abReply.Visible = false;
            // 
            // abFavorite
            // 
            this.abFavorite.BackColor = System.Drawing.Color.Transparent;
            this.abFavorite.Image = ((System.Drawing.Bitmap)(resources.GetObject("abFavorite.Image")));
            this.abFavorite.Location = new System.Drawing.Point(294, 3);
            this.abFavorite.Name = "abFavorite";
            this.abFavorite.Size = new System.Drawing.Size(17, 17);
            this.abFavorite.TabIndex = 2;
            this.abFavorite.ToolTipText = "Favorite";
            this.abFavorite.Visible = false;
            // 
            // abRetweet
            // 
            this.abRetweet.BackColor = System.Drawing.Color.Transparent;
            this.abRetweet.Image = ((System.Drawing.Bitmap)(resources.GetObject("abRetweet.Image")));
            this.abRetweet.Location = new System.Drawing.Point(315, 7);
            this.abRetweet.Name = "abRetweet";
            this.abRetweet.Size = new System.Drawing.Size(20, 11);
            this.abRetweet.TabIndex = 1;
            this.abRetweet.ToolTipText = "Retweet";
            this.abRetweet.Visible = false;
            // 
            // pbDogear
            // 
            this.pbDogear.Location = new System.Drawing.Point(357, 0);
            this.pbDogear.Name = "pbDogear";
            this.pbDogear.Size = new System.Drawing.Size(25, 25);
            this.pbDogear.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbDogear.TabIndex = 6;
            this.pbDogear.TabStop = false;
            this.pbDogear.Visible = false;
            // 
            // TimelineStatus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ttfTextField);
            this.Controls.Add(this.abConversation);
            this.Controls.Add(this.abReply);
            this.Controls.Add(this.abFavorite);
            this.Controls.Add(this.abRetweet);
            this.Controls.Add(this.pbDogear);
            this.DoubleBuffered = true;
            this.Name = "TimelineStatus";
            this.Size = new System.Drawing.Size(382, 114);
            this.cmsRetweet.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbDogear)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ActionButton abRetweet;
        private ActionButton abFavorite;
        private ActionButton abReply;
        private ActionButton abConversation;
        private TweetTextField ttfTextField;
        private System.Windows.Forms.ContextMenuStrip cmsRetweet;
        private System.Windows.Forms.ToolStripMenuItem tsmiRetweet;
        private System.Windows.Forms.ToolStripMenuItem tsmiQuoteTweet;
        private System.Windows.Forms.PictureBox pbDogear;

    }
}
