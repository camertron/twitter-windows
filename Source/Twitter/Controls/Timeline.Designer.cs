namespace Twitter.Controls
{
    partial class Timeline
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Timeline));
            this.tmrTweetAnimate = new System.Windows.Forms.Timer(this.components);
            this.pbLoadingAnimation = new System.Windows.Forms.PictureBox();
            this.lblLoadingTweets = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbLoadingAnimation)).BeginInit();
            this.SuspendLayout();
            // 
            // tmrTweetAnimate
            // 
            this.tmrTweetAnimate.Interval = 1;
            this.tmrTweetAnimate.Tick += new System.EventHandler(this.tmrTweetAnimate_Tick);
            // 
            // pbLoadingAnimation
            // 
            this.pbLoadingAnimation.Image = ((System.Drawing.Image)(resources.GetObject("pbLoadingAnimation.Image")));
            this.pbLoadingAnimation.Location = new System.Drawing.Point(59, 44);
            this.pbLoadingAnimation.Name = "pbLoadingAnimation";
            this.pbLoadingAnimation.Size = new System.Drawing.Size(16, 16);
            this.pbLoadingAnimation.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbLoadingAnimation.TabIndex = 0;
            this.pbLoadingAnimation.TabStop = false;
            // 
            // lblLoadingTweets
            // 
            this.lblLoadingTweets.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblLoadingTweets.Location = new System.Drawing.Point(81, 45);
            this.lblLoadingTweets.Name = "lblLoadingTweets";
            this.lblLoadingTweets.Size = new System.Drawing.Size(89, 14);
            this.lblLoadingTweets.TabIndex = 1;
            this.lblLoadingTweets.Text = "Loading tweets...";
            // 
            // Timeline
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblLoadingTweets);
            this.Controls.Add(this.pbLoadingAnimation);
            this.Name = "Timeline";
            this.Size = new System.Drawing.Size(209, 225);
            ((System.ComponentModel.ISupportInitialize)(this.pbLoadingAnimation)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer tmrTweetAnimate;
        private System.Windows.Forms.PictureBox pbLoadingAnimation;
        private System.Windows.Forms.Label lblLoadingTweets;
    }
}
