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
    public partial class SignIn : Form
    {
        public SignIn()
        {
            InitializeComponent();
        }

        

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSignIn_Click(object sender, EventArgs e)
        {           
                try
                {
                    Sign_In_Info.Role = cmbRole.SelectedItem.ToString();
                    Sign_In_Info.UserName = txtUsername.Text;
                    Sign_In_Info.Password = txtPassword.Text;

                    DBLayer dB = new DBLayer();
                    if (Sign_In_Info.Role == "Manager")
                    {
                        string status = dB.VerifyManager();
                        if (status == "successful")
                        {
                            Manager m = new Manager();
                            m.Visible = true;
                            this.Visible = false;
                        }
                        else
                        {
                            MessageBox.Show("Invalid Username or Password");
                        }
                    }
                    else if (Sign_In_Info.Role == "Employee")
                    {
                        string status = dB.VerifyEmployee();
                        if (status == "Successful")
                        {
                            Employee ep = new Employee();
                            ep.Visible = true;
                            this.Visible = false;
                        }
                        else
                        {
                            MessageBox.Show("Invalid Username or Password");
                        }
                    }
                    else if (Sign_In_Info.Role == "Customer")
                    {
                        string status = dB.VerifyCustomer();
                        if (status == "Successful")
                        {
                            Customer c = new Customer();
                            c.Visible = true;
                            this.Visible = false;
                        }
                        else
                        {
                            MessageBox.Show("Invalid Username or Password");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }


            
        }
    }
}
