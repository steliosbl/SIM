namespace SIMCommon
{
    using System;
    using Newtonsoft.Json;

    public class Message
    {
        public Message(int senderID, int recipientID, string text)
        {
            this.SenderID = senderID;
            this.RecipientID = recipientID;
            this.Text = text;
            this.Timestamp = DateTime.Now;
        }

        [JsonConstructor]
        public Message(int senderID, int recipientID, string text, DateTime timestamp)
        {
            this.SenderID = senderID;
            this.RecipientID = recipientID;
            this.Text = text;
            this.Timestamp = timestamp;
        }

        public int SenderID { get; private set; }

        public int RecipientID { get; private set; }

        public string Text { get; private set; }

        public DateTime Timestamp { get; private set; }
    }
}
