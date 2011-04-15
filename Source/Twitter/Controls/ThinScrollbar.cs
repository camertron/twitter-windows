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
        public event ScrollEventHandler Scroll;

        private const int C_CONTROL_WIDTH = 6;
        private const int C_LARGE_CHANGE_DEFAULT = 10;
        private const int C_VALUE_DEFAULT = 0;
        private const int C_MAX_DEFAULT = 100;

        private int m_iMax = C_MAX_DEFAULT;
        private int m_iValue = C_VALUE_DEFAULT;
        private int m_iLargeChange = C_LARGE_CHANGE_DEFAULT;  //the height of the scroll handle
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
                    int iOldVal = m_iValue;

                    if (iNewVal < 0)
                        iNewVal = 0;
                    else if (iNewVal > m_iMax)
                        iNewVal = m_iMax;
                    else
                        this.Value = iNewVal;

                    if (Scroll != null)
                    {
                        ScrollEventType setScrollType;

                        if (iNewVal == 0)
                            setScrollType = ScrollEventType.First;
                        else if (iNewVal == m_iMax)
                            setScrollType = ScrollEventType.Last;
                        else if (iNewVal > m_iValue)
                            setScrollType = ScrollEventType.SmallIncrement;
                        else
                            setScrollType = ScrollEventType.SmallDecrement;

                        Scroll(this, new ScrollEventArgs(setScrollType, iOldVal, iNewVal, ScrollOrientation.VerticalScroll));
                    }
                }
            }
        }

        public int Max
        {
            get { return m_iMax; }
            set
            {
                if (value <= 0)
                    m_iMax = C_MAX_DEFAULT;
                else
                    m_iMax = value;

                this.Invalidate();
            }
        }

        public int Value
        {
            get { return m_iValue; }
            set
            {
                if (value > m_iMax)
                    m_iValue = m_iMax;
                else if (value < 0)
                    m_iValue = 0;
                else
                    m_iValue = value;

                this.Invalidate();
            }
        }

        public int LargeChange
        {
            get { return m_iLargeChange; }
            set
            {
                if (value < 1)
                    m_iLargeChange = C_LARGE_CHANGE_DEFAULT;
                else if (value > m_iMax)
                    m_iLargeChange = m_iMax;
                else
                    m_iLargeChange = value;

                this.Invalidate();
            }
        }

        public Color HandleColor
        {
            get { return m_sbHandleBrush.Color; }
            set { m_sbHandleBrush.Color = value; this.Invalidate(); }
        }
    }
}
