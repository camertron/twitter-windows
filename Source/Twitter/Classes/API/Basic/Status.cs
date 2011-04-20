using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Twitter.Json;

namespace Twitter.API.Basic
{
    public class Status : ScaffoldObject<JsonNode>
    {
        private User m_uUser = null;
        private JsonNode m_jnNode;

        public Status(JsonNode jnNode) : base(jnNode)
        {
            m_jnNode = jnNode;
        }

        public User User
        {
            get
            {
                if (m_uUser == null)
                {
                    if (m_jnNode.ContainsKey("user"))
                        m_uUser = new User(m_jnNode["user"].ToNode());
                    else
                        m_uUser = null;
                }

                return m_uUser;
            }
        }
    }
}
