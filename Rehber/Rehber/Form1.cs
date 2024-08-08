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
namespace Rehber
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection("server=localhost; Initial Catalog=Rehber;Integrated Security=SSPI");
        String fileName;
        void listele()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("Select * from KISILER", baglanti);
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        void temizle()
        {
            TxtId.Text = "";
            TxtAd.Text = "";
            TxtSoyad.Text = "";
            MskTelefon.Text = "";
            TxtMail.Text = "";
            TxtAd.Focus();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            listele();

        }

        private void BtnEkle_Click(object sender, EventArgs e)
        {
            DialogResult soru = new DialogResult();
            soru = MessageBox.Show("Yeni kayıt eklemek istediğinize emin misiniz?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (soru == DialogResult.Yes)
            {
                if (TxtAd.Text.Length <= 0 || TxtSoyad.Text.Length <= 0 || MskTelefon.Text.Length <= 0 || TxtMail.Text.Length <= 0)
                {
                    MessageBox.Show("Lütfen bütün bilgileri eksiksiz doldurunuz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    baglanti.Open();
                    SqlCommand komut = new SqlCommand("Insert into KISILER(AD,SOYAD,TELEFON, MAIL, KISIRESIM) values ('" + TxtAd.Text + "', '" + TxtSoyad.Text + "' , '" + MskTelefon.Text + "' , '" + TxtMail.Text + "', '" + TxtResim.Text + "')", baglanti);
                    komut.ExecuteNonQuery();
                    baglanti.Close();
                    MessageBox.Show("Yeni Kişi Eklendi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    listele();
                    temizle();
                }
            }
            else
            {
                MessageBox.Show("Kişi ekleme işlemi iptal edildi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            

        }

        private void BtnTemizle_Click(object sender, EventArgs e)
        {
            temizle();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilen = dataGridView1.SelectedCells[0].RowIndex;
            TxtId.Text = dataGridView1.Rows[secilen].Cells[0].Value.ToString();
            TxtAd.Text = dataGridView1.Rows[secilen].Cells[1].Value.ToString();
            TxtSoyad.Text = dataGridView1.Rows[secilen].Cells[2].Value.ToString();
            MskTelefon.Text = dataGridView1.Rows[secilen].Cells[3].Value.ToString();
            TxtMail.Text = dataGridView1.Rows[secilen].Cells[4].Value.ToString();
            TxtResim.Text = dataGridView1.Rows[secilen].Cells[5].Value.ToString();
            pictureBox1.Image = Image.FromFile(dataGridView1.Rows[secilen].Cells[5].Value.ToString());
        }

        private void BtnSil_Click(object sender, EventArgs e)
        {
            DialogResult soru = new DialogResult();
            soru = MessageBox.Show("Seçilen kaydı silmek istediğinize emin misiniz?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (soru == DialogResult.Yes)
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("Delete from KISILER where ID=" + TxtId.Text, baglanti);
                komut.ExecuteNonQuery();
                baglanti.Close();
                MessageBox.Show("Kişi Rehberden Silindi", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                listele();
                temizle();
            }
            else
            {
                MessageBox.Show("Kişi Silme işlemi iptal edildi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void BtnGuncelle_Click(object sender, EventArgs e)
        {
            DialogResult soru = new DialogResult();
            soru = MessageBox.Show("Seçilen kaydı güncellemek istediğinize emin misiniz?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (soru == DialogResult.Yes)
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("Update KISILER set AD = @p1, SOYAD = @p2, TELEFON = @p3, MAIL = @p4, KISIRESIM = @p5 where ID = @p6", baglanti);
                komut.Parameters.AddWithValue("@p1", TxtAd.Text);
                komut.Parameters.AddWithValue("@p2", TxtSoyad.Text);
                komut.Parameters.AddWithValue("@p3", MskTelefon.Text);
                komut.Parameters.AddWithValue("@p4", TxtMail.Text);
                komut.Parameters.AddWithValue("@p5", TxtResim.Text);
                komut.Parameters.AddWithValue("@p6", TxtId.Text);
                komut.ExecuteNonQuery();
                baglanti.Close();
                MessageBox.Show("Kişi Bilgisi Güncellendi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                listele();
                temizle();
            }
            else
            {
                MessageBox.Show("Kayıt güncelleme işlemi iptal edildi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
        }
        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            pictureBox1.ImageLocation = openFileDialog1.FileName;
            TxtResim.Text = openFileDialog1.FileName;

        }

        private void TxtResim_TextChanged(object sender, EventArgs e)
        {
            //String imagePath = TxtResim.Text;
            //String path = Application.StartupPath + "\\" + "Images\\" + fileName;
            //File.Copy(imagePath, path);
            // pictureBox1.ImageLocation = openFileDialog1.FileName;
        }
    }
}
