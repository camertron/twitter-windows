using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Twitter
{
    public class Imaging
    {
        private static Dictionary<Point, float> c_dOpTableBase = null;

        private static Dictionary<Point, float> CalculateOpTable(Bitmap bmpToCalc)
        {
            Dictionary<Point, float> dFinal = new Dictionary<Point,float>();

            if (c_dOpTableBase == null)
            {
                c_dOpTableBase = new Dictionary<Point, float>();
                c_dOpTableBase.Add(new Point(0, 0), 0.0f);
                c_dOpTableBase.Add(new Point(1, 0), 0.0f);
                c_dOpTableBase.Add(new Point(2, 0), 0.36f);
                c_dOpTableBase.Add(new Point(3, 0), 0.75f);
                c_dOpTableBase.Add(new Point(4, 0), 0.95f);
                c_dOpTableBase.Add(new Point(0, 1), 0.0f);
                c_dOpTableBase.Add(new Point(1, 1), 0.55f);
                c_dOpTableBase.Add(new Point(0, 2), 0.27f);
                c_dOpTableBase.Add(new Point(0, 3), 0.76f);
                c_dOpTableBase.Add(new Point(0, 4), 0.96f);
            }

            //top right
            Dictionary<Point, float>.Enumerator dEnum = c_dOpTableBase.GetEnumerator();

            while (dEnum.MoveNext())
                dFinal.Add(new Point(bmpToCalc.Width - (dEnum.Current.Key.X + 1), dEnum.Current.Key.Y), dEnum.Current.Value);

            //bottom left
            dEnum = c_dOpTableBase.GetEnumerator();

            while (dEnum.MoveNext())
                dFinal.Add(new Point(dEnum.Current.Key.X, bmpToCalc.Height - (dEnum.Current.Key.Y + 1)), dEnum.Current.Value);

            //bottom right
            dEnum = c_dOpTableBase.GetEnumerator();

            while (dEnum.MoveNext())
                dFinal.Add(new Point(bmpToCalc.Width - (dEnum.Current.Key.X + 1), bmpToCalc.Height - (dEnum.Current.Key.Y + 1)), dEnum.Current.Value);

            //copy over all base points
            dEnum = c_dOpTableBase.GetEnumerator();

            while (dEnum.MoveNext())
                dFinal.Add(dEnum.Current.Key, dEnum.Current.Value);

            return dFinal;
        }

        public static Bitmap RoundAvatarCorners(Bitmap bmpAvatar)
        {
            Bitmap bmpFinal = new Bitmap(bmpAvatar.Width, bmpAvatar.Height);
            Color clrOrig, clrNew;
            Point ptCurPoint = new Point(0, 0);
            int iCurAlpha;

            //calculate opacity table
            Dictionary<Point, float> dOpTable = CalculateOpTable(bmpAvatar);

            for (int c = 0; c < bmpAvatar.Width; c++)
            {
                for (int r = 0; r < bmpAvatar.Height; r++)
                {
                    clrOrig = bmpAvatar.GetPixel(c, r);
                    ptCurPoint.X = c;
                    ptCurPoint.Y = r;

                    if (dOpTable.ContainsKey(ptCurPoint))
                    {
                        iCurAlpha = (int)(dOpTable[ptCurPoint] * (float)clrOrig.A);
                        clrNew = Color.FromArgb(iCurAlpha, clrOrig);
                    }
                    else
                        clrNew = clrOrig;

                    bmpFinal.SetPixel(c, r, clrNew);
                }
            }

            return bmpFinal;
        }
    }
}
