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
using DAL.Repo;
using System.Globalization;
using GMap.NET.WindowsForms.Markers;
using GMap.NET.MapProviders;
using DAL.VM;

namespace WinFormAtlas
{
    public partial class Ayarlar : Form
    {
        public Ayarlar()
        {
            InitializeComponent();
        }
        StartForm bu = (StartForm)Application.OpenForms["StartForm"];
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

        private void Navigasyon(double LA, double LO)
        {
            try
            {
                cizgi.Routes.Remove(yon);
                gMapControl1.Refresh();
                pointA = new PointLatLng(CoreRepo.BaslangicNoktasiLAT(),CoreRepo.BaslangicNoktasiLON());
                pointB = new PointLatLng(LA, LO);
                var yonlendirme = GMapProviders.GoogleMap.GetDirections(out navigasyon, pointA, pointB, true, false, false, false, true);
                yon = new GMapRoute(navigasyon.Route, "Rota Çizildi");
                cizgi.Routes.Add(yon);
                gMapControl1.Overlays.Add(cizgi);
                gMapControl1.Zoom = gMapControl1.Zoom + 1;
                gMapControl1.Zoom = gMapControl1.Zoom - 1;
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

        private void Ayarlar_Load(object sender, EventArgs e)
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
            if (TextMusteriAdi.Text != "" && TextAdres.Text != "" && TextTelefon.Text !="")
            {
                bool sonuc = CoreRepo.Ekle(new VMCore
                {
                    Adres = TextAdres.Text,
                    lat = double.Parse(TextLat.Text, CultureInfo.InvariantCulture),
                    lon = double.Parse(textLon.Text, CultureInfo.InvariantCulture),
                    FirmaAdi = TextMusteriAdi.Text,
                    Sifre = TextTelefon.Text
                });
                if (sonuc == true)
                {
                    MessageBox.Show("Ayarlar Başarıyla Kaydedildi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Database Hatası Lütfen Geliştiriciyle İrtibata Geçin", "Kritik Sistem Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Firma Bilgilerini Eksiksiz Doldurun!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void Ayarlar_FormClosing(object sender, FormClosingEventArgs e)
        {
            bu.Show();
            bu.StartForm_Load(sender, e);
        }
    }
}