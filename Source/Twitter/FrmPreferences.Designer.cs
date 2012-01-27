namespace Twitter
{
    partial class FrmPreferences
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPreferences));
            this.tcTabs = new System.Windows.Forms.TabControl();
            this.tpGeneral = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.tpAccounts = new System.Windows.Forms.TabPage();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.lbAccounts = new System.Windows.Forms.ListBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tcTabs.SuspendLayout();
            this.tpGeneral.SuspendLayout();
            this.tpAccounts.SuspendLayout();
            this.SuspendLayout();
            // 
            // tcTabs
            // 
            this.tcTabs.Controls.Add(this.tpGeneral);
            this.tcTabs.Controls.Add(this.tpAccounts);
            this.tcTabs.Location = new System.Drawing.Point(12, 12);
            this.tcTabs.Name = "tcTabs";
            this.tcTabs.SelectedIndex = 0;
            this.tcTabs.Size = new System.Drawing.Size(352, 224);
            this.tcTabs.TabIndex = 0;
            // 
            // tpGeneral
            // 
            this.tpGeneral.Controls.Add(this.label3);
            this.tpGeneral.Controls.Add(this.checkBox1);
            this.tpGeneral.Controls.Add(this.label2);
            this.tpGeneral.Controls.Add(this.label1);
            this.tpGeneral.Controls.Add(this.comboBox1);
            this.tpGeneral.Location = new System.Drawing.Point(4, 22);
            this.tpGeneral.Name = "tpGeneral";
            this.tpGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tpGeneral.Size = new System.Drawing.Size(344, 198);
            this.tpGeneral.TabIndex = 0;
            this.tpGeneral.Text = "General";
            this.tpGeneral.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(180, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(141, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "(if already scrolled to the top)";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(163, 54);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(145, 17);
            this.checkBox1.TabIndex = 3;
            this.checkBox1.Text = "Automatically scroll to top";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(16, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(141, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "When new tweets arrive:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(57, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Image service:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "yFrog",
            "TwitPic",
            "Twitgoo",
            "Posterous",
            "img.ly",
            "pix.im"});
            this.comboBox1.Location = new System.Drawing.Point(163, 14);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 0;
            // 
            // tpAccounts
            // 
            this.tpAccounts.Controls.Add(this.btnRemove);
            this.tpAccounts.Controls.Add(this.btnAdd);
            this.tpAccounts.Controls.Add(this.lbAccounts);
            this.tpAccounts.Location = new System.Drawing.Point(4, 22);
            this.tpAccounts.Name = "tpAccounts";
            this.tpAccounts.Padding = new System.Windows.Forms.Padding(3);
            this.tpAccounts.Size = new System.Drawing.Size(344, 198);
            this.tpAccounts.TabIndex = 1;
            this.tpAccounts.Text = "Accounts";
            this.tpAccounts.UseVisualStyleBackColor = true;
            // 
            // btnRemove
            // 
            this.btnRemove.Enabled = false;
            this.btnRemove.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRemove.Location = new System.Drawing.Point(39, 163);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(26, 23);
            this.btnRemove.TabIndex = 2;
            this.btnRemove.Text = "-";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.Location = new System.Drawing.Point(12, 163);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(26, 23);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "+";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // lbAccounts
            // 
            this.lbAccounts.FormattingEnabled = true;
            this.lbAccounts.Location = new System.Drawing.Point(12, 12);
            this.lbAccounts.Name = "lbAccounts";
            this.lbAccounts.Size = new System.Drawing.Size(320, 147);
            this.lbAccounts.TabIndex = 0;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(289, 242);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(208, 242);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // FrmPreferences
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(375, 276);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.tcTabs);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmPreferences";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Preferences";
            this.tcTabs.ResumeLayout(false);
            this.tpGeneral.ResumeLayout(false);
            this.tpGeneral.PerformLayout();
            this.tpAccounts.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tcTabs;
        private System.Windows.Forms.TabPage tpGeneral;
        private System.Windows.Forms.TabPage tpAccounts;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox lbAccounts;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnAdd;
    }
}
