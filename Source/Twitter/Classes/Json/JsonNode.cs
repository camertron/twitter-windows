using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Twitter.Json
{
    public class JsonNode
    {
        private Dictionary<string, JsonObject> m_dElements;

        public JsonNode()
        {
            m_dElements = new Dictionary<string, JsonObject>();
        }

        public JsonObject this[string sIndex]
        {
            get { return m_dElements[sIndex]; }
        }

        public void Add(string sKey, object objObject)
        {
            m_dElements.Add(sKey, new JsonObject(objObject));
        }

        public bool ContainsKey(string sKey)
        {
            return m_dElements.ContainsKey(sKey);
        }
    }

    public class JsonObject
    {
        private object m_objInternalObject;

        public JsonObject(object objInitObject)
        {
            m_objInternalObject = objInitObject;
        }

        public bool IsString()
        {
            return m_objInternalObject.GetType() == typeof(string);
        }

        public bool IsList()
        {
            return m_objInternalObject.GetType() == typeof(List<JsonObject>);
        }

        public bool IsNode()
        {
            return m_objInternalObject.GetType() == typeof(JsonNode);
        }

        public bool IsBool()
        {
            return m_objInternalObject.GetType() == typeof(Boolean);
        }

        public JsonNode ToNode()
        {
            return (JsonNode)m_objInternalObject;
        }

        public List<JsonObject> ToList()
        {
            if (m_objInternalObject.GetType() == typeof(JsonObject))
            {
                List<JsonObject> ljoList = new List<JsonObject>();
                ljoList.Add((JsonObject)m_objInternalObject);
                return ljoList;
            }
            else
                return (List<JsonObject>)m_objInternalObject;
        }

        public override string ToString()
        {
            return (string)m_objInternalObject;
        }

        public bool ToBool()
        {
            bool bResult;

            if (Boolean.TryParse(m_objInternalObject.ToString(), out bResult))
                return bResult;
            else
                return false;
        }
    }
}
