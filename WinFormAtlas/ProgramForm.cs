using DAL.Repo;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormAtlas
{
    public partial class ProgramForm : Form
    {
        public ProgramForm()
        {
            InitializeComponent();
        }
        bool formkontrol;
        bool ilkbaslangic;
        StartForm buu = (StartForm)Application.OpenForms["StartForm"];
        PointLatLng pointA;
        PointLatLng pointB;
        GMapOverlay markers = new GMapOverlay("markers");
        GMapOverlay cizgi = new GMapOverlay("cizgi");
        GMapRoute yon;
        GDirections navigasyon;
        GMapMarker marker = new GMarkerGoogle(new PointLatLng(CoreRepo.BaslangicNoktasiLAT(), CoreRepo.BaslangicNoktasiLON()), GMarkerGoogleType.blue_pushpin);

        private void HaritaBaslangic() //Başlangıç Noktası Harita Yükleme
        {
            gMapControl1.MapProvider = GoogleMapProvider.Instance;
            GMaps.Instance.Mode = AccessMode.ServerOnly;
            gMapControl1.ShowCenter = false;
            gMapControl1.Position = new PointLatLng(CoreRepo.BaslangicNoktasiLAT(), CoreRepo.BaslangicNoktasiLON());
        }

        private void Arama(string quescins)
        {
            gMapControl1.SetPositionByKeywords(quescins);
            Isaretleyici(gMapControl1.Position.Lat, gMapControl1.Position.Lng);
            gMapControl1.Refresh();
        }

        private void Navigasyon(double LA, double LO)
        {
            try
            {
                cizgi.Routes.Remove(yon);
                gMapControl1.Refresh();
                pointA = new PointLatLng(CoreRepo.BaslangicNoktasiLAT(), CoreRepo.BaslangicNoktasiLON());
                pointB = new PointLatLng(LA, LO);
                var yonlendirme = GMapProviders.GoogleMap.GetDirections(out navigasyon, pointA, pointB, true, false, false, false, true);
                yon = new GMapRoute(navigasyon.Route, "Rota Çizildi");
                cizgi.Routes.Add(yon);
                gMapControl1.Overlays.Add(cizgi);
                gMapControl1.Zoom = gMapControl1.Zoom = 15;
                gMapControl1.Refresh();
                TextMesafe.Text = Math.Round(yon.Distance, 2).ToString();
                gMapControl1.Refresh();
            }
            catch
            {
                cizgi.Routes.Remove(yon);
                gMapControl1.Refresh();
            }
        }

        private void Isaretleyici(double la, double lo)
        {
            Placemark? p = null;
            GeoCoderStatusCode status;
            var pos = GMapProviders.GoogleMap.GetPlacemark(new PointLatLng(la, lo), out status);
            if (status == GeoCoderStatusCode.G_GEO_SUCCESS && pos != null)
            {
                p = pos;
            }

            if (p != null)
            {
                TextLat.Text = la.ToString(CultureInfo.InvariantCulture);
                textLon.Text = lo.ToString(CultureInfo.InvariantCulture);
                TextAdres.Text = pos.Value.Address.ToString();
            }
            else
            {
                TextLat.Text = la.ToString(CultureInfo.InvariantCulture);
                textLon.Text = lo.ToString(CultureInfo.InvariantCulture);
                TextAdres.Text = la.ToString() + lo.ToString();
            }

            marker.Position = new PointLatLng(la, lo);
            marker.ToolTipMode = MarkerTooltipMode.Always;
            marker.ToolTipText = string.Format("Kordinat: \n Latitude:{0} \n Longitude:{1}", la.ToString(), lo.ToString());
            markers.Markers.Add(marker);
            gMapControl1.Overlays.Add(markers);
            Navigasyon(la, lo);
        }

        public void ProgramForm_Load(object sender, EventArgs e)
        {
            gridControl1.DataSource = MusteriRepo.MusterilerListe();
            gridControl2.DataSource = RotaRepo.NavListe();
            HaritaBaslangic();
        }
        
        private void ProgramForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (formkontrol)
            {
                Ayarlar ac = new Ayarlar();
                ac.Show();
            }
            else
            {
                buu.Show();
                buu.StartForm_Load(sender, e);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult Uyari = new DialogResult();
            Uyari = MessageBox.Show("Ayarlara Girmek Programın \nYeniden Başlamasına Neden Olur \nDevam Etmek İstiyormusunuz?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (Uyari == DialogResult.Yes)
            {
                formkontrol = true;
                this.Close();
            }
        }

        private void gMapControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            gMapControl1.Overlays.Remove(markers);
            gMapControl1.Refresh();
            double a = 0, b = 0;
            a = gMapControl1.FromLocalToLatLng(e.X, e.Y).Lat;
            b = gMapControl1.FromLocalToLatLng(e.X, e.Y).Lng;
            Isaretleyici(a, b);
        }

        private void textBox1_MouseLeave(object sender, EventArgs e) //şekıl şukul
        {
            if (textBox1.Text == "")
            {
                textBox1.Text = "Bir Yer Arayın...";
            }
        }

        private void textBox1_MouseClick(object sender, MouseEventArgs e) //şekil şukul
        {
            if (textBox1.Text == "Bir Yer Arayın...")
            {
                textBox1.Clear();
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Arama(textBox1.Text);
            }
            if (e.KeyCode == Keys.Escape)
            {
                textBox1.Clear();
                HaritaBaslangic();
                cizgi.Routes.Remove(yon);
                gMapControl1.Overlays.Remove(markers);
                gMapControl1.Refresh();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            this.Opacity = 70;
            IlkEkran ac = new IlkEkran();
            ac.Show();
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            int ID = Convert.ToInt32(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "MusterilerID"));
            string isim = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Isim").ToString();
            Islem ac = new Islem();
            ac.id = ID;
            ac.label1.Text = "Müşteri Adı: "+isim + " <<>> ";
            ac.Show();
            this.Enabled = false;
            this.Opacity = 70;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            bool sonu= RotaRepo.ListeyiSil();
            if (sonu)
            {
                ProgramForm_Load(sender, e);
            }
            else
            {
                MessageBox.Show("Genel sistem Hatası \nSistem Çökmüştür.", "Ölümcül Hata", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Application.Exit();
            }
        }

        private void button2_Click(object sender, EventArgs e) //excel aktarımı
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Microsoft Excel Engine|*.xlxs";
            save.OverwritePrompt = true;

            if (save.ShowDialog() == DialogResult.OK)
            {
                gridView2.ExportToXlsx(save.FileName);
            }
        }

        private void gridView2_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                int ID = Convert.ToInt32(gridView2.GetRowCellValue(gridView2.FocusedRowHandle, "MusterilerID"));
                string isim = gridView2.GetRowCellValue(gridView2.FocusedRowHandle, "Isim").ToString();
                DialogResult Uyari = new DialogResult();
                Uyari = MessageBox.Show(isim + " Bu Müşteri Listeden Silinecek!\nDevam Etmek İstiyormusunuz?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (Uyari == DialogResult.Yes)
                {
                    bool iss = RotaRepo.Sil(ID);
                    if (iss)
                    {
                        ProgramForm_Load(sender, e);
                    }
                    else
                    {
                        MessageBox.Show("Genel sistem Hatası \nSistem Çökmüştür.", "Ölümcül Hata", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        Application.Exit();
                    }
                }
            }
            catch
            {
                MessageBox.Show("Liste Zaten Boş!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void gridView2_Click(object sender, EventArgs e)
        {
            double la = Convert.ToDouble(gridView2.GetRowCellValue(gridView2.FocusedRowHandle, "lat"), CultureInfo.InvariantCulture);
            double lo = Convert.ToDouble(gridView2.GetRowCellValue(gridView2.FocusedRowHandle, "lon"), CultureInfo.InvariantCulture);
            Isaretleyici(la, lo);
        }

        private void gridView1_Click(object sender, EventArgs e)
        {
            double la = Convert.ToDouble(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "lat"), CultureInfo.InvariantCulture);
            double lo = Convert.ToDouble(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "lon"), CultureInfo.InvariantCulture);
            Isaretleyici(la, lo);
        }
    }
}