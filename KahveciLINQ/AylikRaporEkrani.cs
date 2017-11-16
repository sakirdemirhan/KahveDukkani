using DAL;
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
    public partial class AylikRaporEkrani : Form
    {
        public AylikRaporEkrani()
        {
            InitializeComponent();
        }

        private void AylikRaporEkrani_Load(object sender, EventArgs e)
        {
            dateTimePicker1.ShowUpDown = true;
            this.Text = "Aylık Rapor";
        }
        public void RaporGetir(DateTime secilenGun)
        {
            KahveciContext db = new KahveciContext();
            GunlukRaporViewModel rapor = new GunlukRaporViewModel();
            try
            {
                rapor.ToplamSatisTutari = db.Siparisler
               .Where(x => DbFunctions.TruncateTime(x.Tarih).Value.Month == secilenGun.Month && DbFunctions.TruncateTime(x.Tarih).Value.Year == secilenGun.Year)
               .Sum(x => x.SiparistekiUrunler
                   .Sum(y => y.Tutar));

                rapor.ToplamSatilanUrunSayisi = db.Siparisler
                    .Where(x => DbFunctions.TruncateTime(x.Tarih).Value.Month == secilenGun.Month && DbFunctions.TruncateTime(x.Tarih).Value.Year == secilenGun.Year)
                    .Sum(x => x.SiparistekiUrunler
                        .Sum(y => y.Miktar));

                rapor.KullaniciBasiSatislar = (from sd in db.Siparisler
                                               join k in db.Kullanicilar
                                               on sd.KaydedenKullaniciId equals k.KullaniciID
                                               where DbFunctions.TruncateTime(sd.Tarih).Value.Month == secilenGun.Month && DbFunctions.TruncateTime(sd.Tarih).Value.Year == secilenGun.Year
                                               group sd by k.KullaniciAdi into yeni
                                               select new KullaniciSatisViewModel()
                                               {
                                                   KullaniciAdi = yeni.Key,
                                                   ToplamSatisTutari = yeni.Sum(x => x.SiparistekiUrunler.Sum(a => a.Tutar))
                                               }
                                               ).ToList();
                rapor.UrunBasiSatislar = (from sd in db.SiparisDetaylar
                                          where DbFunctions.TruncateTime(sd.Siparis.Tarih).Value.Month == secilenGun.Month && DbFunctions.TruncateTime(sd.Siparis.Tarih).Value.Year == secilenGun.Year
                                          group sd by sd.Urun.UrunID into yeni
                                          select new UrunBasiSatisViewModel()
                                          {
                                              UrunID = yeni.Key,
                                              UrunAdi = yeni.Max(x => x.Urun.UrunAdi),
                                              Adet = yeni.Sum(x => x.Miktar)
                                          }
                                          ).ToList();
        }
            catch (Exception ex)
            {
                label3.Text = ex.Message;
                MessageBox.Show("Bu ayda kayıt yok.");

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
            RaporGetir(dateTimePicker1.Value);
        }
    }
}
