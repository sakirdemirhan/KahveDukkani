using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntity.Models
{
    [Table("tblSiparis")]
    public class Siparis
    {
        [Key]
        public int SiparisID { get; set; }
        public DateTime Tarih { get; set; }
        public int KaydedenKullaniciId { get; set; }
        public virtual List<SiparisDetay> SiparistekiUrunler { get; set; }
        public Siparis()
        {
            Tarih = DateTime.Now;
        }
    }
}
