using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Twitter
{
    public class LinearMotionAnimation
    {
        public enum MotionType
        {
            Linear = 1,
            EaseIn = 2,
            EaseOut = 3
        }

        private MotionType m_mtMotionType;

        // these represent our trajectory
        private Point m_ptStart, m_ptEnd;
        private int m_iDuration; //miliseconds
        private float m_fRise, m_fRun;
        private float m_fSlope;
        private float m_fYIntercept;

        public LinearMotionAnimation(Point ptStart, Point ptEnd, int iDuration, MotionType mtType = MotionType.Linear)
        {
            m_ptStart = ptStart;
            m_ptEnd = ptEnd;
            m_iDuration = iDuration;
            m_mtMotionType = mtType;

            m_fRise = ptEnd.Y - ptStart.Y;
            m_fRun = ptEnd.X - ptStart.X;

            if (m_fRun == 0.0f)
                m_fRun = 1;

            m_fSlope = m_fRise / m_fRun;
            m_fYIntercept = ptStart.Y - (int)(m_fSlope * ptStart.X);
        }

        public MotionType Type
        {
            get { return m_mtMotionType; }
        }

        //iElapsed is in miliseconds
        public Point PositionForTime(int iElapsed)
        {
            float fPercentProgress = 0.0f;

            switch (m_mtMotionType)
            {
                case MotionType.Linear:
                    fPercentProgress = (float)iElapsed / (float)m_iDuration;
                    break;

                //ease in/out both map the percent progress to a parabolic percentage (x^2) between 0 and 1 inclusive
                case MotionType.EaseOut:
                    fPercentProgress = (float)Math.Pow((float)iElapsed / (float)m_iDuration, 5);
                    break;
                case MotionType.EaseIn:
                    fPercentProgress = ((float)Math.Pow((float)iElapsed / (float)m_iDuration, (1.0f / 4.0f)));
                    break;
            }

            float fX = m_fRun * fPercentProgress;
            float fY = (m_fSlope * fX);// +m_fYIntercept;
            Point ptFinal = new Point((int)fX + m_ptStart.X, (int)fY + (int)m_fYIntercept);
            return ptFinal;
        }

        public int Duration
        {
            get { return m_iDuration; }
        }
    }
}
