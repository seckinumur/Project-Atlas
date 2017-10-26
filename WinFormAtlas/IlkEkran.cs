using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using GMap.NET.MapProviders;
using System.Globalization;
using DAL.Repo;
using DAL.VM;

namespace WinFormAtlas
{
    public partial class IlkEkran : Form
    {
        public IlkEkran()
        {
            InitializeComponent();
        }

        ProgramForm kontrolformu = (ProgramForm)Application.OpenForms["ProgramForm"];
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
                gMapControl1.Zoom = gMapControl1.Zoom + 1;
                gMapControl1.Zoom = gMapControl1.Zoom - 1;
                TextMesafe.Text = Math.Round(yon.Distance, 2).ToString(CultureInfo.InvariantCulture);
                gMapControl1.Refresh();
            }
            catch
            {
                cizgi.Routes.Remove(yon);
                gMapControl1.Refresh();
            }
        }

        private void Temizle() //Temizle
        {
            TextMusteriAdi.Clear();
            TextAdres.Clear();
            TextMesafe.Clear();
            TextTelefon.Clear();
            TextEMail.Clear();
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

        private void IlkEkran_Load(object sender, EventArgs e)
        {
            HaritaBaslangic();
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

        private void ButtonEkle_Click(object sender, EventArgs e)
        {
            if (TextMusteriAdi.Text != "" && TextAdres.Text != "")
            {
                bool sonuc = MusteriRepo.Ekle(new VMMusteri
                {
                    Adres = TextAdres.Text,
                    EMail = TextEMail.Text,
                    Isim = TextMusteriAdi.Text,
                    lat = double.Parse(TextLat.Text, CultureInfo.InvariantCulture),
                    lon = double.Parse(textLon.Text,CultureInfo.InvariantCulture),
                    Mesafe = double.Parse(TextMesafe.Text, CultureInfo.InvariantCulture),
                    Telefon = TextTelefon.Text
                });
                if (sonuc == true)
                {
                    MessageBox.Show("Müşteri Başarıyla Eklendi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    kontrolformu.Enabled = true;
                    kontrolformu.Opacity = 100;
                    kontrolformu.ProgramForm_Load(sender, e);
                    this.Close();
                }
                else
                {
                    DialogResult soru = new DialogResult();
                    soru = MessageBox.Show("Bu Adrese Kayıtlı Müşteri Var! Üzerine Yazılsın mı!", "Bilgi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (soru == DialogResult.Yes)
                    {
                        bool sonuc1 = MusteriRepo.ZorlaEkle(new VMMusteri
                        {
                            Adres = TextAdres.Text,
                            EMail = TextEMail.Text,
                            Isim = TextMusteriAdi.Text,
                            lat = double.Parse(TextLat.Text, CultureInfo.InvariantCulture),
                            lon = double.Parse(textLon.Text, CultureInfo.InvariantCulture),
                            Mesafe = int.Parse(TextMesafe.Text, CultureInfo.InvariantCulture),
                            Telefon = TextTelefon.Text
                        });
                        if (sonuc1 == true)
                        {
                            MessageBox.Show("Müşteri Başarıyla Güncellendi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            kontrolformu.Enabled = true;
                            kontrolformu.Opacity = 100;
                            kontrolformu.ProgramForm_Load(sender, e);
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Veri Tabanına Müşteri Zorla Yazma Hatası!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("İşlem Kullanıcı Tarafından İptal Edildi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            else
            {
                MessageBox.Show("Müşteri Bilgilerini Eksiksiz Doldurun!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e) //key down
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

        private void textBox1_MouseLeave(object sender, EventArgs e) //mouse leave
        {
            if (textBox1.Text == "")
            {
                textBox1.Text = "Bir Yer Arayın...";
            }
        }

        private void textBox1_MouseClick(object sender, MouseEventArgs e) // mouse clic
        {
            if (textBox1.Text == "Bir Yer Arayın...")
            {
                textBox1.Clear();
            }
        }

        private void IlkEkran_FormClosing(object sender, FormClosingEventArgs e)
        {
            kontrolformu.Enabled = true;
            kontrolformu.Opacity = 100;
        }
    }
}