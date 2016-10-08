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
    public partial class SignInForm : Form
    {
        private SIMClient.Main client;
        public SignInForm(SIMClient.Main client)
        {
            this.client = client;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.client.SignIn(usernameTextBox.Text, passwordTextBox.Text))
            {
                
            }
            else
            {
                MessageBox.Show("INCORRECT PASSWORD");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.client.Server.Register(usernameTextBox.Text, passwordTextBox.Text))
            {
                MessageBox.Show("REGISTRATION SUCCESSFUL");
            }
            else
            {
                MessageBox.Show("REGISTRATION FAILED");
            }
        }
    }
}
