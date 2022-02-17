using Inventory.DAL.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.DAL
{
    public class CountryRepository : ICountryRepository
    {

        #region PrivateProperties
        private readonly string connectionString = ConnectionString.Value;
        #endregion

        #region AddCountry
        public string AddCountry(string countryname, int createdBy)
        {
            try
            {
                if (!String.IsNullOrEmpty(countryname))
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        using (SqlCommand command = new SqlCommand("sp_country_insert", con))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@name", countryname);
                            command.Parameters.AddWithValue("@createdBy", createdBy);
                            con.Open();
                            int result = command.ExecuteNonQuery();
                            con.Close();
                            if (result > 0)
                            {
                                return JsonConvert.SerializeObject(new ApiResponse { status = true, Data = "success", Error = "" });
                            }
                            else
                            {
                                return JsonConvert.SerializeObject(new ApiResponse { status = false, Data = "fail", Error = "Something went wrong." }); ;
                            }
                        }
                    }
                }
                else
                {
                    return JsonConvert.SerializeObject(new ApiResponse { status = false, Data = "", Error = "Country name required" });
                }

            }
            catch (Exception ex)
            {
                ExceptionLogging.ExceptionLogging.WriteException(ex);
                return JsonConvert.SerializeObject(new ApiResponse { status = false, Data = "", Error = ex.Message.ToString() });
                throw;
            }
        }
        #endregion


        #region GetCountries
        public string getCountries(int deleteFlag = 0)
        {

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("sp_country_get", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@deleteflag", deleteFlag);
                        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command);
                        DataSet dataSet = new DataSet();
                        connection.Open();
                        int result = sqlDataAdapter.Fill(dataSet);
                        connection.Close();
                        if (result > 0 && dataSet.Tables[0].Rows.Count > 0)
                        {
                            return JsonConvert.SerializeObject(new ApiResponse { status = true, Data = dataSet.Tables[0], Error = "" });
                        }
                        else
                        {
                            return JsonConvert.SerializeObject(new ApiResponse { status = false, Data = "", Error = "Something went wrong." }); ;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.ExceptionLogging.WriteException(ex);
                return JsonConvert.SerializeObject(new ApiResponse { status = false, Data = "", Error = ex.Message.ToString() }); ;
                throw;
            }
        }
        #endregion


        #region GetCountryById
        public string getCountryById(int countryId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("sp_country_getById", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@countryId", countryId);
                        DataSet dataSet = new DataSet();
                        SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                        connection.Open();
                        int result = dataAdapter.Fill(dataSet);
                        connection.Close();
                        if (result > 0 && dataSet.Tables[0].Rows.Count > 0)
                        {
                            return JsonConvert.SerializeObject(new ApiResponse { status = true, Data = dataSet.Tables[0], Error = "" });
                        }
                        else
                        {
                            return JsonConvert.SerializeObject(new ApiResponse { status = false, Data = "", Error = "Something went wrong." }); ;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.ExceptionLogging.WriteException(ex);
                return JsonConvert.SerializeObject(new ApiResponse { status = false, Data = "", Error = ex.Message.ToString() }); ;
                throw;
            }
        }
        #endregion


        #region ModifyCountry
        public string ModifyCountry(int countryId, string countryName, int updatedBy)
        {
            try
            {
                if (String.IsNullOrEmpty(countryName))
                {
                    return JsonConvert.SerializeObject(new ApiResponse { status = false, Data = "", Error = "Country name required" });
                }
                else if (countryId <= 0)
                {
                    return JsonConvert.SerializeObject(new ApiResponse { status = false, Data = "", Error = "Invalid request. Id should greater than zero." });
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("sp_country_update", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@countryId", countryId);
                        command.Parameters.AddWithValue("@name", countryName);
                        command.Parameters.AddWithValue("@updatedBy", updatedBy);
                        connection.Open();
                        int result = command.ExecuteNonQuery();
                        connection.Close();
                        if (result > 0)
                        {
                            return JsonConvert.SerializeObject(new ApiResponse { status = true, Data = "success", Error = "" }); ;
                        }
                        else
                        {
                            return JsonConvert.SerializeObject(new ApiResponse { status = false, Data = "fail", Error = "Something went wrong." });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.ExceptionLogging.WriteException(ex);
                return JsonConvert.SerializeObject(new ApiResponse { status = false, Data = "", Error = ex.Message.ToString() });
                throw;
            }
        }
        #endregion


        #region DeleteCountry
        public string DeleteCountry(int countryId, int DeletedBy, bool isDeleted)
        {
            try
            {
                if (countryId <= 0)
                {
                    return JsonConvert.SerializeObject(new ApiResponse { status = false, Data = "", Error = "Invalid request. Id should greater than zero." });
                }


                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("sp_country_delete", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@countryId", countryId);
                        command.Parameters.AddWithValue("@isdeleted", isDeleted);
                        command.Parameters.AddWithValue("@deletedBy", DeletedBy);
                        connection.Open();
                        int result = command.ExecuteNonQuery();
                        connection.Close();
                        if (result > 0)
                        {
                            return JsonConvert.SerializeObject(new ApiResponse { status = true, Data = "success", Error = "" }); ;
                        }
                        else
                        {
                            return JsonConvert.SerializeObject(new ApiResponse { status = false, Data = "fail", Error = "Something went wrong." });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.ExceptionLogging.WriteException(ex);
                return JsonConvert.SerializeObject(new ApiResponse { status = false, Data = "", Error = ex.Message.ToString() });
                throw;
            }
        }
        #endregion


    }
}
