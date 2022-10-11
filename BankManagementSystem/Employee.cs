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
    public partial class Employee : Form
    {
        public Employee()
        {
            InitializeComponent();
        }
        public int AccountNo;
        public double Balance;
        public string FirstName, LastName, Phone, Username, Password,CreatedBy, target;
        public DateTime DOB;

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                AccountNo = int.Parse(txtAccountNo.Text);
                FirstName = txtFirstName.Text;
                LastName = txtLastName.Text;
                DOB = dtpDOB.Value;
                Phone = txtPhone.Text;
                Username = txtUserName.Text;
                Password = txtPassword.Text;
                Balance = double.Parse(txtBalance.Text);
                CreatedBy = txtCreatedBy.Text;

                MemoryStream ms = new MemoryStream();
                pbCustomer.BackgroundImage.Save(ms, pbCustomer.BackgroundImage.RawFormat);
                Photo = ms.ToArray();

                DBLayer dB = new DBLayer();
                dB.UpdateCustomerAccount(this);
                Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnUpdateCustomer_Click(object sender, EventArgs e)
        {
            Clear();
            dataGridView1.DataSource = null;
            btnDelete.Enabled = false;
            btnAdd.Enabled = false;
            btnSearch.Enabled = true;
            txtSearch.Enabled = true;
            btnClear.Enabled = true;
            btnBrowse.Enabled = true;
            btnUpdate.Enabled = true;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            target = txtSearch.Text;
            DBLayer dB = new DBLayer();
            DataTable dt = dB.SearchCustomer(this);
            dataGridView1.DataSource = dt;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "Choose Photo (*.jpg; *.png; *.Jpeg; *.bmp;) | *.jpg; *.png; *.Jpeg; *.bmp;";
            if (op.ShowDialog() == DialogResult.OK)
            {
                pbCustomer.SizeMode = PictureBoxSizeMode.StretchImage;
                pbCustomer.BackgroundImage = Image.FromFile(op.FileName);
            }
        }

        private void btnDeleteCustomer_Click(object sender, EventArgs e)
        {
            Clear();
            dataGridView1.DataSource = null;
            btnDelete.Enabled = true;
            btnAdd.Enabled = false;
            btnSearch.Enabled = true;
            txtSearch.Enabled = true;
            btnClear.Enabled = true;
            btnBrowse.Enabled = false;
            btnUpdate.Enabled = false;
        }

        private void dataGridView1_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                txtAccountNo.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                txtFirstName.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                txtLastName.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                dtpDOB.Value = (DateTime)dataGridView1.CurrentRow.Cells[3].Value;
                txtPhone.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
                txtBalance.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
                txtUserName.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();
                txtPassword.Text = dataGridView1.CurrentRow.Cells[7].Value.ToString();
                txtCreatedBy.Text = dataGridView1.CurrentRow.Cells[8].Value.ToString();
                Image photo = ByteArrayToImage((byte[])dataGridView1.CurrentRow.Cells[9].Value);
                pbCustomer.BackgroundImage = photo;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                AccountNo = int.Parse(txtAccountNo.Text);
                DBLayer db = new DBLayer();
                db.DeleteCustomer(this);
                Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No Customer Selected");
            }
        }

        private void btnSignOut_Click(object sender, EventArgs e)
        {
            this.Close();
            SignIn signIn = new SignIn();
            signIn.Visible = true;         
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void btnTransaction_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            Transaction t = new Transaction();
            t.Visible = true;
        }

        private void btnDebtors_Click(object sender, EventArgs e)
        {
            Clear();
            dataGridView1.DataSource = null;
            btnUpdate.Enabled = false;
            btnSearch.Enabled = false;
            txtSearch.Enabled = false;
            btnDelete.Enabled = false;
            btnAdd.Enabled = false;
            btnClear.Enabled = false;
            btnBrowse.Enabled = false;
           
            DBLayer dB = new DBLayer();
            DataTable dt = dB.GetDeptors();
            dataGridView1.DataSource = dt;
        }

        private void btnDisplayTransactions_Click(object sender, EventArgs e)
        {
            Clear();
            dataGridView1.DataSource = null;
            btnUpdate.Enabled = false;
            btnSearch.Enabled = false;
            txtSearch.Enabled = false;
            btnDelete.Enabled = false;
            btnAdd.Enabled = false;
            btnClear.Enabled = false;
            btnBrowse.Enabled = false;

            DBLayer dB = new DBLayer();
            DataTable dt = dB.DisplayTransactions();
            dataGridView1.DataSource = dt;
        }

        private void btnCreateCustomer_Click(object sender, EventArgs e)
        {
            Clear();
            dataGridView1.DataSource = null;
            btnUpdate.Enabled = false;
            btnSearch.Enabled = false;
            txtSearch.Enabled = false;
            btnDelete.Enabled = false;
            btnAdd.Enabled = true;
            btnClear.Enabled = true;
            btnBrowse.Enabled = true;
        }

        public byte[] Photo;

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

        private void Employee_Load(object sender, EventArgs e)
        {
            lblFirstName.Text = Sign_In_Info.UserName;
            txtCreatedBy.Text = Sign_In_Info.UserName;
         
        }


        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                FirstName = txtFirstName.Text;
                LastName = txtLastName.Text;
                DOB = dtpDOB.Value;
                Phone = txtPhone.Text;
                Username = txtUserName.Text;
                Password = txtPassword.Text;
                Balance = double.Parse(txtBalance.Text);
                CreatedBy = txtCreatedBy.Text;

                MemoryStream ms = new MemoryStream();
                pbCustomer.BackgroundImage.Save(ms, pbCustomer.BackgroundImage.RawFormat);
                Photo = ms.ToArray();

                DBLayer dB = new DBLayer();
                dB.CreateCustomerAccount(this);
                Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Clear()
        {
            foreach (Control c in gbEmployeeInfo.Controls)
            {
                if (c is TextBox && c  != txtCreatedBy)
                {
                    c.Text = "";
                }
            }
            pbCustomer.BackgroundImage = null;
            dataGridView1.DataSource = null;
        }







    }
}
