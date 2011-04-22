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
        private bool m_bIsRetweet = false;
        private bool m_bIsReply = false;
        private List<string> m_lsReplyNames;

        public Status(JsonNode jnNode) : base(jnNode)
        {
            m_jnNode = jnNode;
            m_lsReplyNames = new List<string>();

            if (m_jnNode.ContainsKey("text") && m_jnNode["text"].IsString())
            {
                m_stStatusText = StatusText.FromString(m_jnNode["text"].ToString());
                m_bIsRetweet = (m_stStatusText.Words.Count > 0) && (m_stStatusText.Words[0].Type == StatusTextElement.StatusTextElementType.ScreenName);
                m_bIsReply = (m_jnNode.ContainsKey("retweeted_status")) || (m_stStatusText.Words.Count > 0) && (m_stStatusText.Words[0].Text == "RT");

                for (int i = 0; i < m_stStatusText.Words.Count; i++)
                {
                    if (m_stStatusText.Words[i].Type == StatusTextElement.StatusTextElementType.ScreenName)
                        m_lsReplyNames.Add(m_stStatusText.Words[i].Text);
                }
            }
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
                }

                return m_stRetweetedStatus;
            }
            set { m_stRetweetedStatus = value; }
        }

        //if this status is showing up in your timeline because it was retweeted
        public bool IsRetweet
        {
            get { return m_bIsRetweet; }
        }

        public bool IsReply
        {
            get { return m_bIsReply; }
        }

        public List<string> ReplyNames
        {
            get { return m_lsReplyNames; }
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
