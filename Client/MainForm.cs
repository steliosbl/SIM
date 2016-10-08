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
    public partial class MainForm : Form
    {
        private SIMClient.Main client;
        public MainForm(SIMClient.Main client)
        {
            this.client = client;
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            label2.Text = "Welcome " + this.client.CurrentUser.Nickname;
            label2.Update();

            foreach (var profile in this.client.Server.Profiles)
            {
                listView1.Items.Add(new ListViewItem(profile.Nickname));
            }

            foreach (var thread in this.client.Database.GetAllThreads())
            {
                if (thread.IsGroup)
                {
                    listView2.Items.Add(new ListViewItem(thread.GroupName));
                }
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {

        }

        private void signOutButton_Click(object sender, EventArgs e)
        {
            this.client.SignOut();
            var f = new SignInForm(this.client);
            this.Close();
            f.Show();
        }
    }
}
