using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntity.Models
{
    [Table("tblUrun")]
    public class Urun
    {
        [Key]
        public int UrunID { get; set; }
        [Required]
        public string UrunAdi { get; set; }
        [Required]
        public decimal Fiyat { get; set; }
    }
}
