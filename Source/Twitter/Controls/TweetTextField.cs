using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

namespace Twitter.Controls
{
    public partial class TweetTextField : UserControl
    {
        public delegate void TextElementClickHandler(object sender, TweetTextElement tstElement);
        public event TextElementClickHandler TextElementClicked;
        public new event EventHandler TextChanged;

        private Pen m_pnBorderPen;
        private TimelineStatusText m_tstStatusText;
        private Font m_fntFont;
        private BorderStyle m_bsBorderStyle;
        private bool m_bConstrictHeight;
        private bool m_bAlreadyUpdating;
        private int m_iControlMargin;

        private const int C_TEXT_HEIGHT = 15;

        [DllImport("user32.dll", EntryPoint = "HideCaret")]
        public static extern long HideCaret(IntPtr hwnd);

        /*protected override void WndProc(ref Message m)
        {
            try
            {
                base.WndProc(ref m);
                HideCaret(rtbTextBox.Handle);
            }
            catch { }  //can throw errors when this object is being destroyed
        }*/

        public TweetTextField()
        {
            InitializeComponent();

            //default border color
            m_pnBorderPen = new Pen(Color.FromArgb(180, 180, 180));
            m_fntFont = new Font("Arial", 9);
            m_bsBorderStyle = BorderStyle.None;
            base.BorderStyle = System.Windows.Forms.BorderStyle.None;
            m_bConstrictHeight = true;
            m_bAlreadyUpdating = false;
            m_iControlMargin = 1;

            rtbTextBox.Click += new EventHandler(rtbTextBox_Click);
            rtbTextBox.TextChanged += new EventHandler(rtbTextBox_TextChanged);
            rtbTextBox.MouseWheel += new MouseEventHandler(rtbTextBox_MouseWheel);
        }

        private void rtbTextBox_MouseWheel(object sender, MouseEventArgs e)
        {
            MessageBox.Show("rtbTextBox");
            OnMouseWheel(e);
        }

        public new bool Focus()
        {
            base.Focus();
            return rtbTextBox.Focus();
        }

        protected override void OnGotFocus(EventArgs e)
        {
            rtbTextBox.Focus();
        }

        private void rtbTextBox_TextChanged(object sender, EventArgs e)
        {
            UpdateText(rtbTextBox.Text);

            if (TextChanged != null)
                TextChanged(this, e);
        }

        private void rtbTextBox_Click(object sender, EventArgs e)
        {
            if (m_tstStatusText != null)
            {
                Point ptCursorLoc = rtbTextBox.PointToClient(Cursor.Position);
                int iCharIndex = rtbTextBox.GetCharIndexFromPosition(ptCursorLoc);
                TweetTextElement tstElement = m_tstStatusText.FindWord(iCharIndex);

                if ((tstElement != null) && (TextElementClicked != null))
                    TextElementClicked(this, tstElement);
            }
        }

        public int SelectionStart
        {
            get { return rtbTextBox.SelectionStart; }
            set { rtbTextBox.SelectionStart = value; }
        }

        public int SelectionLength
        {
            get { return rtbTextBox.SelectionLength; }
            set { rtbTextBox.SelectionLength = value; }
        }

        public int ControlMargin
        {
            get { return m_iControlMargin; }
            set { m_iControlMargin = value; this.OnResize(null); }
        }

        public override Cursor Cursor
        {
            get { return base.Cursor; }
            set { base.Cursor = value; rtbTextBox.Cursor = value; }
        }

        public override Font Font
        {
            get { return m_fntFont; }
            set { m_fntFont = value; UpdateText(rtbTextBox.Text); }
        }

        public bool ConstrictHeight
        {
            get { return m_bConstrictHeight; }
            set { m_bConstrictHeight = value; this.OnResize(null); }
        }

        public RichTextBoxScrollBars ScrollBars
        {
            get { return rtbTextBox.ScrollBars; }
            set { rtbTextBox.ScrollBars = value; }
        }

        public bool ReadOnly
        {
            get { return rtbTextBox.ReadOnly; }
            set { rtbTextBox.ReadOnly = value; }
        }

        [EditorBrowsable]
        public override string Text
        {
            get { return rtbTextBox.Text; }
            set { rtbTextBox.Text = value; }
        }

        private void UpdateText(string sNewText)
        {
            if (! m_bAlreadyUpdating)
            {
                m_bAlreadyUpdating = true;

                int iStart = rtbTextBox.SelectionStart;
                m_tstStatusText = TimelineStatusText.FromString(sNewText, m_fntFont);
                rtbTextBox.Rtf = m_tstStatusText.ToRTF();
                rtbTextBox.SelectionStart = iStart;

                m_bAlreadyUpdating = false;
            }
        }

        public void UpdateText()
        {
            UpdateText(rtbTextBox.Text);
        }

        public Color BorderColor
        {
            get { return m_pnBorderPen.Color; }
            set { m_pnBorderPen.Color = value; }
        }

        public BorderStyle ControlBorderStyle
        {
            get { return m_bsBorderStyle; }
            set
            {
                m_bsBorderStyle = value;
                this.Invalidate();
            }
        }

        public override Color BackColor
        {
            get { return base.BackColor; }
            set { base.BackColor = value; rtbTextBox.BackColor = value; }
        }

        protected override void OnResize(EventArgs e)
        {
            rtbTextBox.Left = m_iControlMargin;
            rtbTextBox.Top = m_iControlMargin;
            rtbTextBox.Width = this.Width - (m_iControlMargin * 2);

            if (m_bConstrictHeight)
                this.Height = ((rtbTextBox.GetLineFromCharIndex(rtbTextBox.Text.Length) + 1) * C_TEXT_HEIGHT) + m_iControlMargin;
            else
                rtbTextBox.Height = this.Height - (m_iControlMargin * 2);

            base.OnResize(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.ControlBorderStyle == System.Windows.Forms.BorderStyle.FixedSingle)
                e.Graphics.DrawRectangle(m_pnBorderPen, 0, 0, this.Width - 1, this.Height - 1);

            base.OnPaint(e);
        }
    }

    public class TimelineStatusText
    {
        private static Color c_clrNormalText = Color.Black;
        private static Color c_clrURL = Color.FromArgb(47, 83, 114);
        private static Color c_clrHashtag = Color.FromArgb(108, 108, 108);
        private static Color c_clrScreenName = c_clrURL;
        private Font m_fntFont;

        private List<TweetTextElement> m_ltstWords;
        private string m_sText;

        private TimelineStatusText() { }

        public static TimelineStatusText FromString(string sFromStr, Font fntFont)
        {
            TimelineStatusText tstFinal = new TimelineStatusText();
            tstFinal.m_ltstWords = TweetTextElement.ListFromString(sFromStr);
            tstFinal.m_fntFont = fntFont;
            tstFinal.m_sText = sFromStr;

            return tstFinal;
        }

        public TweetTextElement FindWord(int iCharIndex)
        {
            for (int i = 0; i < m_ltstWords.Count; i++)
            {
                if ((iCharIndex >= m_ltstWords[i].CharStart) && (iCharIndex <= m_ltstWords[i].CharEnd))
                    return m_ltstWords[i];
            }

            return null;
        }

        public string ToRTF()
        {
            StringBuilder sbFinal = new StringBuilder("{\\rtf1\\ansi\\deff0{\\fonttbl{\\f0\\fnil\\fcharset0 " + m_fntFont.FontFamily.Name + ";}");
            
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
            sbFinal.Append("\\viewkind4\\uc1\\pard\\fs" + ((m_fntFont.Size) * 2).ToString());

            //add all text elements
            for (int i = 0; i < m_ltstWords.Count; i++)
            {
                switch (m_ltstWords[i].Type)
                {
                    case TweetTextElement.TextElementType.URL:
                        sbFinal.Append("\\cf2 "); break;
                    case TweetTextElement.TextElementType.Hashtag:
                        sbFinal.Append("\\cf3 "); break;
                    case TweetTextElement.TextElementType.ScreenName:
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

        public List<TweetTextElement> Words
        {
            get { return m_ltstWords; }
        }

        public string Text
        {
            get { return m_sText; }
        }
    }

    public class TweetTextElement
    {
        public enum TextElementType
        {
            Normal = 1,
            URL = 2,
            Hashtag = 3,
            ScreenName = 4
        }

        private static char[] c_acSplitters = new char[1] { ' ' };
        private const string C_TWEET_SPLIT_REGEX = @"(@\w+)|(#\w+)|(http\:\/\/[^\s]+)";

        private string m_sText;
        private TextElementType m_teType;
        private int m_iCharStart;
        private int m_iCharEnd;

        public TweetTextElement()
        {
            m_sText = "";
        }

        public string Text
        {
            get { return m_sText; }
            set { m_sText = value; }
        }

        public TextElementType Type
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

        public static List<TweetTextElement> ListFromString(string sToParse)
        {
            //string[] asWords = sToParse.Split(c_acSplitters);
            string[] asWords = Regex.Split(sToParse, C_TWEET_SPLIT_REGEX);
            TweetTextElement tstCurElement;
            List<TweetTextElement> ltstFinal = new List<TweetTextElement>();
            int iCharCounter = 0;

            for (int i = 0; i < asWords.Length; i++)
            {
                tstCurElement = new TweetTextElement()
                {
                    m_sText = asWords[i]
                };

                if (asWords[i].Contains("http://"))
                    tstCurElement.m_teType = TextElementType.URL;
                else if ((asWords[i].Length > 0) && (asWords[i][0] == '@'))
                    tstCurElement.m_teType = TextElementType.ScreenName;
                else if ((asWords[i].Length > 0) && (asWords[i][0] == '#'))
                    tstCurElement.m_teType = TextElementType.Hashtag;
                else
                    tstCurElement.m_teType = TextElementType.Normal;

                tstCurElement.m_iCharStart = iCharCounter;
                iCharCounter += asWords[i].Length;
                tstCurElement.m_iCharEnd = iCharCounter;

                ltstFinal.Add(tstCurElement);
            }

            return ltstFinal;
        }
    }
}
