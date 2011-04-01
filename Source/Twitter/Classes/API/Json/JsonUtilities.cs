using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Twitter.API.Json
{
    public class JsonUtilities
    {
        public static string Escape(string sToEscape)
        {
            StringBuilder sbFinal = new StringBuilder();

            for (int i = 0; i < sToEscape.Length; i++)
            {
                switch (sToEscape[i])
                {
                    case '\r':
                        sbFinal.Append("\\r"); break;
                    case '\n':
                        sbFinal.Append("\\n"); break;
                    case '\t':
                        sbFinal.Append("\\t"); break;
                    case '"':
                        sbFinal.Append("\\\""); break;
                    case '/':
                        sbFinal.Append("\\/"); break;
                    default:
                        sbFinal.Append(sToEscape[i]); break;
                }
            }

            return System.Text.RegularExpressions.Regex.Escape(sbFinal.ToString());
        }

        public static string Unescape(string sToUnescape)
        {
            string sFinal = sToUnescape.Replace("\\r", "\r");

            sFinal = sFinal.Replace("\\n", "\n");
            sFinal = sFinal.Replace("\\t", "\t");
            sFinal = sFinal.Replace("\\t", "\t");
            sFinal = sFinal.Replace("\\\"", "\"");
            sFinal = sFinal.Replace("\\/", "/");

            //for some reason, HtmlDecode() doesn't handle &apos (probably a bug)
            sFinal = System.Web.HttpUtility.HtmlDecode(sFinal).Replace("&apos;", "'");

            //convert unicode \uNNNN hex sequences to real chars
            return System.Text.RegularExpressions.Regex.Unescape(sFinal);
        }
    }
}
