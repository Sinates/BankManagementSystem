using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BankManagementSystem
{
    public partial class Customer : Form
    {
        public Customer()
        {
            InitializeComponent();
            DBLayer db = new DBLayer();
            DataTable dt = db.GetCustomerInfo();
            dgvCustomerInfo.DataSource = dt;
        }

        public Image ByteArrayToImage(byte[] byteArray)
        {
            Image returnimage = null;
            try
            {
                MemoryStream ms = new MemoryStream(byteArray, 0, byteArray.Length);
                ms.Write(byteArray, 0, byteArray.Length);
                returnimage = Image.FromStream(ms, true);
            }
            catch { }
            return returnimage;
        }

        private void Customer_Load(object sender, EventArgs e)
        {
            
            lblFullName.Text = dgvCustomerInfo.CurrentRow.Cells[1].Value.ToString()+" "+ dgvCustomerInfo.CurrentRow.Cells[2].Value.ToString();
            lblAccountNo.Text = dgvCustomerInfo.CurrentRow.Cells[0].Value.ToString();
            lblBalance.Text = dgvCustomerInfo.CurrentRow.Cells[5].Value.ToString();
            lblUsername.Text = dgvCustomerInfo.CurrentRow.Cells[6].Value.ToString();
            lblPassword.Text = dgvCustomerInfo.CurrentRow.Cells[7].Value.ToString();
            Image photo = ByteArrayToImage((byte[])dgvCustomerInfo.CurrentRow.Cells[9].Value);
            pictureBox1.BackgroundImage = photo;
            dgvCustomerInfo.Visible = false ;
        }

        private void btnMessege_Click(object sender, EventArgs e)
        {
            DBLayer db = new DBLayer();
            DataTable dt = db.GetMessege();
            dataGridView1.DataSource = dt;
            dataGridView1.Columns[0].Width = 345;
        }

        private void btnSignOut_Click(object sender, EventArgs e)
        {
            SignIn signIn = new SignIn();
            this.Close();
            signIn.Visible = true;
        }
    }
}
