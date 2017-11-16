using DAL;
using DomainEntity.Models;
using DomainEntity.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KahveciLINQ
{
    public partial class RaporEkrani : Form
    {
        
        public RaporEkrani()
        {
            InitializeComponent();
        }

        

        private void RaporEkrani_Load(object sender, EventArgs e)
        {
            this.Text = "Günlük Rapor";
        }

        public void RaporGetir(DateTime secilenGun)
        {
            KahveciContext db = new KahveciContext();
            GunlukRaporViewModel rapor = new GunlukRaporViewModel();
            try
            {
                rapor.ToplamSatisTutari = db.Siparisler
               .Where(x => DbFunctions.TruncateTime(x.Tarih) == dateTimePicker1.Value.Date)
               .Sum(x => x.SiparistekiUrunler
                   .Sum(y => y.Tutar));

                rapor.ToplamSatilanUrunSayisi = db.Siparisler
                    .Where(x => DbFunctions.TruncateTime(x.Tarih) == dateTimePicker1.Value.Date)
                    .Sum(x => x.SiparistekiUrunler
                        .Sum(y => y.Miktar));

                rapor.KullaniciBasiSatislar = (from sd in db.Siparisler
                                               join k in db.Kullanicilar
                                               on sd.KaydedenKullaniciId equals k.KullaniciID
                                               where DbFunctions.TruncateTime(sd.Tarih) == secilenGun
                                               group sd by k.KullaniciAdi into yeni
                                               select new KullaniciSatisViewModel()
                                               {
                                                   KullaniciAdi = yeni.Key,
                                                   ToplamSatisTutari = yeni.Sum(x => x.SiparistekiUrunler.Sum(a => a.Tutar))
                                               }
                                               ).ToList();
                rapor.UrunBasiSatislar = (from sd in db.SiparisDetaylar
                                          where DbFunctions.TruncateTime(sd.Siparis.Tarih) == secilenGun
                                          group sd by sd.Urun.UrunID into yeni
                                          select new UrunBasiSatisViewModel()
                                          {
                                              UrunID = yeni.Key,
                                              UrunAdi = yeni.Max(x => x.Urun.UrunAdi),
                                              Adet = yeni.Sum(x => x.Miktar)
                                          }
                                          ).ToList();
            }
            catch (Exception)
            {

                MessageBox.Show("Bu günde kayıt yok.");
            }
           

            label6.Text = rapor.ToplamSatisTutari.ToString("C");
            label7.Text = rapor.ToplamSatilanUrunSayisi.ToString();
            dataGridView1.DataSource = null;
            dataGridView2.DataSource = null;
            dataGridView1.DataSource = rapor.KullaniciBasiSatislar;
            dataGridView2.DataSource = rapor.UrunBasiSatislar;
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            RaporGetir(dateTimePicker1.Value.Date);
        }
    }
}
