using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HastaKayitProjesiDb
{
    public partial class HastaKayitFormu : Form
    {
        public HastaKayitFormu()
        {
            InitializeComponent();
        }

        
        private void GetData()
        {
            try
            {
                listView1.Items.Clear();

                string query = "Select * From TblHastaKayit";

                DataTable table = Database.GetDataTable(query);


                foreach (DataRow row in table.Rows)
                {
                    ListViewItem item = new ListViewItem(row["HastaTC"].ToString());         // ListViewItem: Listenin ilk sütununu temsil eder.
                    item.SubItems.Add(row["HastaAdSoyad"].ToString());                      // ListViewItem.SubItems: Geri kalan sütunları temsil eder.
                    item.SubItems.Add(row["Yas"].ToString());
                    item.SubItems.Add(row["Cinsiyet"].ToString());
                    item.SubItems.Add(row["Sikayet"].ToString());
                    item.SubItems.Add(row["Tahliller"].ToString());

                    listView1.Items.Add(item);
                }
            }
            catch
            {
                MessageBox.Show("Görüntüleme sırasında bir hata oluştu!");
            }
        }


        private void ClearForm()
        {
            txtTC.Clear();
            txtName.Clear();
            txtAge.Clear();
            txtComplaint.Clear();
            txtTest.Clear();
            txtGender.Clear();
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            AddHasta();
            GetData();
            ClearForm();
        }

        private void AddHasta()
        {
            try
            {
                string tc = txtTC.Text;
                if(tc == "")
                {
                    MessageBox.Show("TC giriniz!");
                    return;
                }


                string adsoyad = txtName.Text;
                if(adsoyad == "")
                {
                    MessageBox.Show("Ad Soyad giriniz!");
                    return;
                }


                string yas = txtAge.Text;
                if(yas == "")
                {
                    MessageBox.Show("Yaş boş olamaz!");
                    return;
                }


                List<SqlParameter> parameters = new List<SqlParameter>();

                string query = "insert into TblHastaKayit (HastaTC, HastaAdSoyad, Yas, Cinsiyet, Sikayet, Tahliller)" +
                             "values(@hastaTc, @adsoyad, @yas, @cinsiyet, @sikayet, @tahlil)";


                parameters.Add(Database.AddParameter("@hastaTc", txtTC.Text, SqlDbType.NVarChar));
                parameters.Add(Database.AddParameter("@adsoyad", txtName.Text, SqlDbType.NVarChar));
                parameters.Add(Database.AddParameter("@yas", txtAge.Text, SqlDbType.Int));
                parameters.Add(Database.AddParameter("@cinsiyet", txtGender.Text, SqlDbType.NVarChar));
                parameters.Add(Database.AddParameter("@sikayet", txtComplaint.Text, SqlDbType.NVarChar));
                parameters.Add(Database.AddParameter("@tahlil", txtTest.Text, SqlDbType.NVarChar));


                int result = Database.ExecuteCommand(query, parameters);

                if (result > 0)
                {
                    MessageBox.Show("Hasta kaydedildi!");
                }
                else
                {
                    MessageBox.Show("Hasta kaydedilirken bir hata oluştu!");
                }
            }
            catch
            {
                MessageBox.Show("Hasta kaydı başarısız!");
            }
            
        }


        private void btnView_Click(object sender, EventArgs e)
        {
            GetData();
        }


        private void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteHasta();
            GetData();
            ClearForm();
        }

        private void DeleteHasta()
        {
            try
            {
                if (listView1.SelectedItems.Count == 1)
                {
                    string tc = listView1.SelectedItems[0].Text;
                    if (string.IsNullOrEmpty(tc))
                    {
                        return;
                    }

                    List<SqlParameter> parameters = new List<SqlParameter>();

                    string query = "Delete From TblHastaKayit where HastaTC = @hastaTc";

                    parameters.Add(Database.AddParameter("@hastaTc", tc, SqlDbType.NVarChar));
                    
                    int result = Database.ExecuteCommand(query, parameters);

                    if(result > 0)
                    {
                        MessageBox.Show("Silme işlemi başarılı.");
                    }
                    else
                    {
                        MessageBox.Show("Silme işlemi başarısız.");
                    }
                }
            }
            catch
            {
                MessageBox.Show("Silme işlemi sırasında bir hata oluştu!");
            }
            
        }


        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            txtTC.Text = listView1.SelectedItems[0].SubItems[0].Text;
            txtName.Text = listView1.SelectedItems[0].SubItems[1].Text;
            txtAge.Text = listView1.SelectedItems[0].SubItems[2].Text;
            txtGender.Text = listView1.SelectedItems[0].SubItems[3].Text;
            txtComplaint.Text = listView1.SelectedItems[0].SubItems[4].Text;
            txtTest.Text = listView1.SelectedItems[0].SubItems[5].Text;
        }


        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateHasta();
            GetData();
            ClearForm();
        }

        private void UpdateHasta()
        {
            try
            {
                string tc = txtTC.Text;
                if (tc == "")
                {
                    MessageBox.Show("TC giriniz!");
                    return;
                }


                string adsoyad = txtName.Text;
                if (adsoyad == "")
                {
                    MessageBox.Show("Ad Soyad giriniz!");
                    return;
                }


                string yas = txtAge.Text;
                if(yas == "")
                {
                    MessageBox.Show("Yaş boş olamaz!");
                    return;
                }


                List<SqlParameter> parameters = new List<SqlParameter>();

                string query = "Update TblHastaKayit Set HastaTC = @hastaTc, HastaAdSoyad = @adsoyad, Yas = @yas," +
                    "Cinsiyet = @cinsiyet, Sikayet = @sikayet, Tahliller = @tahlil where HastaTC = @hastaTc";

                parameters.Add(Database.AddParameter("@hastaTc", txtTC.Text, SqlDbType.NVarChar));
                parameters.Add(Database.AddParameter("@adsoyad", txtName.Text, SqlDbType.NVarChar));
                parameters.Add(Database.AddParameter("@yas", txtAge.Text, SqlDbType.Int));
                parameters.Add(Database.AddParameter("@cinsiyet", txtGender.Text, SqlDbType.NVarChar));
                parameters.Add(Database.AddParameter("@sikayet", txtComplaint.Text, SqlDbType.NVarChar));
                parameters.Add(Database.AddParameter("@tahlil", txtTest.Text, SqlDbType.NVarChar));


                int result = Database.ExecuteCommand(query, parameters);

                if(result > 0)
                {
                    MessageBox.Show("Kayıt güncellendi!");
                }
                else
                {
                    MessageBox.Show("Kayıt güncelleme başarısız!");
                }
            }
            catch
            {
                MessageBox.Show("Kayıt güncelleme sırasında bir hata oluştu!");
            }

        }


        private void btnSearch_Click(object sender, EventArgs e)
        {
            bool aranankayit = false;

            for (int i = 0; i < listView1.Items.Count; i++)
            {
                if (txtTC.Text == listView1.Items[i].SubItems[0].Text)
                {
                    aranankayit = true;
                    MessageBox.Show(txtTC.Text + " TC numaralı hasta mevcut.");
                    listView1.Items[i].Selected = true;
                }
            }

            if (aranankayit == false)
            {
                MessageBox.Show(txtTC.Text + " TC numaralı hasta bulunamadı.");
            }
        }

    }
}
