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
    public class RotaRepo
    {
        public static bool Ekle(int id)
        {
            using (PADB db = new PADB())
            {
                try
                {
                    bool kontrol = db.Nav.Any(p => p.MusterilerID == id);
                    if (kontrol)
                    {
                        return true;
                    }
                    else
                    {
                        db.Nav.Add(new Nav
                        {
                            MusterilerID = id
                        });
                        db.SaveChanges();
                        return true;
                    }
                }
                catch
                {
                    return false;
                }
            }
        }
        public static bool Sil(int id)
        {
            try
            {
                using (PADB db = new PADB())
                {
                    var Bul = db.Nav.FirstOrDefault(p => p.MusterilerID == id);
                    db.Nav.Remove(Bul);
                    db.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        public static bool ListeyiSil()
        {
            try
            {
                using (PADB db = new PADB())
                {
                    var Bul = db.Nav.ToList();
                    db.Nav.RemoveRange(Bul);
                    db.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        public static List<VMMusteri> NavListe()
        {
            using (PADB db = new PADB())
            {
                return db.Nav.Select(p => new VMMusteri
                {
                    Adres = p.Musteriler.Adres,
                    EMail = p.Musteriler.EMail,
                    Isim = p.Musteriler.Isim,
                    lat = p.Musteriler.lat,
                    lon = p.Musteriler.lon,
                    Mesafe = p.Musteriler.Mesafe,
                    Telefon = p.Musteriler.Telefon,
                    MusterilerID = p.Musteriler.MusterilerID
                }).OrderBy(e => e.Mesafe).ToList();
            }
        }
    }
}
