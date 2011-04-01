using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Twitter.API.Json
{
    public class JsonTokenizer
    {
        private static object c_tokenizer_lock = typeof(JsonTokenizer);
        private static JsonTokenizer c_tokenizer = null;
        private StreamReader m_srStream;
        private FSA m_fsaAutomaton;
        private bool m_bReachedEOF;

        //private constructor
        private JsonTokenizer()
        {
            m_fsaAutomaton = FSA.FromStream(ResourceManager.GetManager().GetResourceStream("LexerTable.csv"));
        }

        public static JsonTokenizer GetTokenizer()
        {
            lock (c_tokenizer_lock)
            {
                if (c_tokenizer == null)
                    c_tokenizer = new JsonTokenizer();
            }

            return c_tokenizer;
        }

        public void InitWithStream(StreamReader srStream)
        {
            m_srStream = srStream;
            m_bReachedEOF = false;
            m_fsaAutomaton.Reset();
        }

        public bool AtEnd
        {
            get { return m_bReachedEOF; }
        }

        public JsonToken GetNextToken()
        {
            StringBuilder sbCurTokenString = new StringBuilder("");         //current string accumulator
            char cCurChar = '0';                                            //the current character read from the source file
            string sAppendChar = "";                                        //used to eliminate unecessary white space
            JsonToken tkFinal;
            char cPrevChar = (char)0;

            while (true)
            {
                //if we've reached the end of the source file, return an EOF token
                if (m_srStream.EndOfStream)
                {
                    if (m_bReachedEOF)
                    {
                        tkFinal = new JsonToken("End of Stream", JsonToken.TokenType.EOS);
                        break;
                    }
                    else
                    {
                        m_fsaAutomaton.FeedEOF();
                        m_bReachedEOF = true;
                    }
                }
                else
                {
                    //get next character from source file, feed it to the FSA
                    cCurChar = (char)m_srStream.Peek();
                    m_fsaAutomaton.Feed(cCurChar);
                    sAppendChar = cCurChar.ToString();
                }

                if (sAppendChar.Length > 0)
                    cPrevChar = sAppendChar[0];

                //if an accepting state has been reached, exit the loop
                if (m_fsaAutomaton.Accepted)
                {
                    //check to see if this state requires the source file to be backed up.
                    //Backups occur when a token can only be recognized by identifying the
                    //character that follows it.
                    if (m_fsaAutomaton.BackUp)
                        m_fsaAutomaton.Feed(cPrevChar);  //feed the backup char to the FSA
                    else
                    {
                        //append the last character and feed it to the FSA again.
                        //It is not logically clear why the FSA must be fed the current character
                        //a second time.  If it is not, the tokenizer does not lex correctly.
                        //It seems that the second feeding returns the tokenizer immediately to
                        //state 1, the starting state, which is desirable.
                        sbCurTokenString.Append(sAppendChar);
                        m_fsaAutomaton.Feed(cCurChar);
                        m_srStream.Read(); //actually consume the character (no backup was required)
                    }

                    tkFinal = new JsonToken(JsonUtilities.Unescape(QuoteTrim(sbCurTokenString.ToString())));
                    break;
                }
                else
                {
                    sbCurTokenString.Append(sAppendChar);
                    m_srStream.Read(); //actually consume the character
                }
            }  //while

            return tkFinal;
        }

        private string QuoteTrim(string sToTrim)
        {
            if (sToTrim.Length >= 2)
            {
                char cFirst = sToTrim[0];
                char cLast = sToTrim[sToTrim.Length - 1];

                if (((cFirst == '\"') || (cFirst == '\'')) && (cFirst == cLast))
                    return sToTrim.Substring(1, sToTrim.Length - 2);
            }

            return sToTrim;
        }
    }
}
