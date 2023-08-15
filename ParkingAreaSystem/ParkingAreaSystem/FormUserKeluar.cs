using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ParkingAreaSystem
{
    public partial class FormUserKeluar : Form
    {
        public FormUserKeluar()
        {
            InitializeComponent();
        }
        public static int showIdUser;
        public static string showPosisi;
        public static string showWaktuMasuk;
        public static string showWaktuKeluar;
        public static int showTarif;

        SqlConnection con = new SqlConnection(@"Data Source=LAPTOP-TMDMITR7;Initial Catalog=parking_area;Integrated Security=True");

        private void btnKeluar_Click(object sender, EventArgs e)
        {
            if(txtId.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Masukkan ID untuk keluar!", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DataTable dtPengguna = new DataTable();
            SqlCommand dataPengguna = new SqlCommand("SELECT * FROM user_parkir WHERE waktu_keluar IS NULL", con);
            DataTable dtRow = new DataTable();
            SqlCommand dataRow = new SqlCommand("SELECT COUNT(*) AS jmlrow FROM user_parkir  WHERE waktu_keluar IS NULL", con);

            con.Open();

            SqlDataReader sdrPengguna = dataPengguna.ExecuteReader();
            dtPengguna.Load(sdrPengguna);
            SqlDataReader sdrRow = dataRow.ExecuteReader();
            dtRow.Load(sdrRow);

            con.Close();

            DateTime currentTime = DateTime.Now;

            int userId;
            int userStatus;
            string userPosisi;
            string userWaktuKeluar = Convert.ToString(currentTime);
            string userWaktuMasuk;

            int jmlRow = Convert.ToInt32(dtRow.Rows[0]["jmlrow"].ToString()); ;
            int idInput = Convert.ToInt32(txtId.Text.ToString());

            for (int i = 1; i<=jmlRow ; i++)
            {
                con.Open();
                if (jmlRow == 1 || i==jmlRow+1 || idInput > Convert.ToInt32(dtPengguna.Rows[jmlRow-1]["id_user"].ToString()) || idInput < Convert.ToInt32(dtPengguna.Rows[1]["id_user"].ToString()))
                {
                    MessageBox.Show("ID Tidak Ditemukan!", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    con.Close();
                    return;
                }

                userId = Convert.ToInt32(dtPengguna.Rows[i - 1]["id_user"].ToString());
                userStatus = Convert.ToInt32(dtPengguna.Rows[i - 1]["status_user"].ToString());
                userPosisi = Convert.ToString(dtPengguna.Rows[i - 1]["posisi_parkir"].ToString());
                userWaktuMasuk = Convert.ToString(dtPengguna.Rows[i - 1]["waktu_masuk"].ToString());

                //MessageBox.Show(parkirUserId.ToString());
                //MessageBox.Show(parkirStatus.ToString());

                if (userStatus == 1 && userId == idInput)
                {
                    SqlCommand cmdArea = new SqlCommand("UPDATE area_parkir SET status_parkir = 0, id_user = 0 WHERE id_user = @parkirUserId", con);
                    cmdArea.CommandType = CommandType.Text;
                    cmdArea.Parameters.AddWithValue("@parkirUserId", idInput);
                    cmdArea.ExecuteNonQuery();

                    SqlCommand cmdUser = new SqlCommand("UPDATE user_parkir SET status_user = 0, waktu_keluar = @userWaktuKeluar, tarif = @userTarif WHERE id_user = @userId", con);
                    cmdUser.CommandType = CommandType.Text;
                    cmdUser.Parameters.AddWithValue("@userId", idInput);
                    cmdUser.Parameters.AddWithValue("@userTarif", hitungTarif(userWaktuMasuk, userWaktuKeluar));
                    cmdUser.Parameters.AddWithValue("@userWaktuKeluar", userWaktuKeluar);
                    cmdUser.ExecuteNonQuery();

                    SqlCommand cmdLogKeluar = new SqlCommand("INSERT INTO log_keluar(id_user, posisi_parkir, waktu_keluar, tarif_parkir) VALUES (@keluarUserId, @keluarPosisiParkir, @keluarWaktuKeluar, @keluarTarif)", con);
                    cmdLogKeluar.CommandType = CommandType.Text;
                    cmdLogKeluar.Parameters.AddWithValue("@keluarUserId", idInput);
                    cmdLogKeluar.Parameters.AddWithValue("@keluarPosisiParkir", userPosisi);
                    cmdLogKeluar.Parameters.AddWithValue("@keluarTarif", hitungTarif(userWaktuMasuk, userWaktuKeluar));
                    cmdLogKeluar.Parameters.AddWithValue("@keluarWaktuKeluar", userWaktuKeluar);
                    cmdLogKeluar.ExecuteNonQuery();

                    showIdUser = userId;
                    showWaktuMasuk = userWaktuMasuk;
                    showWaktuKeluar = userWaktuKeluar;
                    showPosisi = userPosisi;
                    showTarif = Convert.ToInt32(hitungTarif(userWaktuMasuk, userWaktuKeluar));

                    FormUserKeluarNotif frmUserKeluarNotif = new FormUserKeluarNotif();
                    frmUserKeluarNotif.Show();

                    con.Close();
                    break;
                }
                con.Close();
            }
        }

        private string hitungTarif(string waktuMasuk, string waktuKeluar)
        {
            DateTime wm = DateTime.Parse(waktuMasuk);
            DateTime wk = DateTime.Parse(waktuKeluar);
            var diff = wk.Subtract(wm);

            double lamaParkir = Convert.ToDouble(diff.TotalHours);

            double biayaParkir = lamaParkir * 1000;

            if (lamaParkir <= 0.16) //0.16 adalah 1/6 jam, maksudnya 10 menit
            {
                biayaParkir = 0;
            }
            else if (lamaParkir >= 24)
            {
                biayaParkir = 25000;
            }

            int biayaParkirBulat = Convert.ToInt32(Math.Round(biayaParkir / 1000) * 1000);


            return biayaParkirBulat.ToString();
        }

        private void txtId_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Hide();
            var frmUser = new FormUser();
            frmUser.ShowDialog();
            this.Close();
        }
    }
}
