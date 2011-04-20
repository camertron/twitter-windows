using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Twitter.Json;

namespace Twitter.API.Basic
{
    public class UserTimeline : ScaffoldObject<List<JsonObject>>
    {
        private List<Status> m_lsStatuses = null;
        private List<JsonObject> m_ljoStatuses;

        public UserTimeline(List<JsonObject> ljoStatuses) : base(ljoStatuses)
        {
            m_ljoStatuses = ljoStatuses;
        }

        public List<Status> Statuses
        {
            get
            {
                if (m_lsStatuses == null)
                {
                    m_lsStatuses = new List<Status>();

                    for (int i = 0; i < m_ljoStatuses.Count; i++)
                        m_lsStatuses.Add(new Status(m_ljoStatuses[i].ToNode()));
                }

                return m_lsStatuses;
            }
        }
    }
}
