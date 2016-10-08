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
    public partial class ConnectForm : Form
    {
        public ConnectForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            failiureLabel.Visible = false;
            connectingLabel.Visible = true;
            System.Net.IPAddress address;
            try
            {
                address = System.Net.IPAddress.Parse(addressTextBox.Text);
                var client = new SIMClient.Main(address);
                var f = new SignInForm(client);
                this.Hide();
                f.Closed += (s, args) => this.Close();
                f.Show();
            }
            catch (Exception ex) when (ex is FormatException || ex is SIMClient.Main.InitializationFailiureException)
            {
                connectingLabel.Visible = false;
                failiureLabel.Visible = true;
            }
        }

        private void enterKeyPress(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            addressTextBox.KeyDown -= new KeyEventHandler(enterKeyPress);
            if (e.KeyCode == Keys.Enter)
            {
                this.button1_Click(this, e);
            }
        }

        private void addressTextBox_TextChanged(object sender, EventArgs e)
        {
            this.addressTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(enterKeyPress);
        }
    }
}
