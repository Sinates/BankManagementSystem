using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BankManagementSystem
{
    public partial class Transaction : Form
    {
        public Transaction()
        {
            InitializeComponent();
        }
        public int From, To,AccountNo;
        public double Amount;
        public string Type;
        public string FirstName, LastName, Status;
        public DateTime RepaymentDate;

        private void Transaction_Load(object sender, EventArgs e)
        {
            gbLoan.Enabled = false;
            gbTransaction.Enabled = false;
        }

        private void btnEnter_Click(object sender, EventArgs e)
        {
            try
            {
                Type = txtType.Text;
                if (Type == "Deposit")
                {
                    To = int.Parse(txtTo.Text);
                    Amount = double.Parse(txtAmount.Text);
                    DBLayer db = new DBLayer();
                    db.DepositMoney(this);
                    Clear();
                    txtType.Text = "Deposit";
                }
                else if (Type == "Withdraw")
                {
                    From = int.Parse(txtFrom.Text);
                    Amount = double.Parse(txtAmount.Text);
                    DBLayer db = new DBLayer();
                    db.WithdrawMoney(this);
                    Clear();
                    txtType.Text = "Withdraw";
                }
                else if (Type == "Transfer")
                {
                    To = int.Parse(txtTo.Text);
                    From = int.Parse(txtFrom.Text);
                    Amount = double.Parse(txtAmount.Text);
                    DBLayer db = new DBLayer();
                    db.TransferMoney(this);
                    Clear();
                    txtType.Text = "Transfer";
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Invalid Input");
            }
            
            
        }

        private void btnWithdraw_Click(object sender, EventArgs e)
        {
            Clear();
            gbLoan.Enabled = false;
            gbTransaction.Enabled = true;
            txtTo.Enabled = false;
            txtFrom.Enabled = true;
            txtType.Enabled = false;
            txtType.Text = "Withdraw";         
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            Employee ep = new Employee();
            ep.Visible = true;
        }

        private void btnSignOut_Click(object sender, EventArgs e)
        {
            this.Close();
            SignIn signIn = new SignIn();
            signIn.Visible = true;
        }

        private void btnTransfer_Click(object sender, EventArgs e)
        {
            Clear();
            gbLoan.Enabled = false;
            gbTransaction.Enabled = true;
            txtTo.Enabled = true;
            txtFrom.Enabled = true;
            txtType.Enabled = false;
            txtType.Text = "Transfer";           
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void btnRequestLoan_Click(object sender, EventArgs e)
        {
            Clear();
            gbTransaction.Enabled = false;
            gbLoan.Enabled = true;
            txtStatus.Enabled = false;
            txtStatus.Text = "Pending";            
        }

        private void btnRequest_Click(object sender, EventArgs e)
        {
            try
            {
                FirstName = txtFirstName.Text;
                LastName = txtLastName.Text;
                Amount = double.Parse(txtLoanAmount.Text);
                RepaymentDate = dtpRepayment.Value;
                AccountNo = int.Parse(txtAccountNo.Text);
                Status = txtStatus.Text;

                DBLayer db = new DBLayer();
                db.RequestLone(this);
                Clear();
                txtStatus.Text = "Pending";
            }
            catch(Exception ex)
            {
                MessageBox.Show("Invalid Input");
            }
        }

        private void btnDeposit_Click(object sender, EventArgs e)
        {
            Clear();
            gbLoan.Enabled = false;
            gbTransaction.Enabled = true;
            txtFrom.Enabled = false;
            txtType.Enabled = false;
            txtTo.Enabled = true;
            txtType.Text = "Deposit";          
        }
        public void Clear()
        {
            foreach(Control c in gbLoan.Controls)
            {
                if (c is TextBox)
                    c.Text = "";
            }
            foreach (Control c in gbTransaction.Controls)
            {
                if (c is TextBox)
                    c.Text = "";
            }

        }
    }
}
