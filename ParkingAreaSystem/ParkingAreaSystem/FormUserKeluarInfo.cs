using System.Data.SqlClient;
using System.Windows.Forms;
using System.Data;

namespace ParkingAreaSystem
{
    public partial class FormUserMasuk : Form
    {
        public FormUserMasuk()
        {
            InitializeComponent();
        }

        private void FormUserMasuk_Load(object sender, System.EventArgs e)
        {
            lblId.Text = FormUser.showIdUser;
            lblPosisiParkir.Text = FormUser.showPosisiParkir;
            lblWaktuMasuk.Text = FormUser.showWaktuMasuk;
        }
    }
}
