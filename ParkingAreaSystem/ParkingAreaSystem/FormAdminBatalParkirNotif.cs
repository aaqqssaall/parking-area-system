using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ParkingAreaSystem
{
    public partial class FormAdminBatalParkirNotif : Form
    {
        public FormAdminBatalParkirNotif()
        {
            InitializeComponent();
        }

        private void FormAdminBatalParkirNotif_Load(object sender, EventArgs e)
        {
            lblId.Text = FormAdminBatalParkir.showIdUser.ToString();
            lblPosisi.Text = FormAdminBatalParkir.showPosisi;
            lblWaktuMasuk.Text = FormAdminBatalParkir.showWaktuMasuk;
            lblWaktuKeluar.Text = FormAdminBatalParkir.showWaktuKeluar;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
