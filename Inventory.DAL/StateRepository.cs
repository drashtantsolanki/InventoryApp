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
    public class StateRepository : IStateRepository
    {

        #region PrivateProperties
        private readonly string connectionString = ConnectionString.Value;
        #endregion

        #region Add State
        public string AddState(string name, int countryId, int createdBy)
        {
            try
            {
                if (String.IsNullOrEmpty(name))
                {
                    return JsonConvert.SerializeObject(new ApiResponse { status = false, Data = "", Error = "State name required" });
                }

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("sp_state_insert", con))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@name", name);
                        command.Parameters.AddWithValue("@countryId", countryId);
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
            catch (Exception ex)
            {
                ExceptionLogging.ExceptionLogging.WriteException(ex);
                return JsonConvert.SerializeObject(new ApiResponse { status = false, Data = "", Error = ex.Message.ToString() });
                throw;
            }

        } 
        #endregion

        #region Get States
        public string GetStates(int deleteFlag = 0)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("sp_state_get", connection))
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

        #region Get State By Id
        public string GetStateById(int stateId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("sp_state_getById", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@stateId", stateId);
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

        #region Modify State
        public string ModifyState(int stateId, int countryId, string stateName, int updatedBy)
        {
            try
            {
                if (String.IsNullOrEmpty(stateName))
                {
                    return JsonConvert.SerializeObject(new ApiResponse { status = false, Data = "", Error = "Country name required" });
                }
                else if (stateId <= 0)
                {
                    return JsonConvert.SerializeObject(new ApiResponse { status = false, Data = "", Error = "Invalid request. Id should greater than zero." });
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("sp_state_update", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@stateId", stateId);
                        command.Parameters.AddWithValue("@countryId", countryId);
                        command.Parameters.AddWithValue("@name", stateName);
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

        #region Delete State
        public string DeleteState(int stateId, int deletedBy, bool isDeleted)
        {
            try
            {
                if (stateId <= 0)
                {
                    return JsonConvert.SerializeObject(new ApiResponse { status = false, Data = "", Error = "Invalid request. Id should greater than zero." });
                }


                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("sp_state_delete", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@stateId", stateId);
                        command.Parameters.AddWithValue("@isdeleted", isDeleted);
                        command.Parameters.AddWithValue("@deletedBy", deletedBy);
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
