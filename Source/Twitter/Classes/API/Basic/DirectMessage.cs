using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Twitter.Json;

namespace Twitter.API.Basic
{
    public class DirectMessage : ScaffoldObject<JsonNode>
    {
        public DirectMessage(JsonNode jnNode) : base(jnNode) { }
    }
}
