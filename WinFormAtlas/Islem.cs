using DAL.Repo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormAtlas
{
    public partial class Islem : Form
    {
        public Islem()
        {
            InitializeComponent();
        }

        public int id;
        ProgramForm gercek = (ProgramForm)Application.OpenForms["ProgramForm"];
        private void button1_Click(object sender, EventArgs e)
        {
            bool souc= RotaRepo.Ekle(id);
            if(souc != true)
            {
                MessageBox.Show("Genel sistem Hatası \nSistem Çökmüştür.", "Ölümcül Hata", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Application.Exit();
            }
            else
            {
                this.Close();
                gercek.Enabled = true;
                gercek.Opacity = 100;
                gercek.ProgramForm_Load(sender, e);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = label1.Text.Substring(1) + label1.Text.Substring(0, 1);
        }

        private void Islem_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
            gercek.Enabled = true;
            gercek.Opacity = 100;
            gercek.ProgramForm_Load(sender, e);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bool souc = MusteriRepo.Sil(id);
            if (souc != true)
            {
                MessageBox.Show("Genel sistem Hatası \nSistem Çökmüştür.", "Ölümcül Hata", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Application.Exit();
            }
            else
            {
                this.Close();
                gercek.Enabled = true;
                gercek.Opacity = 100;
                gercek.ProgramForm_Load(sender, e);
            }
        }
    }
}
