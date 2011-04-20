using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Twitter.Json;

namespace Twitter.API
{
    public class ScaffoldObject<T>
    {
        private T m_tObj;

        public ScaffoldObject(T tInitObj)
        {
            m_tObj = tInitObj;
        }

        public T Object
        {
            get { return m_tObj; }
        }

        public JsonObject this[string sIndex]
        {
            get { return ((JsonNode)((object)m_tObj))[sIndex]; }
        }

        public JsonObject this[int iIndex]
        {
            get { return ((List<JsonObject>)((object)m_tObj))[iIndex]; }
        }
    }
}
