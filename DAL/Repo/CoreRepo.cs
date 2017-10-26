using DAL.VM;
using Entitiy.Context;
using Entitiy.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repo
{
    public class CoreRepo
    {
        public static bool Kontrol()
        {
            using (PADB db = new PADB())
            {
                try
                {
                    return db.Core.Any(p => p.YeniKurulum == true);
                }
                catch
                {
                    return true;
                }
            }
        }
        public static double BaslangicNoktasiLAT()
        {
            using (PADB db = new PADB())
            {
                try
                {
                    return db.Core.FirstOrDefault(p => p.CoreID == 1).lat;
                }
                catch
                {
                    return 0;
                }
            }
        }
        public static double BaslangicNoktasiLON()
        {
            using (PADB db = new PADB())
            {
                try
                {
                    return db.Core.FirstOrDefault(p => p.CoreID == 1).lon;
                }
                catch
                {
                    return 0;
                }
            }
        }
        public static bool Ekle(VMCore Data)
        {
            using (PADB db = new PADB())
            {
                try
                {
                    var bul = db.Core.FirstOrDefault(p => p.CoreID == 1);
                    bul.Adres = Data.Adres;
                    bul.FirmaAdi = Data.FirmaAdi;
                    bul.lat = Data.lat;
                    bul.lon = Data.lon;
                    bul.Sifre = Data.Sifre;
                    bul.YeniKurulum = false;
                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
        public static string OturumName()
        {
            using (PADB db = new PADB())
            {
                try
                {
                    return db.Core.FirstOrDefault(p => p.CoreID == 1).FirmaAdi;
                }
                catch
                {
                    return "Database Bağlantısı Sağlanamadı!";
                }
            }
        }
        public static bool logon(string data)
        {
            using (PADB db = new PADB())
            {
                try
                {
                    return db.Core.Any(p => p.Sifre==data && p.CoreID==1);
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}

