namespace Twitter
{
    partial class FrmMain
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.pnlSidebar = new System.Windows.Forms.Panel();
            this.pbLarry = new System.Windows.Forms.PictureBox();
            this.pbAvatar = new System.Windows.Forms.PictureBox();
            this.sbbSearch = new Twitter.Controls.SidebarButton();
            this.sbbPeople = new Twitter.Controls.SidebarButton();
            this.sbbLists = new Twitter.Controls.SidebarButton();
            this.sbbMessages = new Twitter.Controls.SidebarButton();
            this.sbbReplies = new Twitter.Controls.SidebarButton();
            this.sbbTimeline = new Twitter.Controls.SidebarButton();
            this.cmsLarry = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiNewTweet = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiNewDM = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiGoToUser = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiMarkAllRead = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiPreferences = new System.Windows.Forms.ToolStripMenuItem();
            this.tmlTimeline = new Twitter.Controls.Timeline();
            this.tsbTimelineScroller = new Twitter.Controls.ThinScrollbar();
            this.pnlSidebar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLarry)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbAvatar)).BeginInit();
            this.cmsLarry.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlSidebar
            // 
            this.pnlSidebar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.pnlSidebar.Controls.Add(this.pbLarry);
            this.pnlSidebar.Controls.Add(this.pbAvatar);
            this.pnlSidebar.Controls.Add(this.sbbSearch);
            this.pnlSidebar.Controls.Add(this.sbbPeople);
            this.pnlSidebar.Controls.Add(this.sbbLists);
            this.pnlSidebar.Controls.Add(this.sbbMessages);
            this.pnlSidebar.Controls.Add(this.sbbReplies);
            this.pnlSidebar.Controls.Add(this.sbbTimeline);
            this.pnlSidebar.Location = new System.Drawing.Point(0, 0);
            this.pnlSidebar.Name = "pnlSidebar";
            this.pnlSidebar.Size = new System.Drawing.Size(68, 676);
            this.pnlSidebar.TabIndex = 5;
            // 
            // pbLarry
            // 
            this.pbLarry.Image = ((System.Drawing.Image)(resources.GetObject("pbLarry.Image")));
            this.pbLarry.Location = new System.Drawing.Point(17, 647);
            this.pbLarry.Name = "pbLarry";
            this.pbLarry.Size = new System.Drawing.Size(36, 16);
            this.pbLarry.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbLarry.TabIndex = 6;
            this.pbLarry.TabStop = false;
            this.pbLarry.Click += new System.EventHandler(this.pbLarry_Click);
            // 
            // pbAvatar
            // 
            this.pbAvatar.Location = new System.Drawing.Point(11, 10);
            this.pbAvatar.Name = "pbAvatar";
            this.pbAvatar.Size = new System.Drawing.Size(47, 46);
            this.pbAvatar.TabIndex = 6;
            this.pbAvatar.TabStop = false;
            // 
            // sbbSearch
            // 
            this.sbbSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.sbbSearch.Image = ((System.Drawing.Bitmap)(resources.GetObject("sbbSearch.Image")));
            this.sbbSearch.Location = new System.Drawing.Point(22, 265);
            this.sbbSearch.MouseDownImage = null;
            this.sbbSearch.Name = "sbbSearch";
            this.sbbSearch.NewActivity = false;
            this.sbbSearch.Selected = false;
            this.sbbSearch.SelectedImage = ((System.Drawing.Bitmap)(resources.GetObject("sbbSearch.SelectedImage")));
            this.sbbSearch.Size = new System.Drawing.Size(46, 28);
            this.sbbSearch.TabIndex = 9;
            this.sbbSearch.Click += new System.EventHandler(this.sbbSearch_Click);
            // 
            // sbbPeople
            // 
            this.sbbPeople.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.sbbPeople.Image = ((System.Drawing.Bitmap)(resources.GetObject("sbbPeople.Image")));
            this.sbbPeople.Location = new System.Drawing.Point(20, 223);
            this.sbbPeople.MouseDownImage = null;
            this.sbbPeople.Name = "sbbPeople";
            this.sbbPeople.NewActivity = false;
            this.sbbPeople.Selected = false;
            this.sbbPeople.SelectedImage = ((System.Drawing.Bitmap)(resources.GetObject("sbbPeople.SelectedImage")));
            this.sbbPeople.Size = new System.Drawing.Size(48, 28);
            this.sbbPeople.TabIndex = 8;
            this.sbbPeople.Click += new System.EventHandler(this.sbbPeople_Click);
            // 
            // sbbLists
            // 
            this.sbbLists.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.sbbLists.Image = ((System.Drawing.Bitmap)(resources.GetObject("sbbLists.Image")));
            this.sbbLists.Location = new System.Drawing.Point(20, 188);
            this.sbbLists.MouseDownImage = null;
            this.sbbLists.Name = "sbbLists";
            this.sbbLists.NewActivity = false;
            this.sbbLists.Selected = false;
            this.sbbLists.SelectedImage = ((System.Drawing.Bitmap)(resources.GetObject("sbbLists.SelectedImage")));
            this.sbbLists.Size = new System.Drawing.Size(48, 28);
            this.sbbLists.TabIndex = 7;
            this.sbbLists.Click += new System.EventHandler(this.sbbLists_Click);
            // 
            // sbbMessages
            // 
            this.sbbMessages.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.sbbMessages.Image = ((System.Drawing.Bitmap)(resources.GetObject("sbbMessages.Image")));
            this.sbbMessages.Location = new System.Drawing.Point(20, 150);
            this.sbbMessages.MouseDownImage = null;
            this.sbbMessages.Name = "sbbMessages";
            this.sbbMessages.NewActivity = false;
            this.sbbMessages.Selected = false;
            this.sbbMessages.SelectedImage = ((System.Drawing.Bitmap)(resources.GetObject("sbbMessages.SelectedImage")));
            this.sbbMessages.Size = new System.Drawing.Size(48, 28);
            this.sbbMessages.TabIndex = 6;
            this.sbbMessages.Click += new System.EventHandler(this.sbbMessages_Click);
            // 
            // sbbReplies
            // 
            this.sbbReplies.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.sbbReplies.Image = ((System.Drawing.Bitmap)(resources.GetObject("sbbReplies.Image")));
            this.sbbReplies.Location = new System.Drawing.Point(20, 111);
            this.sbbReplies.MouseDownImage = null;
            this.sbbReplies.Name = "sbbReplies";
            this.sbbReplies.NewActivity = false;
            this.sbbReplies.Selected = false;
            this.sbbReplies.SelectedImage = ((System.Drawing.Bitmap)(resources.GetObject("sbbReplies.SelectedImage")));
            this.sbbReplies.Size = new System.Drawing.Size(48, 28);
            this.sbbReplies.TabIndex = 5;
            this.sbbReplies.Click += new System.EventHandler(this.sbbReplies_Click);
            // 
            // sbbTimeline
            // 
            this.sbbTimeline.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.sbbTimeline.Image = ((System.Drawing.Bitmap)(resources.GetObject("sbbTimeline.Image")));
            this.sbbTimeline.Location = new System.Drawing.Point(22, 73);
            this.sbbTimeline.MouseDownImage = null;
            this.sbbTimeline.Name = "sbbTimeline";
            this.sbbTimeline.NewActivity = false;
            this.sbbTimeline.Selected = true;
            this.sbbTimeline.SelectedImage = ((System.Drawing.Bitmap)(resources.GetObject("sbbTimeline.SelectedImage")));
            this.sbbTimeline.Size = new System.Drawing.Size(46, 28);
            this.sbbTimeline.TabIndex = 4;
            this.sbbTimeline.Click += new System.EventHandler(this.sbbTimeline_Click);
            // 
            // cmsLarry
            // 
            this.cmsLarry.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiNewTweet,
            this.tsmiNewDM,
            this.toolStripMenuItem1,
            this.tsmiGoToUser,
            this.tsmiMarkAllRead,
            this.toolStripMenuItem2,
            this.tsmiPreferences});
            this.cmsLarry.Name = "cmsLarry";
            this.cmsLarry.Size = new System.Drawing.Size(257, 148);
            // 
            // tsmiNewTweet
            // 
            this.tsmiNewTweet.Name = "tsmiNewTweet";
            this.tsmiNewTweet.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.tsmiNewTweet.Size = new System.Drawing.Size(256, 22);
            this.tsmiNewTweet.Text = "New Tweet";
            this.tsmiNewTweet.Click += new System.EventHandler(this.tsmiNewTweet_Click);
            // 
            // tsmiNewDM
            // 
            this.tsmiNewDM.Name = "tsmiNewDM";
            this.tsmiNewDM.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.N)));
            this.tsmiNewDM.Size = new System.Drawing.Size(256, 22);
            this.tsmiNewDM.Text = "New Direct Message";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(253, 6);
            // 
            // tsmiGoToUser
            // 
            this.tsmiGoToUser.Name = "tsmiGoToUser";
            this.tsmiGoToUser.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.U)));
            this.tsmiGoToUser.Size = new System.Drawing.Size(256, 22);
            this.tsmiGoToUser.Text = "Go to User";
            // 
            // tsmiMarkAllRead
            // 
            this.tsmiMarkAllRead.Name = "tsmiMarkAllRead";
            this.tsmiMarkAllRead.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.K)));
            this.tsmiMarkAllRead.Size = new System.Drawing.Size(256, 22);
            this.tsmiMarkAllRead.Text = "Mark All as Read";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(253, 6);
            // 
            // tsmiPreferences
            // 
            this.tsmiPreferences.Name = "tsmiPreferences";
            this.tsmiPreferences.Size = new System.Drawing.Size(256, 22);
            this.tsmiPreferences.Text = "Preferences";
            this.tsmiPreferences.Click += new System.EventHandler(this.tsmiPreferences_Click);
            // 
            // tmlTimeline
            // 
            this.tmlTimeline.BackColor = System.Drawing.Color.White;
            this.tmlTimeline.Location = new System.Drawing.Point(67, 0);
            this.tmlTimeline.Name = "tmlTimeline";
            this.tmlTimeline.ScrolledToTop = false;
            this.tmlTimeline.Size = new System.Drawing.Size(459, 0);
            this.tmlTimeline.TabIndex = 3;
            // 
            // tsbTimelineScroller
            // 
            this.tsbTimelineScroller.BackColor = System.Drawing.Color.White;
            this.tsbTimelineScroller.HandleColor = System.Drawing.Color.DarkGray;
            this.tsbTimelineScroller.LargeChange = 30;
            this.tsbTimelineScroller.Location = new System.Drawing.Point(529, 1);
            this.tsbTimelineScroller.Max = 100;
            this.tsbTimelineScroller.Name = "tsbTimelineScroller";
            this.tsbTimelineScroller.Size = new System.Drawing.Size(7, 663);
            this.tsbTimelineScroller.TabIndex = 6;
            this.tsbTimelineScroller.Value = 0;
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(538, 675);
            this.Controls.Add(this.tsbTimelineScroller);
            this.Controls.Add(this.pnlSidebar);
            this.Controls.Add(this.tmlTimeline);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Twitter";
            this.pnlSidebar.ResumeLayout(false);
            this.pnlSidebar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLarry)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbAvatar)).EndInit();
            this.cmsLarry.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.Timeline tmlTimeline;
        private Controls.SidebarButton sbbTimeline;
        private System.Windows.Forms.Panel pnlSidebar;
        private Controls.SidebarButton sbbReplies;
        private Controls.SidebarButton sbbMessages;
        private Controls.SidebarButton sbbLists;
        private Controls.SidebarButton sbbPeople;
        private Controls.SidebarButton sbbSearch;
        private System.Windows.Forms.PictureBox pbAvatar;
        private System.Windows.Forms.PictureBox pbLarry;
        private System.Windows.Forms.ContextMenuStrip cmsLarry;
        private System.Windows.Forms.ToolStripMenuItem tsmiNewTweet;
        private System.Windows.Forms.ToolStripMenuItem tsmiNewDM;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem tsmiGoToUser;
        private System.Windows.Forms.ToolStripMenuItem tsmiMarkAllRead;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem tsmiPreferences;
        private Controls.ThinScrollbar tsbTimelineScroller;

    }
}

