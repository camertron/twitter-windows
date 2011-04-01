namespace Twitter.Controls
{
    partial class TweetTextField
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
            this.rtbTextBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // rtbTextBox
            // 
            this.rtbTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbTextBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.rtbTextBox.DetectUrls = false;
            this.rtbTextBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbTextBox.Location = new System.Drawing.Point(64, 28);
            this.rtbTextBox.Name = "rtbTextBox";
            this.rtbTextBox.Size = new System.Drawing.Size(344, 81);
            this.rtbTextBox.TabIndex = 0;
            this.rtbTextBox.TabStop = false;
            this.rtbTextBox.Text = "";
            // 
            // TweetTextField
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.rtbTextBox);
            this.Name = "TweetTextField";
            this.Size = new System.Drawing.Size(461, 147);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtbTextBox;
    }
}
