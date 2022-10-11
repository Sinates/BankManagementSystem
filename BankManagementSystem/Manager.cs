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
    public partial class Manager : Form
    {
        public Manager()
        {
            InitializeComponent();
        }
        public int EmployeeID,LoanID;
        public string FirstName, LastName, Phone, Username, Password,target;
        public DateTime DOB;
        public byte[] Photo;
        SignIn signIn = new SignIn();

        private void btnSignOut_Click(object sender, EventArgs e)
        {
            SignIn signIn = new SignIn();
            signIn.Visible = true;
            this.Close();
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

                MemoryStream ms = new MemoryStream();
                pbEmployee.BackgroundImage.Save(ms, pbEmployee.BackgroundImage.RawFormat);
                Photo = ms.ToArray();

                DBLayer dB = new DBLayer();
                dB.AddEmployee(this);
                Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "Choose Photo (*.jpg; *.png; *.Jpeg; *.bmp;) | *.jpg; *.png; *.Jpeg; *.bmp;";
            if (op.ShowDialog() == DialogResult.OK)
            {
                pbEmployee.SizeMode = PictureBoxSizeMode.StretchImage;
                pbEmployee.BackgroundImage = Image.FromFile(op.FileName);
            }
        }

        private void btnUpdateEmployee_Click(object sender, EventArgs e)
        {
            Clear();
            panelLone.Visible = false;
            btnDelete.Enabled = false;
            btnAdd.Enabled = false;
            btnSearch.Enabled = true;
            txtSearch.Enabled = true;
            btnClear.Enabled = true;
            btnBrowse.Enabled = true;
            btnUpdate.Enabled = true;     
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


        private void dataGridView1_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if(panelLone.Visible)
            {
                txtLoanID.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            }
            else
            {
                txtEmpID.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                txtFirstName.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                txtLastName.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                dtpDOB.Value = (DateTime)dataGridView1.CurrentRow.Cells[3].Value;
                txtPhone.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
                txtUserName.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();
                txtPassword.Text = dataGridView1.CurrentRow.Cells[7].Value.ToString();
                Image photo = ByteArrayToImage((byte[])dataGridView1.CurrentRow.Cells[9].Value);
                pbEmployee.BackgroundImage = photo;
            }     
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                EmployeeID = int.Parse(txtEmpID.Text);
                FirstName = txtFirstName.Text;
                LastName = txtLastName.Text;
                DOB = dtpDOB.Value;
                Phone = txtPhone.Text;
                Username = txtUserName.Text;
                Password = txtPassword.Text;

                MemoryStream ms = new MemoryStream();
                pbEmployee.BackgroundImage.Save(ms, pbEmployee.BackgroundImage.RawFormat);
                Photo = ms.ToArray();

                DBLayer dB = new DBLayer();
                dB.UpdateEmployee(this);
                Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDeleteEmployee_Click(object sender, EventArgs e)
        {
            Clear();
            panelLone.Visible = false;
            btnDelete.Enabled = true;
            btnAdd.Enabled = false;
            btnSearch.Enabled = true;
            txtSearch.Enabled = true;
            btnClear.Enabled = true;
            btnBrowse.Enabled = false;
            btnUpdate.Enabled = false;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                EmployeeID = int.Parse(txtEmpID.Text);
                DBLayer db = new DBLayer();
                db.DeleteEmployee(this);
                Clear();
            }
            catch(Exception ex)
            {
                MessageBox.Show("No Employee Selected");
            }
        }

        private void btnDeny_Click(object sender, EventArgs e)
        {
            try
            {
                LoanID = int.Parse(txtLoanID.Text);
                DBLayer db = new DBLayer();
                db.DenyLoan(this);
                Clear();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Select Loan First");
            }
           
        }

        private void btnApprove_Click(object sender, EventArgs e)
        {
            try
            {
                LoanID = int.Parse(txtLoanID.Text);
                DBLayer db = new DBLayer();
                db.ApproveLoan(this);
                Clear();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Select Loan first");
            }
            
        }

        private void btnDisplayEmployee_Click(object sender, EventArgs e)
        {
            Clear();
            btnUpdate.Enabled = false;
            btnSearch.Enabled = false;
            txtSearch.Enabled = false;
            btnDelete.Enabled = false;
            panelLone.Visible = false;
            btnAdd.Enabled = false;
            btnClear.Enabled = false;
            btnBrowse.Enabled = false;

            DBLayer dB = new DBLayer();
            DataTable dt = dB.DisplayEmployee();
            dataGridView1.DataSource = dt;
        }

        private void btnApproveOrDenyLone_Click(object sender, EventArgs e)
        {
            Clear();
            btnUpdate.Enabled = false;
            btnSearch.Enabled = false;
            txtSearch.Enabled = false;
            btnDelete.Enabled = false;
            panelLone.Visible = true;
            btnAdd.Enabled = false;
            btnClear.Enabled = false;
            btnBrowse.Enabled = false;

            DBLayer dB = new DBLayer();
            DataTable dt = dB.GetPendingLoans();
            dataGridView1.DataSource = dt;

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            target = txtSearch.Text;
            DBLayer dB = new DBLayer();
            DataTable dt = dB.SearchEmployee(this);
            dataGridView1.DataSource = dt;
        }

   

        private void btnAddEmployee_Click(object sender, EventArgs e)
        {
            Clear();
            btnUpdate.Enabled = false;
            btnSearch.Enabled = false;
            txtSearch.Enabled =false;
            btnDelete.Enabled = false;
            panelLone.Visible = false;
            btnAdd.Enabled = true;
            btnClear.Enabled = true;
            btnBrowse.Enabled = true;           
        }
        public void Clear()
        {
            foreach(Control c in gbEmployeeInfo.Controls)
            {
                if(c is TextBox)
                {
                    c.Text = "";
                }
            }
            foreach (Control c in panelLone.Controls)
            {
                if (c is TextBox)
                {
                    c.Text = "";
                }
            }
            pbEmployee.BackgroundImage = null;
            dataGridView1.DataSource = null;
        }
    }
}
