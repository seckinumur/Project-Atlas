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
    public partial class StartForm : Form
    {
        public StartForm()
        {
            InitializeComponent();
        }

        public async void StartForm_Load(object sender, EventArgs e)
        {
            Task<bool> Sonuc = new Task<bool>(Oturum);
            Task<string> name = new Task<string>(Kullanici);
            Sonuc.Start();
            name.Start();
            if (await Sonuc == true)
            {
                this.Hide();
                Ayarlar ac = new Ayarlar();
                ac.Show();
            }
            else
            {
                this.Hide();
                LoginForm ac = new LoginForm();
                if (await name != "Database Bağlantısı Sağlanamadı!")
                {
                    ac.lblName.Text = name.Result;
                    ac.Show();
                }
                else
                {
                    MessageBox.Show(name.Result, "Kritik Sistem Hatası!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private static bool Oturum()
        {
            var data = CoreRepo.Kontrol();
            Thread.Sleep(4000);
            return data;
        }
        private static string Kullanici()
        {
            return CoreRepo.OturumName();
        }
    }
}
