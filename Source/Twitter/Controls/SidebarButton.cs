using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Twitter.Controls
{
    public partial class SidebarButton : UserControl
    {
        private Bitmap m_bmpImage = null;
        private Bitmap m_bmpSelected = null;
        private Bitmap m_bmpMouseDown = null;
        public bool m_bSelected = false;

        public SidebarButton()
        {
            InitializeComponent();
            this.Resize += new EventHandler(SidebarButton_Resize);
        }

        private void SidebarButton_Resize(object sender, EventArgs e)
        {
            pbNewActivity.Left = this.Width - pbNewActivity.Width;
            this.Height = 28;
        }

        public Bitmap SelectedImage
        {
            get { return m_bmpSelected; }
            set { m_bmpSelected = value; this.Invalidate(); }
        }

        public Bitmap MouseDownImage
        {
            get { return m_bmpMouseDown; }
            set { m_bmpMouseDown = value; }
        }

        public Bitmap Image
        {
            get { return m_bmpImage; }
            set { m_bmpImage = value; this.Invalidate(); }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (m_bSelected)
            {
                if (m_bmpSelected != null)
                    e.Graphics.DrawImage(m_bmpSelected, 0, 0);
            }
            else
            {
                if (m_bmpImage != null)
                    e.Graphics.DrawImage(m_bmpImage, 0, 0);
            }

            base.OnPaint(e);
        }

        public bool Selected
        {
            get { return m_bSelected; }
            set { m_bSelected = value; this.Invalidate(); }
        }

        public bool NewActivity
        {
            get { return pbNewActivity.Visible; }
            set { pbNewActivity.Visible = value; }
        }
    }
}
