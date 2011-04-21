using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Text.RegularExpressions;

namespace Twitter.API
{
    public class StatusText
    {
        private static Color c_clrNormalText = Color.Black;
        private static Color c_clrURL = Color.FromArgb(47, 83, 114);
        private static Color c_clrHashtag = Color.FromArgb(108, 108, 108);
        private static Color c_clrScreenName = c_clrURL;

        private List<StatusTextElement> m_ltstWords;
        private string m_sText;

        private StatusText() { }

        public static StatusText FromString(string sFromStr)
        {
            StatusText stFinal = new StatusText();
            stFinal.m_ltstWords = StatusTextElement.ListFromString(sFromStr);
            stFinal.m_sText = sFromStr;

            return stFinal;
        }

        public StatusTextElement FindWord(int iCharIndex)
        {
            for (int i = 0; i < m_ltstWords.Count; i++)
            {
                if ((iCharIndex >= m_ltstWords[i].CharStart) && (iCharIndex <= m_ltstWords[i].CharEnd))
                    return m_ltstWords[i];
            }

            return null;
        }

        public string ToRTF(Font fntFont)
        {
            StringBuilder sbFinal = new StringBuilder("{\\rtf1\\ansi\\deff0{\\fonttbl{\\f0\\fnil\\fcharset0 " + fntFont.FontFamily.Name + ";}");
            
            //compute the color table
            sbFinal.Append("{\\colortbl;");
            sbFinal.Append(ColorToRTF(c_clrNormalText));    //cf1
            sbFinal.Append(ColorToRTF(c_clrURL));           //cf2
            sbFinal.Append(ColorToRTF(c_clrHashtag));       //cf3
            sbFinal.Append(ColorToRTF(c_clrScreenName));    //cf4
            sbFinal.Append("}");

            //ending header curly
            sbFinal.Append("}");

            //set view, paragraph, and font size (font size is always double)
            sbFinal.Append("\\viewkind4\\uc1\\pard\\fs" + ((fntFont.Size) * 2).ToString());

            //add all text elements
            for (int i = 0; i < m_ltstWords.Count; i++)
            {
                switch (m_ltstWords[i].Type)
                {
                    case StatusTextElement.StatusTextElementType.URL:
                        sbFinal.Append("\\cf2 "); break;
                    case StatusTextElement.StatusTextElementType.Hashtag:
                        sbFinal.Append("\\cf3 "); break;
                    case StatusTextElement.StatusTextElementType.ScreenName:
                        sbFinal.Append("\\cf4 "); break;
                    default:
                        sbFinal.Append("\\cf1 "); break;  //normal case
                }

                sbFinal.Append(GetRTFUnicodeEscapedString(m_ltstWords[i].Text));
            }

            //ending body curly
            sbFinal.Append("}");

            return sbFinal.ToString();
        }

        private static string GetRTFUnicodeEscapedString(string sToConvert)
        {
            StringBuilder sbFinal = new StringBuilder();

            foreach (char cChr in sToConvert)
            {
                if (cChr <= 0x7f)
                    sbFinal.Append(cChr);
                else
                    sbFinal.Append("\\u" + Convert.ToUInt32(cChr) + "?");
            }

            return sbFinal.ToString();
        }


        private static string ColorToRTF(Color cToTranslate)
        {
            return "\\red" + cToTranslate.R.ToString() + "\\green" + cToTranslate.G.ToString() + "\\blue" + cToTranslate.B.ToString() + ";";
        }

        public List<StatusTextElement> Words
        {
            get { return m_ltstWords; }
        }

        public string Text
        {
            get { return m_sText; }
        }
    }

    public class StatusTextElement
    {
        public enum StatusTextElementType
        {
            Normal = 1,
            URL = 2,
            Hashtag = 3,
            ScreenName = 4
        }

        private static char[] c_acSplitters = new char[1] { ' ' };
        private const string C_TWEET_SPLIT_REGEX = @"(@\w+)|(#\w+)|(http\:\/\/[^\s]+)";

        private string m_sText;
        private StatusTextElementType m_teType;
        private int m_iCharStart;
        private int m_iCharEnd;

        public StatusTextElement()
        {
            m_sText = "";
        }

        public string Text
        {
            get { return m_sText; }
            set { m_sText = value; }
        }

        public StatusTextElementType Type
        {
            get { return m_teType; }
        }

        public int CharStart
        {
            get { return m_iCharStart; }
            set { m_iCharStart = value; }
        }

        public int CharEnd
        {
            get { return m_iCharEnd; }
            set { m_iCharEnd = value; }
        }

        public static List<StatusTextElement> ListFromString(string sToParse)
        {
            //string[] asWords = sToParse.Split(c_acSplitters);
            string[] asWords = Regex.Split(sToParse, C_TWEET_SPLIT_REGEX);
            StatusTextElement tstCurElement;
            List<StatusTextElement> ltstFinal = new List<StatusTextElement>();
            int iCharCounter = 0;

            for (int i = 0; i < asWords.Length; i++)
            {
                tstCurElement = new StatusTextElement()
                {
                    m_sText = asWords[i]
                };

                if (asWords[i].Contains("http://"))
                    tstCurElement.m_teType = StatusTextElementType.URL;
                else if ((asWords[i].Length > 0) && (asWords[i][0] == '@'))
                    tstCurElement.m_teType = StatusTextElementType.ScreenName;
                else if ((asWords[i].Length > 0) && (asWords[i][0] == '#'))
                    tstCurElement.m_teType = StatusTextElementType.Hashtag;
                else
                    tstCurElement.m_teType = StatusTextElementType.Normal;

                tstCurElement.m_iCharStart = iCharCounter;
                iCharCounter += asWords[i].Length;
                tstCurElement.m_iCharEnd = iCharCounter;

                ltstFinal.Add(tstCurElement);
            }

            return ltstFinal;
        }
    }
}
