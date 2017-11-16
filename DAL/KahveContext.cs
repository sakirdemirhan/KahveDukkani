﻿using DomainEntity.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class KahveContext : DbContext
    {
        public virtual DbSet<Kullanici>Kullanicilar { get; set; }
    }
}
