using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;

namespace Etüt_Test
{
    public partial class EtutApp : Form
    {
        public EtutApp()
        {
            InitializeComponent();
        }

        SqlConnection baglanti = new SqlConnection("Data Source=MEHLIKA\\SQLEXPRESS;Initial Catalog=EtutTest;Integrated Security=True");
        /// <summary>
        /// Derslerin veritabanından çekilerek ComboBox'a yüklenmesini sağlar.
        /// </summary>
        void derslistesi()
        {
            SqlDataAdapter da = new SqlDataAdapter("Select * From TBLDERSLER",baglanti);
            DataTable dt = new DataTable();
            da.Fill(dt);
            cmbDers.ValueMember="DERSID";
            cmbDers.DisplayMember = "DERSAD";
            cmbDers.DataSource=dt;
            cmboGRTDers.ValueMember = "DERSID";
            cmboGRTDers.DisplayMember = "DERSAD";
            cmboGRTDers.DataSource = dt;
            baglanti.Close();
        }
        /// <summary>
        /// Var olan etütlerin Grid'de listelenmesini sağlar.
        /// </summary>
        void etüt()
        {
            try
            {
                SqlDataAdapter da3 = new SqlDataAdapter("execute etutTest", baglanti);
                DataTable d3 = new DataTable();
                da3.Fill(d3);
                dataGridView1.DataSource = d3;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            derslistesi();
            etüt();
        }

        private void cmbOgrt_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmbDers_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                SqlDataAdapter da2 = new SqlDataAdapter("Select * From TBLOGRETMEN where BRANSID=" + cmbDers.SelectedValue + ";", baglanti);
                DataTable dt2 = new DataTable();
                da2.Fill(dt2);
                cmbOgrt.ValueMember = "OGRTID";
                cmbOgrt.DisplayMember = "AD";
                cmbOgrt.DataSource = dt2;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnEtutOlustur_Click(object sender, EventArgs e)
        {
            try
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("insert into TBLETUT (DERSID,OGRETMENID,TARIH,SAAT) values (@p1,@p2,@p3,@p4)", baglanti);
                komut.Parameters.AddWithValue("@p1", cmbDers.SelectedValue);
                komut.Parameters.AddWithValue("@p2", cmbOgrt.SelectedValue);
                komut.Parameters.AddWithValue("@p3", mskdTarih.Text);
                komut.Parameters.AddWithValue("@p4", mskdSaat.Text);
                komut.ExecuteNonQuery();
                baglanti.Close();
                MessageBox.Show("Oluşturuldu");
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message); 
            }
        }
        /// <summary>
        /// Var olan bir etüdü güncellemeye yarar.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEtutVer_Click(object sender, EventArgs e)
        {
            try
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("update TBLETUT set OGRENCIID=@p1,DURUM=@p2 where ID=@p3", baglanti);
                komut.Parameters.AddWithValue("@p1", txtId.Text);
                komut.Parameters.AddWithValue("@p2", "True");
                komut.Parameters.AddWithValue("@p3", txtOgrenci.Text);
                komut.ExecuteNonQuery();
                baglanti.Close();
                MessageBox.Show("Etüt Öğrenciye Verildi");
            }
            catch (Exception ex) 
            { 
                MessageBox.Show(ex.Message); 
            }
        }
        /// <summary>
        /// Öğrencinin profil fotoğrafını eklemeyi sağlar.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFotografEkle_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            pictureBox1.ImageLocation = openFileDialog1.FileName;
        }
        /// <summary>
        /// Öğrencilerin, veritabanına eklenebilmesini sağlar.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOgrenciEkle_Click(object sender, EventArgs e)
        {
            try
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("insert into TBLOGRENCI (AD,SOYAD,SINIF,TELEFON,MAIL,FOTOGRAF) values (@p1,@p2,@p3,@p4,@p5,@p6)", baglanti);
                komut.Parameters.AddWithValue("@p1", txtAd.Text);
                komut.Parameters.AddWithValue("@p2", txtSoyad.Text);
                komut.Parameters.AddWithValue("@p3", txtSinif.Text);
                komut.Parameters.AddWithValue("@p4", txtTel.Text);
                komut.Parameters.AddWithValue("@p5", txtMail.Text);
                komut.Parameters.AddWithValue("@p6", pictureBox1.ImageLocation);
                komut.ExecuteNonQuery();
                baglanti.Close();
                MessageBox.Show("Eklendi");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnOgretmenEkle_Click(object sender, EventArgs e)
        {
            try
            {
                baglanti.Open();
                SqlCommand komut2 = new SqlCommand("insert into TBLOGRETMEN (AD,SOYAD,BRANSID) values (@s1,@s2,@s3)", baglanti);
                komut2.Parameters.AddWithValue("@s1", txtOgrtAd.Text);
                komut2.Parameters.AddWithValue("@s2", txtOgrtSoyad.Text);
                komut2.Parameters.AddWithValue("@s3", cmboGRTDers.SelectedValue);
                komut2.ExecuteNonQuery();
                baglanti.Close();
                MessageBox.Show("Öğretmen Eklendi");
            }
            catch (Exception ex)
            {
                //Farklı Logging frameworkleri kullanılabilir.
                MessageBox.Show(ex.Message);
            }
        }

        private void cmboGRTDers_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                baglanti.Open();
                SqlDataAdapter da3 = new SqlDataAdapter("Select * From TBLOGRETMEN where BRANSID=" + cmbDers.SelectedValue + ";", baglanti);
                DataTable dt3 = new DataTable();
                da3.Fill(dt3);
                cmbOgrt.ValueMember = "OGRTID";
                cmbOgrt.DisplayMember = "AD";
                cmbOgrt.DataSource = dt3;
                baglanti.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
