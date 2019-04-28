namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblKullanici",
                c => new
                    {
                        KullaniciID = c.Int(nullable: false, identity: true),
                        KullaniciAdi = c.String(nullable: false, maxLength: 100),
                        Sifre = c.String(nullable: false, maxLength: 20),
                        EklenmeTarihi = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.KullaniciID);
            
            CreateTable(
                "dbo.SiparisDetays",
                c => new
                    {
                        SiparisDetayID = c.Int(nullable: false, identity: true),
                        SiparisID = c.Int(nullable: false),
                        UrunID = c.Int(nullable: false),
                        Miktar = c.Int(nullable: false),
                        Tutar = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.SiparisDetayID)
                .ForeignKey("dbo.tblSiparis", t => t.SiparisID, cascadeDelete: true)
                .ForeignKey("dbo.tblUrun", t => t.UrunID, cascadeDelete: true)
                .Index(t => t.SiparisID)
                .Index(t => t.UrunID);
            
            CreateTable(
                "dbo.tblSiparis",
                c => new
                    {
                        SiparisID = c.Int(nullable: false, identity: true),
                        Tarih = c.DateTime(nullable: false),
                        KaydedenKullaniciId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SiparisID);
            
            CreateTable(
                "dbo.tblUrun",
                c => new
                    {
                        UrunID = c.Int(nullable: false, identity: true),
                        UrunAdi = c.String(nullable: false),
                        Fiyat = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.UrunID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SiparisDetays", "UrunID", "dbo.tblUrun");
            DropForeignKey("dbo.SiparisDetays", "SiparisID", "dbo.tblSiparis");
            DropIndex("dbo.SiparisDetays", new[] { "UrunID" });
            DropIndex("dbo.SiparisDetays", new[] { "SiparisID" });
            DropTable("dbo.tblUrun");
            DropTable("dbo.tblSiparis");
            DropTable("dbo.SiparisDetays");
            DropTable("dbo.tblKullanici");
        }
    }
}
