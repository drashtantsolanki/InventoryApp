using Inventory.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Inventory.DAL.ExceptionLogging;

namespace Inventory.DAL
{
    public class UsersRepository : IUserRepository
    {
        private string connectionString = ConnectionString.Value;
        //private string connectionString = "";

        #region LOGIN
        public DataTable Login(string username, string password)
        {
            try
            {
                password = Encryption(password);
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_login", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", password);
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataSet dataSet = new DataSet();
                        con.Open();
                        int result = adapter.Fill(dataSet);
                        con.Close();
                        if (result > 0 && dataSet.Tables[0].Rows.Count > 0)
                        {
                            return dataSet.Tables[0];
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region Registration
        public bool Registration(string username, string email, string password, int createdBy = 0)
        {
            try
            {
                string encPassword = Encryption(password);
                username = username.ToLower();
                email = email.ToLower();
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("sp_registration", con))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@password", encPassword);
                        command.Parameters.AddWithValue("@createdby", createdBy);
                        con.Open();
                        int result = command.ExecuteNonQuery();
                        con.Close();
                        if (result > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.ExceptionLogging.WriteException(ex);
                throw;
            }
        }
        #endregion

        #region GetAllUsers
        public DataTable GetUsers(int deleteFlag = 0)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("sp_getAllUsers", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@deleteFlag", deleteFlag);
                        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command);
                        DataSet dataSet = new DataSet();
                        connection.Open();
                        int result = sqlDataAdapter.Fill(dataSet);
                        connection.Close();
                        if (result > 0 && dataSet.Tables[0].Rows.Count > 0)
                        {
                            return dataSet.Tables[0];
                        }
                        else
                        {
                            return null;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.ExceptionLogging.WriteException(ex);
                throw;
            }

        } 
        #endregion

        #region Private Methods
        private string Encryption(string password)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] passBytes = Encoding.UTF8.GetBytes(password);
            passBytes = md5.ComputeHash(passBytes);
            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte ba in passBytes)
            {
                stringBuilder.Append(ba.ToString());
            }
            return stringBuilder.ToString();
        } 
        #endregion
    }
}
