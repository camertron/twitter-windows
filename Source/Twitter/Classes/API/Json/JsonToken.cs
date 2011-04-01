using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Twitter.API.Json
{
    public class JsonToken
    {
        public enum TokenType
        {
            Empty = 0,
            OpenCurly = 1,
            CloseCurly = 2,
            Comma = 3,
            Colon = 4,
            String = 5,
            Quote = 6,
            Backslash = 7,
            OpenBracket = 8,
            CloseBracket = 9,
            EOS = 10
        }

        private string m_sText;
        private TokenType m_jtType;
        public static char[] TrimChars = new char[4] { '\r', '\n', '\t', ' ' };

        public JsonToken(string sInitText)
        {
            m_sText = sInitText;
            m_jtType = JsonToken.StringToTokenType(sInitText);
        }

        public JsonToken(string sInitText, JsonToken.TokenType jtInitType)
        {
            m_jtType = jtInitType;
            m_sText = sInitText;
        }

        public static JsonToken.TokenType StringToTokenType(string sToConvert)
        {
            switch (sToConvert)
            {
                case "{": return TokenType.OpenCurly;
                case "}": return TokenType.CloseCurly;
                case ",": return TokenType.Comma;
                case ":": return TokenType.Colon;
                case "\"": return TokenType.Quote;
                case "\\": return TokenType.Backslash;
                case "[": return TokenType.OpenBracket;
                case "]": return TokenType.CloseBracket;
                default:
                    return TokenType.String;
            }
        }

        public override string ToString()
        {
            return m_jtType.ToString() + " (" + m_sText + ")";
        }

        public string Text
        {
            get { return m_sText; }
            set { m_sText = value; }
        }

        public TokenType Type
        {
            get { return m_jtType; }
        }
    }
}
