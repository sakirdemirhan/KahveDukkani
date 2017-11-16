using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntity.ViewModels
{
    public class GunlukRaporViewModel
    {
        public decimal ToplamSatisTutari { get; set; }
        public int ToplamSatilanUrunSayisi { get; set; }
        public List<KullaniciSatisViewModel> KullaniciBasiSatislar { get; set; }
        public List<UrunBasiSatisViewModel> UrunBasiSatislar { get; set; }
    }
    public class KullaniciSatisViewModel
    {
        [Browsable(false)]
        public int KullaniciID { get; set; }
        [DisplayName("Kullanıcı")]
        public string KullaniciAdi { get; set; }
        [DisplayName("Toplam Satış Tutarı")]
        public decimal? ToplamSatisTutari { get; set; }
    }
    public class UrunBasiSatisViewModel
    {
        [Browsable(false)]
        public int UrunID { get; set; }
        [DisplayName("Ürün Adı")]
        public string UrunAdi { get; set; }
        public int Adet { get; set; }
    }
}
