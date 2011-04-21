using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Twitter.Json;

namespace Twitter.API.Basic
{
    public class Status : ScaffoldObject<JsonNode>
    {
        private Status m_stRetweetedStatus = null;
        private User m_uUser = null;
        private JsonNode m_jnNode;
        private StatusText m_stStatusText;

        public Status(JsonNode jnNode) : base(jnNode)
        {
            m_jnNode = jnNode;

            if (m_jnNode.ContainsKey("text") && m_jnNode["text"].IsString())
                m_stStatusText = StatusText.FromString(m_jnNode["text"].ToString());
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

        public Status RetweetedStatus
        {
            get
            {
                if (m_stRetweetedStatus == null)
                {
                    if (m_jnNode.ContainsKey("retweeted_status"))
                        m_stRetweetedStatus = new Status(m_jnNode["retweeted_status"].ToNode());
                    else
                        m_stRetweetedStatus = null;
                }

                return m_stRetweetedStatus;
            }
        }

        //if this status is showing up in your timeline because it was retweeted
        public bool WasRetweeted
        {
            get { return m_jnNode.ContainsKey("retweeted_status"); }
        }

        public StatusText StatusText
        {
            get { return m_stStatusText; }
        }
    }

    public class StatusList : List<Status>
    {
        private Dictionary<string, Status> m_dStatusesById;

        public StatusList() : base()
        {
            m_dStatusesById = new Dictionary<string, Status>();
        }

        public new void Add(Status stNew)
        {
            m_dStatusesById.Add(stNew["id"].ToString(), stNew);
            base.Add(stNew);
        }

        public new void Remove(Status stToRemove)
        {
            m_dStatusesById.Remove(stToRemove["id"].ToString());
            base.Remove(stToRemove);
        }

        public new void RemoveAt(int iIndex)
        {
            Status stToRemove = base[iIndex];
            m_dStatusesById.Remove(stToRemove["id"].ToString());
            base.RemoveAt(iIndex);
        }

        public Status this[string sId]
        {
            get { return m_dStatusesById[sId]; }
            set { m_dStatusesById[sId] = value; }
        }

        public new bool Contains(Status stLookFor)
        {
            return m_dStatusesById.ContainsKey(stLookFor["id"].ToString());
        }
    }
}
