using DAL;
using DomainEntity.Models;
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
    public partial class UrunEkleEkrani : Form
    {
        KahveciContext ctx = new KahveciContext();
        public  string UrunAdi { get; set; }
        public  decimal UrunFiyat { get; set; }
        public bool GuncellemeMi { get; set; }
        public UrunEkleEkrani()
        {
            InitializeComponent();
        }
        Urun result;
        private void UrunEkleEkrani_Load(object sender, EventArgs e)
        {
            if (GuncellemeMi == true)
            {
                this.Text = "Güncelle";
                button1.Text = "Güncelle";
                textBox1.Text = UrunAdi;
                numericUpDown1.Value = UrunFiyat;
                result = ctx.Urunler.Where(x => x.UrunAdi == textBox1.Text).FirstOrDefault();
            }
            else { button1.Text = "Ekle"; this.Text = "Ekle"; }
                
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (GuncellemeMi == true)
            {
                
                if (result != null)
                {
                    try
                    {
                        result.UrunAdi = textBox1.Text;
                        result.Fiyat = numericUpDown1.Value;
                        ctx.Entry(result).State = EntityState.Modified;
                        ctx.SaveChanges();
                        MessageBox.Show("Ürün Güncellendi");
                        SiparisEkrani s = (SiparisEkrani)Application.OpenForms["SiparisEkrani"];
                        s.Yenile();

                    }
                    catch (Exception )
                    {
                        MessageBox.Show("Hata");
                    }
                }
            }
            else
            {
                Urun u = new Urun();
                u.Fiyat = numericUpDown1.Value;
                u.UrunAdi = textBox1.Text;
                ctx.Urunler.Add(u);
                ctx.SaveChanges();
                MessageBox.Show("Ürün Eklendi");
                SiparisEkrani s = (SiparisEkrani)Application.OpenForms["SiparisEkrani"];
                s.Yenile();
            }
        }
    }
}
