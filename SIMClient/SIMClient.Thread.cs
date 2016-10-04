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

        public Thread(int id, List<int> participants, List<SIMCommon.Message> messages, bool hasUnread) : this(id, participants, messages, hasUnread, participants.Count > 2)
        {
        }

        public Thread(int id, List<int> participants, List<SIMCommon.Message> messages, bool hasUnread, bool isGroup) : this(id, participants, messages, hasUnread, isGroup, string.Empty)
        {
        }

        public Thread(int id, List<int> participants, List<SIMCommon.Message> messages, bool hasUnread, bool isGroup, string groupName)
        {
            this.ID = id;
            this.Participants = participants;
            this.Messages = messages;
            this.HasUnread = hasUnread;
            this.IsGroup = isGroup;
            this.GroupName = groupName;
        }

        public int ID { get; private set; }

        public List<int> Participants { get; private set; }

        public List<SIMCommon.Message> Messages { get; private set; }

        public bool HasUnread { get; private set; }

        public bool IsGroup { get; private set; }

        public string GroupName { get; private set; }

        public void LoadMessages(List<SIMCommon.Message> messages)
        {
            this.Messages = messages;
        }
    }
}
