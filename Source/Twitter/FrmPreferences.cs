using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Twitter
{
    public partial class FrmPreferences : Form
    {
        public event EventHandler CancelClicked;
        public event EventHandler OkClicked;

        public FrmPreferences()
        {
            InitializeComponent();

            this.FormClosing += new FormClosingEventHandler(FrmPreferences_FormClosing);

            LinearMotionAnimation lma = new LinearMotionAnimation(new Point(0, 0), new Point(5, 5), 10, LinearMotionAnimation.MotionType.EaseIn);
            Point pt = lma.PositionForTime(2);
            pt = lma.PositionForTime(4);
            pt = lma.PositionForTime(6);
            pt = lma.PositionForTime(8);
            pt = lma.PositionForTime(10);
        }

        private void FrmPreferences_FormClosing(object sender, FormClosingEventArgs e)
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
    }
}
