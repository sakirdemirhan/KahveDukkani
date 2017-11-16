using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DAL;
using Microsoft.Office.Interop.Excel;
using DataTable = System.Data.DataTable;


namespace KahveciLINQ
{
    public partial class TumSatislarEkrani : Form
    {
        private readonly KahveciContext db = new KahveciContext();
        public TumSatislarEkrani()
        {
            InitializeComponent();
        }

        private void TumSatislarEkrani_Load(object sender, EventArgs e)
        {
            this.Text = "Tüm Satışlar Raporu";
            dataGridView1.DataSource = (
                from sd in db.SiparisDetaylar
                join s in db.Siparisler
                on sd.SiparisID equals s.SiparisID
                join u in db.Urunler
                on sd.UrunID equals u.UrunID
                join k in db.Kullanicilar
                on s.KaydedenKullaniciId equals k.KullaniciID
                select new
                {
                    Kullanıcı = k.KullaniciAdi,
                    Ürün = u.UrunAdi,
                    Adet = sd.Miktar,
                    sd.Tutar,
                    s.Tarih
                }
            ).ToList();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var tbl = new DataTable();
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                if (column.Visible)
                {
                    tbl.Columns.Add(column.HeaderText);
                }
            }

            object[] cellValues = new object[dataGridView1.Columns.Count];
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                for (int i = 0; i < row.Cells.Count; i++)
                {
                    cellValues[i] = row.Cells[i].Value;
                }
                tbl.Rows.Add(cellValues);
            }
            string excelFilePath = @"C:\Users\nuuklu\Desktop\";
            try
            {
                if (tbl == null || tbl.Columns.Count == 0)
                    throw new Exception("ExportToExcel: Boş veya geçersiz tablo!\n");

                // load excel, and create a new workbook
                var excelApp = new Microsoft.Office.Interop.Excel.Application();
                excelApp.Workbooks.Add();

                // single worksheet
                Microsoft.Office.Interop.Excel._Worksheet workSheet = excelApp.ActiveSheet;

                // column headings
                for (var i = 0; i < tbl.Columns.Count; i++)
                {
                    workSheet.Cells[1, i + 1] = tbl.Columns[i].ColumnName;
                }

                // rows
                for (var i = 0; i < tbl.Rows.Count; i++)
                {
                    // to do: format datetime values before printing
                    for (var j = 0; j < tbl.Columns.Count; j++)
                    {
                        workSheet.Cells[i + 2, j + 1] = tbl.Rows[i][j];
                    }
                }

                // check file path
                if (!string.IsNullOrEmpty(excelFilePath))
                {
                    try
                    {
                        workSheet.SaveAs(DateTime.Now.Year.ToString()+ DateTime.Now.Month.ToString()+ DateTime.Now.Day.ToString());
                        excelApp.Quit();
                        MessageBox.Show("Excel dosyası kaydedildi!");
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("ExportToExcel: Kayıt başarısız. Kaydetme yolunu kontrol edin.\n"
                                            + ex.Message);
                    }
                }
                else { // no file path is given
                    excelApp.Visible = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("ExportToExcel: \n" + ex.Message);
            }
        }
        
    

}
}
