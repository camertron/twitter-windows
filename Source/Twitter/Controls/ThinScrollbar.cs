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
    public partial class ThinScrollbar : UserControl
    {
        private const int C_CONTROL_WIDTH = 6;

        private int m_iMax = 100;
        private int m_iValue = 0;
        private int m_iLargeChange = 10;  //the step (also the 

        public ThinScrollbar()
        {
            InitializeComponent();
        }

        protected override void OnResize(EventArgs e)
        {
            this.Width = C_CONTROL_WIDTH + 1;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //smoothing
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            //this is the percent scrolled so far
            float fPercentPos = (float)m_iValue / (float)m_iMax;

            //this is where the top and bottom of the scroll handle should be
            int iHandleTop = (int)((this.Height - m_iLargeChange) * fPercentPos);
            int iHandleBottom = iHandleTop + m_iLargeChange;

            //this is how tall the scroll handle should be
            int iHandleHeight = this.Height / (m_iMax / m_iLargeChange);

            e.Graphics.FillEllipse(new SolidBrush(Color.Black), 0, iHandleTop, C_CONTROL_WIDTH, C_CONTROL_WIDTH);
            e.Graphics.FillEllipse(new SolidBrush(Color.Black), 0, iHandleBottom - C_CONTROL_WIDTH, C_CONTROL_WIDTH, C_CONTROL_WIDTH);
            e.Graphics.FillRectangle(new SolidBrush(Color.Black), 0, iHandleTop + (C_CONTROL_WIDTH / 2), C_CONTROL_WIDTH, (iHandleBottom - iHandleTop) - C_CONTROL_WIDTH);
        }

        public int Max
        {
            get { return m_iMax; }
            set { m_iMax = value; this.Invalidate(); }
        }

        public int Value
        {
            get { return m_iValue; }
            set { m_iValue = value; this.Invalidate(); }
        }

        public int LargeChange
        {
            get { return m_iLargeChange; }
            set { m_iLargeChange = value; this.Invalidate(); }
        }
    }
}
