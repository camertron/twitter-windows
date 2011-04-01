using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Twitter.API.Json
{
    public class JsonDocument
    {
        private JsonObject m_jnObject;

        public JsonObject Root
        {
            get { return m_jnObject; }
        }

        public JsonDocument(JsonObject joInitObj)
        {
            m_jnObject = joInitObj;
        }

        public JsonDocument(JsonNode jnInitNode)
        {
            m_jnObject = new JsonObject(jnInitNode);
        }

        public JsonDocument(List<JsonObject> ljoInitList)
        {
            m_jnObject = new JsonObject(ljoInitList);
        }
    }
}
