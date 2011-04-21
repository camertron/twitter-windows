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
using Twitter.API;
using Twitter.API.Basic;

namespace Twitter.Controls
{
    public partial class TweetTextField : UserControl
    {
        public delegate void TextElementClickHandler(object sender, StatusTextElement stElement);
        public event TextElementClickHandler TextElementClicked;
        public new event EventHandler TextChanged;

        private Pen m_pnBorderPen;
        private StatusText m_stStatusText;
        private Font m_fntFont;
        private BorderStyle m_bsBorderStyle;
        private bool m_bConstrictHeight;
        private bool m_bAlreadyUpdating;
        private int m_iControlMargin;

        private const int C_TEXT_HEIGHT = 15;

        [DllImport("user32.dll", EntryPoint = "HideCaret")]
        public static extern long HideCaret(IntPtr hwnd);

        protected override void WndProc(ref Message m)
        {
            try
            {
                base.WndProc(ref m);
                
                if (ReadOnly)
                    HideCaret(rtbTextBox.Handle);
            }
            catch { }  //can throw errors when this object is being destroyed
        }

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
            this.GotFocus += new EventHandler(TweetTextField_GotFocus);
            rtbTextBox.GotFocus += new EventHandler(rtbTextBox_GotFocus);
        }

        private void TweetTextField_GotFocus(object sender, EventArgs e)
        {
            if (ReadOnly)
                this.Focus();
            else
                rtbTextBox.Focus();
        }

        private void rtbTextBox_GotFocus(object sender, EventArgs e)
        {
            if (ReadOnly)
                this.Focus();
        }

        private void rtbTextBox_Click(object sender, EventArgs e)
        {
            if (m_stStatusText != null)
            {
                Point ptCursorLoc = rtbTextBox.PointToClient(Cursor.Position);
                int iCharIndex = rtbTextBox.GetCharIndexFromPosition(ptCursorLoc);
                StatusTextElement stElement = m_stStatusText.FindWord(iCharIndex);

                if ((stElement != null) && (TextElementClicked != null))
                    TextElementClicked(this, stElement);
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

        private void rtbTextBox_TextChanged(object sender, EventArgs e)
        {
            UpdateText(rtbTextBox.Text);

            if (TextChanged != null)
                TextChanged(this, e);
        }

        private void UpdateText(string sNewText)
        {
            if (! m_bAlreadyUpdating)
            {
                m_bAlreadyUpdating = true;

                int iStart = rtbTextBox.SelectionStart;
                m_stStatusText = StatusText.FromString(sNewText);
                rtbTextBox.Rtf = m_stStatusText.ToRTF(m_fntFont);
                rtbTextBox.SelectionStart = iStart;

                m_bAlreadyUpdating = false;
            }
        }

        public void UpdateText()
        {
            UpdateText(rtbTextBox.Text);
        }

        public void UpdateFromStatus(Status stStatus)
        {
            m_stStatusText = stStatus.StatusText;
            rtbTextBox.Rtf = m_stStatusText.ToRTF(m_fntFont);
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

        public StatusText StatusTextElements
        {
            get { return m_stStatusText; }
        }
    }
}
