using System;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;

namespace ParkingAreaSystem
{
    public partial class FormUser : Form
    {
        public FormUser()
        {
            InitializeComponent();
        }

        SqlConnection con = new SqlConnection(@"Data Source=LAPTOP-TMDMITR7;Initial Catalog=parking_area;Integrated Security=True");

        public static string showIdUser;
        public static string showPosisiParkir;
        public static string showWaktuMasuk;

        private void btnMasuk_Click(object sender, EventArgs e)
        {
            DataTable dtArea = new DataTable();
            SqlCommand dataArea = new SqlCommand("SELECT * FROM area_parkir", con);

            DataTable dtPengguna = new DataTable();
            SqlCommand dataPengguna = new SqlCommand("SELECT TOP 1 * FROM user_parkir ORDER BY id_user DESC", con);

            con.Open();

            SqlDataReader sdrArea = dataArea.ExecuteReader();
            dtArea.Load(sdrArea);

            SqlDataReader sdrPengguna = dataPengguna.ExecuteReader();
            dtPengguna.Load(sdrPengguna);

            con.Close();

            int userId;

            DateTime currentTime = DateTime.Now;
            string waktuMasuk = Convert.ToString(currentTime);
            
            string parkirPosisi;
            int parkirStatus;

            for (int i = 1; ; i++)
            {
                if (i > 15)
                {
                    MessageBox.Show("Tempat parkir penuh!", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                userId = Convert.ToInt32(dtPengguna.Rows[0]["id_user"].ToString()) + 1;

                parkirStatus = Convert.ToInt32(dtArea.Rows[i - 1]["status_parkir"].ToString());
                parkirPosisi = Convert.ToString(dtArea.Rows[i - 1]["posisi_parkir"].ToString());


                if (parkirStatus == 0)
                {
                    SqlCommand cmdArea = new SqlCommand("UPDATE area_parkir SET id_user = @parkirUserId, status_parkir = 1 WHERE id_parkir = @parkirId", con);
                    cmdArea.CommandType = CommandType.Text;
                    cmdArea.Parameters.AddWithValue("@parkirId", i);
                    cmdArea.Parameters.AddWithValue("@parkirUserId", userId);

                    con.Open();
                    cmdArea.ExecuteNonQuery();
                    con.Close();

                    SqlCommand cmdUser = new SqlCommand("INSERT INTO user_parkir(id_user, posisi_parkir, status_user, waktu_masuk) VALUES (@userId, @userPosisiParkir, 1, @userWaktuMasuk)", con);
                    cmdUser.CommandType = CommandType.Text;
                    cmdUser.Parameters.AddWithValue("@userId", userId);
                    cmdUser.Parameters.AddWithValue("@userPosisiParkir", parkirPosisi);
                    cmdUser.Parameters.AddWithValue("@userWaktuMasuk", waktuMasuk);

                    showIdUser = userId.ToString();
                    showPosisiParkir = parkirPosisi;
                    showWaktuMasuk = waktuMasuk;

                    con.Open();
                    cmdUser.ExecuteNonQuery();
                    con.Close();

                    SqlCommand cmdLogMasuk = new SqlCommand("INSERT INTO log_masuk(id_user, posisi_parkir, waktu_masuk) VALUES (@masukUserId, @masukPosisiParkir, @masukWaktuMasuk)", con);
                    cmdLogMasuk.CommandType = CommandType.Text;
                    cmdLogMasuk.Parameters.AddWithValue("@masukUserId", userId);
                    cmdLogMasuk.Parameters.AddWithValue("@masukPosisiParkir", parkirPosisi);
                    cmdLogMasuk.Parameters.AddWithValue("@masukWaktuMasuk", waktuMasuk);

                    con.Open();
                    cmdLogMasuk.ExecuteNonQuery();
                    con.Close();

                    FormUserMasuk frmUserMasuk = new FormUserMasuk();
                    frmUserMasuk.Show();

                    break;
                }
            }
        }

        private void btnKeluar_Click(object sender, EventArgs e)
        {
            this.Hide();
            var frmUserKeluar = new FormUserKeluar();
            frmUserKeluar.ShowDialog();
            this.Close();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Hide();
            var frmLogin = new FormLogin();
            frmLogin.ShowDialog();
            this.Close();
        }
    }
}
