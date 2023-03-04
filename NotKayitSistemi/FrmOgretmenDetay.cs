using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NotKayitSistemi
{
    public partial class FrmOgretmenDetay : Form
    {
        public FrmOgretmenDetay()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection("Data Source=DESKTOP-B2AV4A5\\SQLDEVELOPER;Initial Catalog=DbNotKayit;Integrated Security=True");

        private void FrmOgretmenDetay_Load(object sender, EventArgs e)
        {
            this.tBLDERSTableAdapter.Fill(this.dbNotKayitDataSet.TBLDERS);
        }

        private void btnOgrKaydet_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("insert into TBLDERS(OGRNUMARA,OGRAD,OGRSOYAD) values (@e1,@e2,@e3)", baglanti);
            komut.Parameters.AddWithValue("e1", mskNumara.Text);
            komut.Parameters.AddWithValue("e2", txtAd.Text);
            komut.Parameters.AddWithValue("e3", txtSoyad.Text);
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Öğrenci sisteme kaydedildi");
            this.tBLDERSTableAdapter.Fill(this.dbNotKayitDataSet.TBLDERS);

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            mskNumara.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtAd.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            txtSoyad.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            txtSınav1.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
            txtSınav2.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
            txtSınav3.Text = dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString();
        }
        
        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            double ortalama, s1, s2, s3;
            string durum;
            s1 = Convert.ToDouble(txtSınav1.Text);
            s2 = Convert.ToDouble(txtSınav2.Text);
            s3 = Convert.ToDouble(txtSınav3.Text);

            ortalama = (s1 + s2 + s3) / 3;
            lblOrtalama.Text = ortalama.ToString();

            if (ortalama >= 50)
            {
                durum = "True";
            }
            else
            {
                durum = "False";
            }

            baglanti.Open();
            SqlCommand komut = new SqlCommand("update TBLDERS set OGRS1=@S1,OGRS2=@S2,OGRS3=@S3,ORTALAMA=@S4,DURUM=@S5 where OGRNUMARA=@S6", baglanti);
            komut.Parameters.AddWithValue("@S1", txtSınav1.Text);
            komut.Parameters.AddWithValue("@S2", txtSınav2.Text);
            komut.Parameters.AddWithValue("@S3", txtSınav3.Text);
            komut.Parameters.AddWithValue("@S4", decimal.Parse(lblOrtalama.Text));
            komut.Parameters.AddWithValue("@S5", durum);
            komut.Parameters.AddWithValue("@S6", mskNumara.Text);
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Öğrenci sınav notları güncellendi");
            this.tBLDERSTableAdapter.Fill(this.dbNotKayitDataSet.TBLDERS);

            //Gecen Ogrenci Sayısı
            baglanti.Open();
            SqlCommand komut2 = new SqlCommand("SELECT COUNT(*) FROM TBLDERS WHERE DURUM=1", baglanti);
            SqlDataReader dr2 = komut2.ExecuteReader();
            while (dr2.Read())
            {
                lblGecenSayısı.Text = dr2[0].ToString();
            }
            baglanti.Close();
            this.tBLDERSTableAdapter.Fill(this.dbNotKayitDataSet.TBLDERS);

            //Kalan Ogrenci Sayısı
            baglanti.Open();
            SqlCommand komut3 = new SqlCommand("SELECT COUNT(*) FROM TBLDERS WHERE DURUM=0", baglanti);
            SqlDataReader dr3 = komut3.ExecuteReader();
            while (dr3.Read())
            {
                lblKalanSayısı.Text = dr3[0].ToString();
            }
            baglanti.Close();
            this.tBLDERSTableAdapter.Fill(this.dbNotKayitDataSet.TBLDERS);
        }
    }
}
