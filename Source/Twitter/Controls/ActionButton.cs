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
    public partial class ActionButton : UserControl
    {
        private const int C_ACTION_BUTTON_SHADE = 50;

        private Bitmap m_bmpNormalImage = null;
        private Bitmap m_bmpMouseOverImage = null;
        private bool m_bMouseOver = false;

        public ActionButton()
        {
            InitializeComponent();

            this.MouseEnter += new EventHandler(ActionButton_MouseEnter);
            this.MouseLeave += new EventHandler(ActionButton_MouseLeave);
        }

        private void ActionButton_MouseLeave(object sender, EventArgs e)
        {
            m_bMouseOver = false;
            this.Invalidate();
        }

        private void ActionButton_MouseEnter(object sender, EventArgs e)
        {
            m_bMouseOver = true;
            this.Invalidate();
        }

        public string ToolTipText
        {
            get { return ttToolTip.GetToolTip(this); }
            set { ttToolTip.SetToolTip(this, value); }
        }

        [EditorBrowsable]
        public Bitmap Image
        {
            get { return m_bmpNormalImage; }
            set
            {
                Color clrCur, clrNew;

                m_bmpNormalImage = value;

                if (m_bmpNormalImage != null)
                {
                    m_bmpMouseOverImage = new Bitmap(m_bmpNormalImage);

                    for (int r = 0; r < m_bmpNormalImage.Height; r++)
                    {
                        for (int c = 0; c < m_bmpNormalImage.Width; c++)
                        {
                            clrCur = m_bmpNormalImage.GetPixel(c, r);
                            clrNew = Color.FromArgb(clrCur.A, clrCur.R - C_ACTION_BUTTON_SHADE, clrCur.G - C_ACTION_BUTTON_SHADE, clrCur.B - C_ACTION_BUTTON_SHADE);
                            m_bmpMouseOverImage.SetPixel(c, r, clrNew);
                        }
                    }

                    this.Invalidate();
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (m_bMouseOver)
            {
                if (m_bmpMouseOverImage != null)
                    e.Graphics.DrawImage((Image)m_bmpMouseOverImage, 0, 0);
            }
            else
            {
                if (m_bmpNormalImage != null)
                    e.Graphics.DrawImage((Image)m_bmpNormalImage, 0, 0);
            }

            base.OnPaint(e);
        }
    }
}
