using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MonthlyCalculator
{
    public partial class Login : Form
    {
        private MainForm MF;
        private AdminForm AF;
        public Login()
        {
            InitializeComponent();
            FormInitializiation();
        }
        public void FormInitializiation()
        {
            Users.Items.Clear();
            Users.Items.Add("المدير");
            Users.Items.Add("مستخدم");
        }
        private void Quit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Users.SelectedItem.ToString() == "مستخدم")
            {
                Password.Enabled = false;
            }
            else if (Users.SelectedItem.ToString() == "المدير")
            {
                Password.Enabled = true;
            }
        }

        private void Enter_Click(object sender, EventArgs e)
        {
            if(Users.SelectedItem.ToString() == "المدير")
            {
                Password.Text = "";
                AF = new AdminForm();
                AF.SetLoginForm(this);
                AF.Show();
                this.Hide();
            }
            else if (Users.SelectedItem.ToString() == "مستخدم")
            {
                MF = new MainForm();
                MF.SetLoginForm(this);
                MF.Show();
                this.Hide();
            }
        }
    }
}
