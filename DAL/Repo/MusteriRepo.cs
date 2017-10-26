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
    public class MusteriRepo
    {
        public static bool Ekle(VMMusteri Data)
        {
            try
            {
                using (PADB db = new PADB())
                {
                    bool kotrol = db.Musteriler.Any(p => p.Adres == Data.Adres);
                    if (kotrol == true)
                    {
                        return false;
                    }
                    else
                    {
                        db.Musteriler.Add(new Musteriler
                        {
                            Adres = Data.Adres.ToUpper().Trim(),
                            EMail = Data.EMail.ToLower().Trim(),
                            Isim = Data.Isim.ToUpper().Trim(),
                            lat = Data.lat,
                            lon = Data.lon,
                            Mesafe = Data.Mesafe,
                            Telefon = Data.Telefon.ToUpper().Trim()
                        });
                        db.SaveChanges();
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }
        public static bool ZorlaEkle(VMMusteri Data)
        {
            try
            {
                using (PADB db = new PADB())
                {
                    var bul = db.Musteriler.FirstOrDefault(p => p.MusterilerID == Data.MusterilerID);
                    bul.Adres = Data.Adres.ToUpper().Trim();
                    bul.EMail = Data.EMail.ToLower().Trim();
                    bul.Isim = Data.Isim.ToUpper().Trim();
                    bul.lat = Data.lat;
                    bul.lon = Data.lon;
                    bul.Mesafe = Data.Mesafe;
                    bul.Telefon = Data.Telefon.ToUpper().Trim();
                    db.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        public static bool Guncelle(VMMusteri Data)
        {
            try
            {
                using (PADB db = new PADB())
                {
                    var bul = db.Musteriler.FirstOrDefault(p => p.MusterilerID == Data.MusterilerID);
                    bul.Adres = Data.Adres.ToUpper().Trim();
                    bul.EMail = Data.EMail.ToLower().Trim();
                    bul.Isim = Data.Isim.ToUpper().Trim();
                    bul.lat = Data.lat;
                    bul.lon = Data.lon;
                    bul.Mesafe = Data.Mesafe;
                    bul.Telefon = Data.Telefon.ToUpper().Trim();
                    db.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        public static bool Sil(int id)
        {
            try
            {
                using (PADB db = new PADB())
                {
                    var bul = db.Musteriler.FirstOrDefault(p => p.MusterilerID == id);
                    var bul2 = db.Nav.FirstOrDefault(p => p.MusterilerID == id);
                    db.Nav.Remove(bul2);
                    db.Musteriler.Remove(bul);
                    db.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        public static List<VMMusteri> MusterilerListe()
        {
            using (PADB db = new PADB())
            {
                return db.Musteriler.Select(p => new VMMusteri
                {
                    Adres = p.Adres,
                    EMail = p.EMail,
                    Isim = p.Isim,
                    lat = p.lat,
                    lon = p.lon,
                    Mesafe = p.Mesafe,
                    Telefon = p.Telefon,
                    MusterilerID = p.MusterilerID
                }).OrderBy(e => e.Isim).ToList();
            }
        }
    }
}

