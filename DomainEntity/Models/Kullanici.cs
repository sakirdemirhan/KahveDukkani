using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntity.Models
{
    [Table("tblKullanici")]
    public class Kullanici
    {
        
        [Key]
        public int KullaniciID { get; set; }
        [Required]
        [MaxLength(100)]
        public string KullaniciAdi { get; set; }
        [Required]
        [MaxLength(20)]
        public string Sifre { get; set; }

        public DateTime EklenmeTarihi { get; set; }

        public Kullanici()
        {
            EklenmeTarihi = DateTime.Now;
        }
    }
}
