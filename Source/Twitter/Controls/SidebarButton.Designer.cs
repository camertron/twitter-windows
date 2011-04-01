namespace Twitter.Controls
{
    partial class SidebarButton
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SidebarButton));
            this.pbNewActivity = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbNewActivity)).BeginInit();
            this.SuspendLayout();
            // 
            // pbNewActivity
            // 
            this.pbNewActivity.BackColor = System.Drawing.Color.Transparent;
            this.pbNewActivity.Image = ((System.Drawing.Image)(resources.GetObject("pbNewActivity.Image")));
            this.pbNewActivity.Location = new System.Drawing.Point(27, 3);
            this.pbNewActivity.Name = "pbNewActivity";
            this.pbNewActivity.Size = new System.Drawing.Size(15, 19);
            this.pbNewActivity.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbNewActivity.TabIndex = 0;
            this.pbNewActivity.TabStop = false;
            this.pbNewActivity.Visible = false;
            // 
            // SidebarButton
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pbNewActivity);
            this.Name = "SidebarButton";
            this.Size = new System.Drawing.Size(44, 28);
            ((System.ComponentModel.ISupportInitialize)(this.pbNewActivity)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbNewActivity;
    }
}
