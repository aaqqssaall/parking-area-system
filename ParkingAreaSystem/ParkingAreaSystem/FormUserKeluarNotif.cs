using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Data;

namespace ParkingAreaSystem
{
    public partial class FormUserKeluarNotif : Form
    {
        public FormUserKeluarNotif()
        {
            InitializeComponent();
        }

        private void FormUserKeluarNotif_Load(object sender, EventArgs e)
        {
            lblId.Text = FormUserKeluar.showIdUser.ToString();
            lblPosisi.Text = FormUserKeluar.showPosisi;
            lblWaktuMasuk.Text = FormUserKeluar.showWaktuMasuk;
            lblWaktuKeluar.Text = FormUserKeluar.showWaktuKeluar;
            lblTarif.Text = FormUserKeluar.showTarif.ToString();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
