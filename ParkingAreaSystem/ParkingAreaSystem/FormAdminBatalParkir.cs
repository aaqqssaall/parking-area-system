using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ParkingAreaSystem
{
    public partial class FormAdminBatalParkir : Form
    {
        public FormAdminBatalParkir()
        {
            InitializeComponent();
        }

        public static int showIdUser;
        public static string showPosisi;
        public static string showWaktuMasuk;
        public static string showWaktuKeluar;

        SqlConnection con = new SqlConnection(@"Data Source=LAPTOP-TMDMITR7;Initial Catalog=parking_area;Integrated Security=True");

        private void btnBatal_Click(object sender, EventArgs e)
        {
            if (txtId.Text.Trim() == string.Empty)
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

            for (int i = 1; i <= jmlRow; i++)
            {
                con.Open();
                if (jmlRow == 1 || i == jmlRow + 1 || idInput > Convert.ToInt32(dtPengguna.Rows[jmlRow - 1]["id_user"].ToString()) || idInput < Convert.ToInt32(dtPengguna.Rows[1]["id_user"].ToString()))
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
                    cmdUser.Parameters.AddWithValue("@userTarif", 0);
                    cmdUser.Parameters.AddWithValue("@userWaktuKeluar", userWaktuKeluar);
                    cmdUser.ExecuteNonQuery();

                    SqlCommand cmdLogKeluar = new SqlCommand("INSERT INTO log_keluar(id_user, posisi_parkir, waktu_keluar, tarif_parkir) VALUES (@keluarUserId, @keluarPosisiParkir, @keluarWaktuKeluar, @keluarTarif)", con);
                    cmdLogKeluar.CommandType = CommandType.Text;
                    cmdLogKeluar.Parameters.AddWithValue("@keluarUserId", idInput);
                    cmdLogKeluar.Parameters.AddWithValue("@keluarPosisiParkir", userPosisi);
                    cmdLogKeluar.Parameters.AddWithValue("@keluarTarif", 0);
                    cmdLogKeluar.Parameters.AddWithValue("@keluarWaktuKeluar", userWaktuKeluar);
                    cmdLogKeluar.ExecuteNonQuery();

                    showIdUser = userId;
                    showWaktuMasuk = userWaktuMasuk;
                    showWaktuKeluar = userWaktuKeluar;
                    showPosisi = userPosisi;

                    FormAdminBatalParkirNotif frmAdminBatalParkirNotif = new FormAdminBatalParkirNotif();
                    frmAdminBatalParkirNotif.Show();

                    con.Close();
                    break;
                }
                con.Close();
            }
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
            var frmAdmin = new FormAdmin();
            frmAdmin.ShowDialog();
            this.Close();
        }

        private void txtId_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }
    }
}
