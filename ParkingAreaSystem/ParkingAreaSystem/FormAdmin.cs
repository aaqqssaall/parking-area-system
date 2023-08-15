using System;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;

namespace ParkingAreaSystem
{
    public partial class FormAdmin : Form
    {
        public FormAdmin()
        {
            InitializeComponent();
            cbUrutan.Text = "Ascending";
            cbUrutanKeluar.Text = "Ascending";
        }

        private void FormAdmin_Load(object sender, EventArgs e)
        {
            hideCbUrutan();
            hideCbUrutanKeluar();
            dataGridSource.AutoResizeColumns();
            GetInfoParkir();
        }

        SqlConnection con = new SqlConnection(@"Data Source=LAPTOP-TMDMITR7;Initial Catalog=parking_area;Integrated Security=True");

        private void btnInfoAreaParkir_Click(object sender, EventArgs e)
        {
            hideCbUrutan();
            hideCbUrutanKeluar();
            dataGridClear();
            GetInfoParkir();
            //this.Hide();
            //var frmAdminInfoParkir = new FormAdminInfoParkir();
            //frmAdminInfoParkir.ShowDialog();
            //this.Close();
        }

        private void btnInfoUser_Click(object sender, EventArgs e)
        {
            hideCbUrutan();
            hideCbUrutanKeluar();
            dataGridClear();
            GetInfoUser();
        }

        private void btnLogMasuk_Click(object sender, EventArgs e)
        {
            hideCbUrutanKeluar();
            showCbUrutan();
            dataGridClear();
            GetLogMasukAsc();
        }

        private void btnLogKeluar_Click(object sender, EventArgs e)
        {
            hideCbUrutan();
            showCbUrutanKeluar();
            dataGridClear();
            GetLogKeluarAsc();
            //this.Hide();
            //var frmLogKeluar = new FormAdminLogKeluar();
            //frmLogKeluar.ShowDialog();
            //this.Close();
        }

        private void btnBatalParkir_Click(object sender, EventArgs e)
        {
            hideCbUrutan();
            hideCbUrutanKeluar();
            dataGridClear();
            this.Hide();
            var frmBatalParkir = new FormAdminBatalParkir();
            frmBatalParkir.ShowDialog();
            this.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Hide();
            var frmLogin = new FormLogin();
            frmLogin.ShowDialog();
            this.Close();
        }

        private void showCbUrutan()
        {
            lblUrutan.Visible = true;
            cbUrutan.Visible = true;
        }

        private void hideCbUrutan()
        {
            lblUrutan.Visible = false;
            cbUrutan.Visible = false;
        }

        private void showCbUrutanKeluar()
        {
            lblUrutan.Visible = true;
            cbUrutanKeluar.Visible = true;
        }

        private void hideCbUrutanKeluar()
        {
            lblUrutan.Visible = false;
            cbUrutanKeluar.Visible = false;
        }

        private void dataGridClear()
        {
            dataGridSource.DataSource = null;
        }

        private void cbUrutan_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (cbUrutan.SelectedIndex == 0)
            {
                dataGridClear();
                GetLogMasukAsc();
            }
            else
            {
                dataGridClear();
                GetLogMasukDesc();
            }
        }
        private void cbUrutanKeluar_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbUrutanKeluar.SelectedIndex == 0)
            {
                dataGridClear();
                GetLogKeluarAsc();
            }
            else
            {
                dataGridClear();
                GetLogKeluarDesc();
            }
        }
        private void GetInfoParkir()
        {
            SqlCommand cmd = new SqlCommand("SELECT posisi_parkir, status_parkir, id_user FROM area_parkir", con);
            DataTable dt = new DataTable();

            con.Open();

            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);

            con.Close();

            dataGridSource.DataSource = dt;
        }

        private void GetInfoUser()
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM user_parkir", con);
            DataTable dt = new DataTable();

            con.Open();

            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);

            con.Close();

            dataGridSource.DataSource = dt;
        }
        private void GetLogMasukAsc()
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM log_masuk ORDER BY id_user asc", con);
            DataTable dt = new DataTable();

            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);

            con.Close();

            dataGridSource.DataSource = dt;
        }

        private void GetLogMasukDesc()
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM log_masuk ORDER BY id_user desc", con);
            DataTable dt = new DataTable();

            con.Open();

            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);

            con.Close();

            dataGridSource.DataSource = dt;
        }
        private void GetLogKeluarAsc()
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM log_keluar ORDER BY id_user asc", con);
            DataTable dt = new DataTable();

            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);

            con.Close();

            dataGridSource.DataSource = dt;
        }

        private void GetLogKeluarDesc()
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM log_keluar ORDER BY id_user desc", con);
            DataTable dt = new DataTable();

            con.Open();

            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);

            con.Close();

            dataGridSource.DataSource = dt;
        }
    }
}
