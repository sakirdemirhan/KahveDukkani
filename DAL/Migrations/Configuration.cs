namespace DAL.Migrations
{
    using DomainEntity.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DAL.KahveciContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            //loss
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(DAL.KahveciContext context)
        {
            //kurulumdaki default kayýtlar
            if (context.Kullanicilar.Count() == 0)
            {
                Kullanici k = new Kullanici();
                k.KullaniciAdi = "Admin";
                k.Sifre = "123456";
                context.Kullanicilar.Add(k);
                context.SaveChanges();
            }

            if (context.Urunler.Count() == 0)
            {
                Urun u = new Urun();
                u.UrunAdi = "Çay";
                u.Fiyat = 3;
                Urun u2 = new Urun();
                u2.UrunAdi = "Latte";
                u2.Fiyat = 5;
                Urun u3 = new Urun();
                u3.UrunAdi = "Kahve";
                u3.Fiyat = 4;
                context.Urunler.Add(u);
                context.Urunler.Add(u2);
                context.Urunler.Add(u3);
                context.SaveChanges();
            }
        }
    }
}
