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
        private int m_iLargeChange = 10;  //the step
        private int m_iHandleTop = 0;
        private int m_iHandleBottom = 0;

        private int m_iDragSpace;
        private SolidBrush m_sbHandleBrush;

        public ThinScrollbar()
        {
            InitializeComponent();

            m_sbHandleBrush = new SolidBrush(Color.Black);
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
            m_iHandleTop = (int)((this.Height - m_iLargeChange) * fPercentPos);
            m_iHandleBottom = m_iHandleTop + m_iLargeChange;

            //this is how tall the scroll handle should be
            int iHandleHeight = this.Height / (m_iMax / m_iLargeChange);

            e.Graphics.FillEllipse(m_sbHandleBrush, 0, m_iHandleTop, C_CONTROL_WIDTH, C_CONTROL_WIDTH);
            e.Graphics.FillEllipse(m_sbHandleBrush, 0, m_iHandleBottom - C_CONTROL_WIDTH, C_CONTROL_WIDTH, C_CONTROL_WIDTH);
            e.Graphics.FillRectangle(m_sbHandleBrush, 0, m_iHandleTop + (C_CONTROL_WIDTH / 2), C_CONTROL_WIDTH, (m_iHandleBottom - m_iHandleTop) - C_CONTROL_WIDTH);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if ((e.Y >= m_iHandleTop) && (e.Y <= m_iHandleBottom))
                m_iDragSpace = m_iHandleTop - e.Y;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if ((e.Y >= m_iHandleTop) && (e.Y <= m_iHandleBottom))
                {
                    //calculate m_iValue, then invalidate

                    //this is the new top of the scroll handle
                    float fNewTop = e.Y + m_iDragSpace;  //drag space is the space between the top of the handle and where the mouse has grabbed it

                    //this is the percent scrolled the new top represents
                    float fPercentPos = fNewTop / (this.Height - m_iLargeChange);

                    //this is the new scroll value
                    int iNewVal = (int)(m_iMax * fPercentPos);

                    if ((iNewVal >= 0) && (iNewVal <= m_iMax))
                        this.Value = iNewVal;
                }
            }
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

        public Color HandleColor
        {
            get { return m_sbHandleBrush.Color; }
            set { m_sbHandleBrush.Color = value; this.Invalidate(); }
        }
    }
}
