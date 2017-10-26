using DAL.Repo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormAtlas
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private async void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Task<bool> name = new Task<bool>(login);
                name.Start();
                butonlar();
                textBox1.Enabled = false;
                if (await name == true)
                {
                    ProgramForm ac = new ProgramForm();
                    ac.Show();
                    this.Close();
                }
                else
                {
                    gizle1.Visible = false;
                    gizle2.Visible = false;
                    textBox1.Enabled = true;
                    MessageBox.Show("Şifre Yanlış!", "Kullanıcı Denetimi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBox1.Clear();
                }
            }
            if (e.KeyCode == Keys.Escape)
            {
                textBox1.Clear();
            }
        }
        private bool login()
        {
            bool sonuc = CoreRepo.logon(textBox1.Text);
            Thread.Sleep(4000);
            return sonuc;
        }
        private void butonlar()
        {
            gizle1.Visible = true;
            gizle2.Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.seckinumur.com");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            
        }
    }
}
