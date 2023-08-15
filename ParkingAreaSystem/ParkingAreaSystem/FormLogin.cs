using System;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;

namespace ParkingAreaSystem
{
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
        }

        private void btnAdmin_Click(object sender, EventArgs e)
        {
            this.Hide();
            var frmAdmin = new FormAdmin();
            frmAdmin.ShowDialog();
            this.Close();
        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            this.Hide();
            var frmUser = new FormUser();
            frmUser.ShowDialog();
            this.Close();
        }
    }
}
