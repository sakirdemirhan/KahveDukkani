using DAL;
using DomainEntity.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KahveciLINQ
{
    public partial class Form1 : Form
    {
        KahveciContext context = new KahveciContext();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Kullanici k = context.Kullanicilar.FirstOrDefault(x => x.KullaniciAdi == textBox1.Text
            && x.Sifre == textBox2.Text);
            
            if (k == null)
            {
                
                MessageBox.Show("Hatalı Giriş");
            }
            else
            {
                var id = k.KullaniciID;
                SiparisEkrani.GirisYapanKullanici = id;
                new SiparisEkrani().Show();
                this.Hide();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //buton focusu alır. formun hala tuşları dinleyebilmesi için
            this.Text = "Giriş";
            this.KeyPreview = true;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                button1.PerformClick();
        }
    }
}
