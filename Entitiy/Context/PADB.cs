namespace Entitiy.Context
{
    using Entitiy.Model;
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class PADB : DbContext
    {
        public PADB()
            : base("name=PADB")
        {
            Database.SetInitializer(new PADBInitializer());
        }
        public virtual DbSet<Musteriler> Musteriler { get; set; }
        public virtual DbSet<Core> Core { get; set; }
        public virtual DbSet<Nav> Nav { get; set; }
    }
    public class PADBInitializer : CreateDatabaseIfNotExists<PADB>
    {
        protected override void Seed(PADB db)
        {
            db.Core.Add(new Core
            {
                FirmaAdi = "Choice Corp",
                Sifre = "9916",
                Adres = "Izmir,Turkey",
                YeniKurulum=true,
                lat= 38.4189,
                lon= 27.1287
            });
        }
    }
}