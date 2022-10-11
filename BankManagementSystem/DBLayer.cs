using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BankManagementSystem
{
    class DBLayer
    {
        string ConnString;
        public DBLayer()
        {
            if(Sign_In_Info.Role == "Manager")
            {
                ConnString = "Server=DESKTOP-V3EG8JO;database=BankManagementSystem;uid=" + Sign_In_Info.UserName + ";" + "pwd=" + Sign_In_Info.Password + ";";
            }
            else if (Sign_In_Info.Role == "Employee")
            {
                ConnString = "Server=DESKTOP-V3EG8JO;database=BankManagementSystem;uid=" + "Employee" + ";" + "pwd=" + "Employee" + ";";
            }
            else if (Sign_In_Info.Role == "Customer")
            {
                ConnString = "Server=DESKTOP-V3EG8JO;database=BankManagementSystem;uid=" + "Customer" + ";" + "pwd=" + "Customer" + ";";
            }
        }
        SqlConnection Connection()
        {
            try
            {
                SqlConnection conn = new SqlConnection(ConnString);
                conn.Open();
                return conn;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
        public string VerifyManager()
        {
            string status = "successful";
            try
            {
                SqlConnection conn = Connection();
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                return status;
            }
            catch (Exception e)
            {
                status = "unsuccessful";
                return status;
            }
        }
        public string VerifyEmployee()
        {
            string status=null;
            SqlConnection conn = Connection();
            try
            {
                SqlCommand cmd = new SqlCommand("sp_VerifyEmployee", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Username", Sign_In_Info.UserName);
                cmd.Parameters.AddWithValue("@Password", Sign_In_Info.Password);
                status = (string)cmd.ExecuteScalar();
                return status;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return status;
            }
            finally
            {
                conn.Close();
            }
        }
        public string VerifyCustomer()
        {
            string status = null;
            SqlConnection conn = Connection();
            try
            {
                SqlCommand cmd = new SqlCommand("sp_VerifyCustomer", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Username", Sign_In_Info.UserName);
                cmd.Parameters.AddWithValue("@Password", Sign_In_Info.Password);
                status = (string)cmd.ExecuteScalar();
                return status;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return status;
            }
            finally
            {
                conn.Close();
            }
        }

        public void AddEmployee(Manager m)
        {
            SqlConnection conn = Connection();
            try
            {
                SqlCommand cmd = new SqlCommand("sp_InsertIntoEmployee", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FirstName", m.FirstName);
                cmd.Parameters.AddWithValue("@LastName", m.LastName);
                cmd.Parameters.AddWithValue("@DOB", m.DOB);
                cmd.Parameters.AddWithValue("@Phone", m.Phone);
                cmd.Parameters.AddWithValue("@Username", m.Username);
                cmd.Parameters.AddWithValue("@Password", m.Password);
                cmd.Parameters.AddWithValue("@Photo", m.Photo);

                int row = cmd.ExecuteNonQuery();
                if (row > 0)
                    MessageBox.Show("Employee Added Sucessfully");
                else
                    MessageBox.Show("Unable to Add Employee");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        public DataTable SearchEmployee(Manager m)
        {
            SqlConnection conn = Connection();
            try
            {
                using (SqlDataAdapter da = new SqlDataAdapter())
                {
                    da.SelectCommand = new SqlCommand("sp_SearchEmployee", conn);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@Target", m.target);
                    DataSet ds = new DataSet();
                    da.Fill(ds, "dtUsers");
                    DataTable dt = ds.Tables["dtUsers"];
                    return dt;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

        public void UpdateEmployee(Manager m)
        {
            SqlConnection conn = Connection();
            try
            {
                SqlCommand cmd = new SqlCommand("sp_UpdateEmployee", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmployeeID", m.EmployeeID);
                cmd.Parameters.AddWithValue("@FirstName", m.FirstName);
                cmd.Parameters.AddWithValue("@LastName", m.LastName);
                cmd.Parameters.AddWithValue("@DOB", m.DOB);
                cmd.Parameters.AddWithValue("@Phone", m.Phone);
                cmd.Parameters.AddWithValue("@Username", m.Username);
                cmd.Parameters.AddWithValue("@Password", m.Password);
                cmd.Parameters.AddWithValue("@Photo", m.Photo);

                int row = cmd.ExecuteNonQuery();
                if (row > 0)
                    MessageBox.Show("Employee Updated Sucessfully");
                else
                    MessageBox.Show("Unable to Update Employee");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        public void DeleteEmployee(Manager m)
        {
            SqlConnection conn = Connection();
            try
            {
                SqlCommand cmd = new SqlCommand("sp_DeleteEmployee", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmployeeID", m.EmployeeID);

                int row = cmd.ExecuteNonQuery();
                if (row > 0)
                    MessageBox.Show("Employee Deleted Sucessfully");
                else
                    MessageBox.Show("Unable to Delete Employee");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        public DataTable DisplayEmployee()
        {
            SqlConnection conn = Connection();
            try
            {
                using (SqlDataAdapter da = new SqlDataAdapter())
                {
                    da.SelectCommand = new SqlCommand("sp_DisplayEmployee", conn);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    DataSet ds = new DataSet();
                    da.Fill(ds, "dtUsers");
                    DataTable dt = ds.Tables["dtUsers"];
                    return dt;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
            finally
            {
                conn.Close();
            }
        }


        public void CreateCustomerAccount(Employee e)
        {
            SqlConnection conn = Connection();
            try
            {
                SqlCommand cmd = new SqlCommand("sp_InsertIntoCustomerAccount", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FirstName", e.FirstName);
                cmd.Parameters.AddWithValue("@LastName", e.LastName);
                cmd.Parameters.AddWithValue("@DOB", e.DOB);
                cmd.Parameters.AddWithValue("@Phone", e.Phone);
                cmd.Parameters.AddWithValue("@Balance", e.Balance);
                cmd.Parameters.AddWithValue("@Username", e.Username);
                cmd.Parameters.AddWithValue("@Password", e.Password);
                cmd.Parameters.AddWithValue("@CreatedBy", e.CreatedBy);
                cmd.Parameters.AddWithValue("@Photo", e.Photo);

                int row = cmd.ExecuteNonQuery();
                if (row > 0)
                    MessageBox.Show("Customer Added Sucessfully");
                else
                    MessageBox.Show("Unable to Add Customer");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        public DataTable SearchCustomer(Employee e)
        {
            SqlConnection conn = Connection();
            try
            {
                using (SqlDataAdapter da = new SqlDataAdapter())
                {
                    da.SelectCommand = new SqlCommand("sp_SearchCustomer", conn);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@Target", e.target);
                    DataSet ds = new DataSet();
                    da.Fill(ds, "dtUsers");
                    DataTable dt = ds.Tables["dtUsers"];
                    return dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
            finally
            {
                conn.Close();
            }
        }
        public void UpdateCustomerAccount(Employee e)
        {
            SqlConnection conn = Connection();
            try
            {
                SqlCommand cmd = new SqlCommand("sp_UpdateCustomerAccount", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AccountNo", e.AccountNo);
                cmd.Parameters.AddWithValue("@FirstName", e.FirstName);
                cmd.Parameters.AddWithValue("@LastName", e.LastName);
                cmd.Parameters.AddWithValue("@DOB", e.DOB);
                cmd.Parameters.AddWithValue("@Phone", e.Phone);
                cmd.Parameters.AddWithValue("@Balance", e.Balance);
                cmd.Parameters.AddWithValue("@Username", e.Username);
                cmd.Parameters.AddWithValue("@Password", e.Password);
                cmd.Parameters.AddWithValue("@Photo", e.Photo);

                int row = cmd.ExecuteNonQuery();
                if (row > 0)
                    MessageBox.Show("Customer Updated Sucessfully");
                else
                    MessageBox.Show("Unable to Update Customer");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        public void DeleteCustomer(Employee e)
        {
            SqlConnection conn = Connection();
            try
            {
                SqlCommand cmd = new SqlCommand("sp_DeleteCustomer", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AccountNo", e.AccountNo);

                int row = cmd.ExecuteNonQuery();
                if (row > 0)
                    MessageBox.Show("Customer Deleted Sucessfully");
                else
                    MessageBox.Show("Unable to Delete Customer");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        public void DepositMoney(Transaction t)
        {
            SqlConnection conn = Connection();
            try
            {
                SqlCommand cmd = new SqlCommand("sp_DepositMoney", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@To", t.To);
                cmd.Parameters.AddWithValue("@Amount", t.Amount);
                string status = (string)cmd.ExecuteScalar();
                if (status == "Successful")
                    MessageBox.Show("Money Deposited Sucessfully");
                else
                    MessageBox.Show("Unable to Deposit Money");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        public void WithdrawMoney(Transaction t)
        {
            SqlConnection conn = Connection();
            try
            {
                SqlCommand cmd = new SqlCommand("sp_WithdrawMoney", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@From", t.From);
                cmd.Parameters.AddWithValue("@Amount", t.Amount);
                string status = (string)cmd.ExecuteScalar();
                if (status == "Successful")
                    MessageBox.Show("Money Withdrawed Sucessfully");
                else
                    MessageBox.Show("Unable to Withdraw Money");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        public void TransferMoney(Transaction t)
        {
            SqlConnection conn = Connection();
            try
            {
                SqlCommand cmd = new SqlCommand("sp_TransferMoney", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@From", t.From);
                cmd.Parameters.AddWithValue("@To", t.To);
                cmd.Parameters.AddWithValue("@Amount", t.Amount);
                string status = (string)cmd.ExecuteScalar();
                if (status == "Successful")
                    MessageBox.Show("Money Transferred Sucessfully");
                else
                    MessageBox.Show("Unable to Transfer Money");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        public void RequestLone(Transaction t)
        {
            SqlConnection conn = Connection();
            try
            {
                SqlCommand cmd = new SqlCommand("sp_InsertIntoLoan", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FirstName", t.FirstName);
                cmd.Parameters.AddWithValue("@LastName", t.LastName);
                cmd.Parameters.AddWithValue("@Amount", t.Amount);
                cmd.Parameters.AddWithValue("@RepaymentDate", t.RepaymentDate);
                cmd.Parameters.AddWithValue("@AccountNo", t.AccountNo);
                cmd.Parameters.AddWithValue("@Status", t.Status);
                int row = cmd.ExecuteNonQuery();
                string status = (string)cmd.ExecuteScalar();
                if (row > 0)
                    MessageBox.Show("Lone Requested Sucessfully");
                else if(status == "Unsuccessful")
                    MessageBox.Show("The Customer Has Unpaid Lone");
                else
                    MessageBox.Show("Unable to Request Lone");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Invalid Account No");
            }
            finally
            {
                conn.Close();
            }
        }
        public DataTable GetPendingLoans()
        {
            SqlConnection conn = Connection();
            try
            {
                using (SqlDataAdapter da = new SqlDataAdapter())
                {
                    da.SelectCommand = new SqlCommand("sp_GetPendingLoans", conn);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    DataSet ds = new DataSet();
                    da.Fill(ds, "dtUsers");
                    DataTable dt = ds.Tables["dtUsers"];
                    return dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
            finally
            {
                conn.Close();
            }
        }
        public void ApproveLoan(Manager m)
        {
            SqlConnection conn = Connection();
            try
            {
                SqlCommand cmd = new SqlCommand("sp_ApproveLoan", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@LoanID", m.LoanID);
                int row = cmd.ExecuteNonQuery();
                if (row > 0)
                    MessageBox.Show("Lone Approved Sucessfully");
                else
                    MessageBox.Show("Unable to Approve Lone");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        public void DenyLoan(Manager m)
        {
            SqlConnection conn = Connection();
            try
            {
                SqlCommand cmd = new SqlCommand("sp_DenyLoan", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@LoanID", m.LoanID);
                int row = cmd.ExecuteNonQuery();
                if (row > 0)
                    MessageBox.Show("Lone Denied Sucessfully");
                else
                    MessageBox.Show("Unable to Deny Lone");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        public DataTable GetCustomerInfo()
        {
            SqlConnection conn = Connection();
            try
            {
                using (SqlDataAdapter da = new SqlDataAdapter())
                {
                    da.SelectCommand = new SqlCommand("sp_GetCustomerInfo", conn);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@Username", Sign_In_Info.UserName);
                    DataSet ds = new DataSet();
                    da.Fill(ds, "dtUsers");
                    DataTable dt = ds.Tables["dtUsers"];
                    return dt;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

        public DataTable GetMessege()
        {
            SqlConnection conn = Connection();
            try
            {
                using (SqlDataAdapter da = new SqlDataAdapter())
                {
                    da.SelectCommand = new SqlCommand("sp_GetMessege", conn);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@Username", Sign_In_Info.UserName);
                    DataSet ds = new DataSet();
                    da.Fill(ds, "dtUsers");
                    DataTable dt = ds.Tables["dtUsers"];
                    return dt;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
            finally
            {
                conn.Close();
            }
        }
        public DataTable GetDeptors()
        {
            SqlConnection conn = Connection();
            try
            {
                using (SqlDataAdapter da = new SqlDataAdapter())
                {
                    da.SelectCommand = new SqlCommand("sp_GetDeptors", conn);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    DataSet ds = new DataSet();
                    da.Fill(ds, "dtUsers");
                    DataTable dt = ds.Tables["dtUsers"];
                    return dt;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
            finally
            {
                conn.Close();
            }
        }
        public DataTable DisplayTransactions()
        {
            SqlConnection conn = Connection();
            try
            {
                using (SqlDataAdapter da = new SqlDataAdapter())
                {
                    da.SelectCommand = new SqlCommand("sp_DisplayTransactions", conn);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    DataSet ds = new DataSet();
                    da.Fill(ds, "dtUsers");
                    DataTable dt = ds.Tables["dtUsers"];
                    return dt;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
            finally
            {
                conn.Close();
            }
        }





    }
}
