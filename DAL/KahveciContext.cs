﻿using DomainEntity.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class KahveciContext:DbContext
    {
        public virtual DbSet<Kullanici> Kullanicilar { get; set; }
        public virtual DbSet<Urun> Urunler { get; set; }
        public virtual DbSet<Siparis> Siparisler { get; set; }
        public virtual DbSet<SiparisDetay> SiparisDetaylar { get; set; }
    }
}
