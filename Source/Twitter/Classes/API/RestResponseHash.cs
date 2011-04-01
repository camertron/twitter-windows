using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Hammock;

namespace Twitter.API
{
    public class RestResponseHash : Dictionary<string, string>
    {
        public RestResponseHash(RestResponse rrToParse)
        {
            string[] saVars = rrToParse.Content.Split(new char[1] { '&' });
            char[] caKeyValSplitters = new char[1] { '=' };
            string[] saKeyVal;

            for (int i = 0; i < saVars.Length; i ++)
            {
                saKeyVal = saVars[i].Split(caKeyValSplitters);

                if (saKeyVal.Length == 2)
                    this[saKeyVal[0]] = saKeyVal[1];
            }
        }

        public RestResponseHash() { }

        public override string ToString()
        {
            Dictionary<string, string>.Enumerator dssEnum = this.GetEnumerator();
            StringBuilder sbFinal = new StringBuilder();
            bool bFirst = true;

            while (dssEnum.MoveNext())
            {
                if (bFirst)
                    bFirst = false;
                else
                    sbFinal.Append("&");

                sbFinal.Append(dssEnum.Current.Key + "=" + dssEnum.Current.Value);
            }

            return sbFinal.ToString();
        }
    }
}
