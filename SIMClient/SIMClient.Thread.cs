namespace SIMClient
{
    using System;
    using System.Collections.Generic;

    public class Thread
    {
        public Thread(int id) : this(id, null)
        {
        }

        public Thread(int id, List<int> participants) : this(id, participants, null)
        {
        }

        public Thread(int id, List<int> participants, List<SIMCommon.Message> messages) : this(id, participants, messages, false)
        {
        }

        public Thread(int id, List<int> participants, List<SIMCommon.Message> messages, bool hasUnread)
        {
            this.ID = id;
            this.Participants = participants;
            this.Messages = messages;
            this.HasUnread = hasUnread;
        }

        public int ID { get; private set; }

        public List<int> Participants { get; private set; }

        public List<SIMCommon.Message> Messages { get; private set; }

        public bool HasUnread { get; private set; }
    }
}
