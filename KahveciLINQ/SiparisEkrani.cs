using DAL;
using DomainEntity.Models;
using DomainEntity.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KahveciLINQ
{
    public partial class SiparisEkrani : Form
    {
        public static int GirisYapanKullanici { get; set; }

        KahveciContext ctx = new KahveciContext();
        List<Sepet> sepet = new List<Sepet>();
        private readonly ContextMenuStrip collectionRoundMenuStrip;
        public SiparisEkrani()
        {
            InitializeComponent();
            var toolStripMenuItem2 = new ToolStripMenuItem { Text = "Sil" };
            toolStripMenuItem2.Click += toolStripMenuItem2_Click;
            var toolStripMenuItem3 = new ToolStripMenuItem { Text = "Düzenle" };
            toolStripMenuItem3.Click += toolStripMenuItem3_Click;
            collectionRoundMenuStrip = new ContextMenuStrip();
            collectionRoundMenuStrip.Items.AddRange(new ToolStripItem[] {  toolStripMenuItem2,toolStripMenuItem3 });
           
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {//sil
            Urun u = (Urun)listBox2.SelectedItem;
            ctx.Urunler.Remove(u);
            ctx.SaveChanges();
            Yenile();
        }

        public void Yenile()
        {
            flowLayoutPanel1.Controls.Clear();
            ctx = new KahveciContext();
            listBox2.DataSource = null;
            listBox2.ValueMember = "UrunID";
            listBox2.DisplayMember = "UrunAdi";
            listBox2.DataSource = ctx.Urunler.OrderBy(x => x.Fiyat).ToList();
            
            if (ctx.Urunler.Count() != 0)
            {
                foreach (Urun urun in ctx.Urunler)
                {
                    Button btn = new Button();
                    btn.Width = 100;
                    btn.Height = 40;
                    btn.Name = "Urun_" + urun.UrunID;
                    btn.Text = urun.UrunAdi;
                    //btn.Click += butonMetodu_Click;
                    btn.Click += UrunuSipariseEkle;
                    flowLayoutPanel1.Controls.Add(btn);
                }
            }
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {//duzenle
            UrunEkleEkrani ue = new UrunEkleEkrani();
            Urun u = (Urun)listBox2.SelectedItem;
            ue.UrunAdi = u.UrunAdi;
            ue.UrunFiyat = u.Fiyat;
            ue.GuncellemeMi = true;
            ue.Show();
        }

        

        decimal toplamFiyat = 0;
        RegionInfo ri = new RegionInfo("tr-TR");

        private void SiparisEkrani_Load(object sender, EventArgs e)
        {
            this.Text = "Sipariş Ekranı";
            flowLayoutPanel1.AutoScroll = true;
            Yenile();
        }
        public void UrunuSipariseEkle(object hangiButon, EventArgs e)
        {
            toplamFiyat = 0;
            Button tiklananButon = (Button)hangiButon;
            int UrunID = Convert.ToInt32(tiklananButon.Name.Replace("Urun_", ""));
            Urun secilenUrun = ctx.Urunler.Find(UrunID);
            var sepetteki = sepet.Find(x => x.UrunID == UrunID);
            if (sepetteki == null)
            {
                sepet.Add(new Sepet()
                {
                    UrunID = UrunID,
                    UrunAdi = secilenUrun.UrunAdi,
                    BirimFiyat = secilenUrun.Fiyat,
                    Miktar = 1
                });
                foreach (var item in sepet)
                {
                    toplamFiyat += item.ToplamFiyat;
                }
                label2.Text = toplamFiyat.ToString("C");
            }
            else
            {
                sepetteki.Miktar++;
                foreach (var item in sepet)
                {
                    toplamFiyat += item.ToplamFiyat;
                }
                label2.Text = toplamFiyat.ToString("C");
            }

                
            siparisDetayYenile();
        }

        private void siparisDetayYenile()
        {
            listBox1.DataSource = null;
            listBox4.DataSource = null;
            listBox3.DataSource = null;

            listBox1.DataSource = sepet;
            listBox4.DataSource = sepet;
            listBox3.DataSource = sepet;

            listBox1.ValueMember = "UrunID";
            listBox1.DisplayMember = "UrunAdi";
            listBox4.DisplayMember = "Miktar";
            listBox3.DisplayMember = "BirimFiyat";

            
        }

        

        private void button5_Click(object sender, EventArgs e)
        {//temizle
            sepet.Clear();
            siparisDetayYenile();
            toplamFiyat = 0;
            label2.Text = toplamFiyat.ToString("C");
        }
        
        private void button4_Click(object sender, EventArgs e)
        {//sil
            toplamFiyat = 0;
            int secilenID = (int)listBox1.SelectedValue;
            var silinecek = sepet.Find(x => x.UrunID == secilenID);
            silinecek.Miktar--;
            if (silinecek.Miktar == 0)
                sepet.Remove(silinecek);
            siparisDetayYenile();
            foreach (var item in sepet)
            {
                toplamFiyat += item.ToplamFiyat;
            }
            label2.Text = toplamFiyat.ToString("C");
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           //listBox3.SelectedIndex = listBox1.SelectedIndex;
        }

        private void button6_Click(object sender, EventArgs e)
        {//ödeme
            Siparis siparis = new Siparis();
            
            siparis.KaydedenKullaniciId = GirisYapanKullanici;
            siparis.SiparistekiUrunler = new List<SiparisDetay>();
            foreach (Sepet item in sepet)
            {
                SiparisDetay sd = new SiparisDetay();
                sd.Miktar = item.Miktar;
                sd.Tutar = item.ToplamFiyat;
                sd.UrunID = item.UrunID;
                siparis.SiparistekiUrunler.Add(sd);
            }
            ctx.Siparisler.Add(siparis);
            ctx.SaveChanges();
            MessageBox.Show("Siparişler Tablosuna Eklendi.");
            //button5.PerformClick();
        }

        private void günlükRaporToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new RaporEkrani().Show();
        }

        private void aylıkRaporToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AylikRaporEkrani().Show();
        }

        private void listBox2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;
            var index = listBox2.IndexFromPoint(e.Location);
            listBox2.SelectedIndex = index;
            if (index != ListBox.NoMatches)
            {
                //_selectedMenuItem = listBoxCollectionRounds.Items[index].ToString();
                collectionRoundMenuStrip.Show(Cursor.Position);
                collectionRoundMenuStrip.Visible = true;
            }
            else
            {
                collectionRoundMenuStrip.Visible = false;
            }
        }

        private void ürünleriYönetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new UrunEkleEkrani().Show();
        }

        private void tümSatışlarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new TumSatislarEkrani().Show();
        }
    }
}
