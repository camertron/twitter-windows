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
            this.pnlTweet = new System.Windows.Forms.Panel();
            this.pbAvatar = new System.Windows.Forms.PictureBox();
            this.pbButtonBar = new System.Windows.Forms.PictureBox();
            this.ttfTextField = new Twitter.Controls.TweetTextField();
            this.pnlTweet.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbAvatar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbButtonBar)).BeginInit();
            this.SuspendLayout();
            // 
            // btnTweet
            // 
            this.btnTweet.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnTweet.BackgroundImage")));
            this.btnTweet.Enabled = false;
            this.btnTweet.FlatAppearance.BorderSize = 0;
            this.btnTweet.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTweet.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnTweet.Location = new System.Drawing.Point(361, 118);
            this.btnTweet.Name = "btnTweet";
            this.btnTweet.Size = new System.Drawing.Size(68, 28);
            this.btnTweet.TabIndex = 1;
            this.btnTweet.Text = "Tweet";
            this.btnTweet.UseVisualStyleBackColor = true;
            this.btnTweet.Click += new System.EventHandler(this.btnTweet_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCancel.BackgroundImage")));
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnCancel.Location = new System.Drawing.Point(9, 118);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(68, 28);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblCharsLeft
            // 
            this.lblCharsLeft.BackColor = System.Drawing.Color.Transparent;
            this.lblCharsLeft.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCharsLeft.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblCharsLeft.Image = ((System.Drawing.Image)(resources.GetObject("lblCharsLeft.Image")));
            this.lblCharsLeft.Location = new System.Drawing.Point(298, 110);
            this.lblCharsLeft.Name = "lblCharsLeft";
            this.lblCharsLeft.Size = new System.Drawing.Size(48, 44);
            this.lblCharsLeft.TabIndex = 3;
            this.lblCharsLeft.Text = "140";
            this.lblCharsLeft.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pnlTweet
            // 
            this.pnlTweet.BackColor = System.Drawing.Color.White;
            this.pnlTweet.Controls.Add(this.pbAvatar);
            this.pnlTweet.Controls.Add(this.ttfTextField);
            this.pnlTweet.Location = new System.Drawing.Point(0, 0);
            this.pnlTweet.Name = "pnlTweet";
            this.pnlTweet.Size = new System.Drawing.Size(438, 110);
            this.pnlTweet.TabIndex = 4;
            // 
            // pbAvatar
            // 
            this.pbAvatar.Location = new System.Drawing.Point(12, 12);
            this.pbAvatar.Name = "pbAvatar";
            this.pbAvatar.Size = new System.Drawing.Size(48, 48);
            this.pbAvatar.TabIndex = 1;
            this.pbAvatar.TabStop = false;
            // 
            // pbButtonBar
            // 
            this.pbButtonBar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pbButtonBar.BackgroundImage")));
            this.pbButtonBar.Location = new System.Drawing.Point(-1, 110);
            this.pbButtonBar.Name = "pbButtonBar";
            this.pbButtonBar.Size = new System.Drawing.Size(439, 44);
            this.pbButtonBar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbButtonBar.TabIndex = 5;
            this.pbButtonBar.TabStop = false;
            // 
            // ttfTextField
            // 
            this.ttfTextField.BackColor = System.Drawing.Color.White;
            this.ttfTextField.BorderColor = System.Drawing.Color.White;
            this.ttfTextField.ConstrictHeight = false;
            this.ttfTextField.ControlBorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ttfTextField.ControlMargin = 5;
            this.ttfTextField.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.ttfTextField.Location = new System.Drawing.Point(70, 5);
            this.ttfTextField.Margin = new System.Windows.Forms.Padding(0);
            this.ttfTextField.Name = "ttfTextField";
            this.ttfTextField.ReadOnly = false;
            this.ttfTextField.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.ttfTextField.SelectionLength = 0;
            this.ttfTextField.SelectionStart = 0;
            this.ttfTextField.Size = new System.Drawing.Size(365, 101);
            this.ttfTextField.TabIndex = 0;
            // 
            // FrmTweet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(438, 153);
            this.Controls.Add(this.lblCharsLeft);
            this.Controls.Add(this.btnTweet);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.pbButtonBar);
            this.Controls.Add(this.pnlTweet);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmTweet";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "New Tweet";
            this.pnlTweet.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbAvatar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbButtonBar)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnTweet;
        private System.Windows.Forms.Button btnCancel;
        private Controls.TweetTextField ttfTextField;
        private System.Windows.Forms.Label lblCharsLeft;
        private System.Windows.Forms.Panel pnlTweet;
        private System.Windows.Forms.PictureBox pbAvatar;
        private System.Windows.Forms.PictureBox pbButtonBar;

    }
}