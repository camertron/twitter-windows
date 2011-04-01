using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Twitter.API.Json;

namespace Twitter.API.Basic
{
    public class User : ScaffoldObject<JsonNode>
    {
        private JsonNode m_jnNode;

        public User(JsonNode jnNode) : base(jnNode)
        {
            m_jnNode = jnNode;
        }
    }
}
