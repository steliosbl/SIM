using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class ChatForm : Form
    {
        private SIMClient.Main client;
        private SIMCommon.UserProfile profile;
        private SIMClient.Thread thread;
        private int offsetCounter;

        public ChatForm(SIMClient.Main client, SIMCommon.UserProfile profile, SIMClient.Thread thread)
        {
            this.client = client;
            this.profile = profile;
            this.thread = thread;
            this.offsetCounter = 0;
            InitializeComponent();
        }

        private void messageBox_TextChanged(object sender, EventArgs e)
        {
            this.messageBox.KeyDown += new System.Windows.Forms.KeyEventHandler(enterKeyPress);
        }

        private void enterKeyPress(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            messageBox.KeyDown -= new KeyEventHandler(enterKeyPress);
            if (e.KeyCode == Keys.Enter)
            {
                this.sendButton_Click(this, e);
            }
        }

        private void sendButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(messageBox.Text))
            {
                if (this.client.Send(messageBox.Text, this.profile.ID))
                {

                }
                else
                {
                    MessageBox.Show("UNABLE TO SEND");
                }
            }
        }

        private void ChatForm_Load(object sender, EventArgs e)
        {
            this.thread.LoadMessages(this.client.Database.ReadThread(this.thread.ID));
            this.LoadMessages();
        }

        private void LoadMessages()
        {
            foreach (var message in this.thread.Messages)
            {
                threadBox.Text += Environment.NewLine;
                try
                {
                    threadBox.Text += this.client.Server.Profiles.FindAll(profile => profile.ID == message.SenderID)[0].Nickname + ": ";
                }
                catch (IndexOutOfRangeException)
                {
                    threadBox.Text += "UNKNOWN: ";
                }

                threadBox.Text += message.Text;
            }

            threadBox.SelectionStart = threadBox.Text.Length;
            threadBox.ScrollToCaret();
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            this.offsetCounter += SIMCommon.Constants.SIMClientThreadOffset;
            this.thread.LoadMessages(this.client.Database.ReadThread(this.thread.ID, this.offsetCounter));
            this.LoadMessages();
        }
    }
}
