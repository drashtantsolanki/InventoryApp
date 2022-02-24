using Inventory.DAL.Extensions;
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
    public class SupplierRepository : ISupplierRepository
    {
        private readonly string connectionString = ConnectionString.Value;
        private readonly CustomValidations customValidations = new CustomValidations();

        public string AddSupplier(Supplier supplier)
        {
            try
            {
                if (!String.IsNullOrEmpty(supplier.GSTNo))
                {
                    if (!customValidations.ValidateGst(supplier.GSTNo))
                    {
                        return JsonConvert.SerializeObject(new ApiResponse { status = false, Data = "fail", Error = "Invalid GST Number." });
                    }
                    supplier.PAN = supplier.GSTNo.Substring(2, 10);

                }
                if (!String.IsNullOrEmpty(supplier.PAN))
                {
                    if (!customValidations.ValidatePan(supplier.PAN))
                    {
                        return JsonConvert.SerializeObject(new ApiResponse { status = false, Data = "fail", Error = "Invalid PAN Number." });
                    }
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("sp_Supplier_insert", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@name", supplier.Name);
                        command.Parameters.AddWithValue("@contactNumber", supplier.ContactNo);
                        command.Parameters.AddWithValue("@code", supplier.Code);
                        command.Parameters.AddWithValue("@email", supplier.EmailId);
                        command.Parameters.AddWithValue("@gstno", supplier.GSTNo);
                        command.Parameters.AddWithValue("@pan", supplier.PAN);
                        command.Parameters.AddWithValue("@address", supplier.Address);
                        command.Parameters.AddWithValue("@cityId", supplier.CityId);
                        command.Parameters.AddWithValue("@stateId", supplier.StateId);
                        command.Parameters.AddWithValue("@countryId", supplier.CountryId);
                        command.Parameters.AddWithValue("@createdBy", supplier.CreatedBy);
                        //command.Parameters.AddWithValue("", supplier.CreatedBy);
                        connection.Open();
                        int result = command.ExecuteNonQuery();
                        connection.Close();
                        if (result > 0)
                        {
                            return JsonConvert.SerializeObject(new ApiResponse { status = true, Data = "Success", Error = "" });
                        }
                        else
                        {
                            return JsonConvert.SerializeObject(new ApiResponse { status = false, Data = "Fail", Error = "Something went wrong." });
                        }

                    }
                }

            }
            catch (SqlException sqlEx)
            {
                //if (sqlEx.Number == 2627)
                //{
                //    if (sqlEx.Message.Contains("UNIQUE_EMAIL"))
                //    {
                //        return JsonConvert.SerializeObject(new ApiResponse { status = false, Data = "Fail.", Error = "Email is already Registered.  =>" + sqlEx.Message.ToString() });
                //    }
                //    else if (sqlEx.Message.Contains("UNIQUE_CODE"))
                //    {
                //        return JsonConvert.SerializeObject(new ApiResponse { status = false, Data = "Fail.", Error = "Code should be unique.  =>" + sqlEx.Message.ToString() });
                //    }
                //    else if (sqlEx.Message.Contains("UNIQUE_GST"))
                //    {
                //        return JsonConvert.SerializeObject(new ApiResponse { status = false, Data = "Fail.", Error = "GST Number is already registered.  =>" + sqlEx.Message.ToString() });
                //    }
                //    else if (sqlEx.Message.Contains("UNIQUE_PAN"))
                //    {
                //        return JsonConvert.SerializeObject(new ApiResponse { status = false, Data = "Fail.", Error = "PAN Number is already registered.  =>" + sqlEx.Message.ToString() });
                //    }
                //    else
                //    {
                //        return JsonConvert.SerializeObject(new ApiResponse { status = false, Data = "Fail.", Error = sqlEx.Message.ToString() });
                //    }
                //}
                //else
                //{
                //    return JsonConvert.SerializeObject(new ApiResponse { status = false, Data = "Fail.", Error = sqlEx.Message.ToString() });
                //}
                return JsonConvert.SerializeObject(new ApiResponse { status = false, Data = "Fail.", Error = sqlEx.Message.ToString() });
            }
            catch (Exception ex)
            {
                ExceptionLogging.ExceptionLogging.WriteException(ex);
                return JsonConvert.SerializeObject(new ApiResponse { status = true, Data = "", Error = ex.Message.ToString() });
                throw;
            }
        }


        public string GetSuppliers(int deleteFlag = 0)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("sp_supplier_get", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@deleteflag", deleteFlag);
                        //command.Parameters.AddWithValue("", supplier.CreatedBy);
                        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command);
                        DataSet dataSet = new DataSet();
                        connection.Open();
                        int result = sqlDataAdapter.Fill(dataSet);
                        connection.Close();
                        if (result > 0)
                        {
                            return JsonConvert.SerializeObject(new ApiResponse { status = true, Data = dataSet.Tables[0], Error = "" });
                        }
                        else
                        {
                            return JsonConvert.SerializeObject(new ApiResponse { status = false, Data = "Fail", Error = "Something went wrong." });
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

        public string GetSupplierById(int supplierId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("sp_supplier_getById", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@supplierId", supplierId);
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
                throw;
            }
        }

        public string ModifySupplier(Supplier supplier)
        {
            try
            {
                if (!String.IsNullOrEmpty(supplier.GSTNo))
                {
                    if (!customValidations.ValidateGst(supplier.GSTNo))
                    {
                        return JsonConvert.SerializeObject(new ApiResponse { status = false, Data = "fail", Error = "Invalid GST Number." });
                    }
                    //supplier.PAN = supplier.GSTNo.Substring(2, 10);

                }
                if (!String.IsNullOrEmpty(supplier.PAN))
                {
                    if (!customValidations.ValidatePan(supplier.PAN))
                    {
                        return JsonConvert.SerializeObject(new ApiResponse { status = false, Data = "fail", Error = "Invalid PAN Number." });
                    }
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("sp_supplier_update", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Id", supplier.Id);
                        command.Parameters.AddWithValue("@name", supplier.Name);
                        command.Parameters.AddWithValue("@contactNumber", supplier.ContactNo);
                        command.Parameters.AddWithValue("@code", supplier.Code);
                        command.Parameters.AddWithValue("@email", supplier.EmailId);
                        command.Parameters.AddWithValue("@gstno", supplier.GSTNo);
                        command.Parameters.AddWithValue("@pan", supplier.PAN);
                        command.Parameters.AddWithValue("@address", supplier.Address);
                        command.Parameters.AddWithValue("@cityId", supplier.CityId);
                        command.Parameters.AddWithValue("@stateId", supplier.StateId);
                        command.Parameters.AddWithValue("@countryId", supplier.CountryId);
                        command.Parameters.AddWithValue("@updatedBy", supplier.UpdatedBy);
                        //command.Parameters.AddWithValue("", supplier.CreatedBy);
                        connection.Open();
                        int result = command.ExecuteNonQuery();
                        connection.Close();
                        if (result > 0)
                        {
                            return JsonConvert.SerializeObject(new ApiResponse { status = true, Data = "Success", Error = "" });
                        }
                        else
                        {
                            return JsonConvert.SerializeObject(new ApiResponse { status = false, Data = "Fail", Error = "Something went wrong." });
                        }

                    }
                }
            }
            catch (SqlException sqlEx)
            {
                return JsonConvert.SerializeObject(new ApiResponse { status = false, Data = "Fail.", Error = sqlEx.Message.ToString() });
            }
            catch (Exception ex)
            {
                ExceptionLogging.ExceptionLogging.WriteException(ex);
                throw;
            }
        }

        public string DeleteSupplier(int supplierId, int deletedBy, bool isDeleted)
        {
            try
            {
                if (supplierId <= 0)
                {
                    return JsonConvert.SerializeObject(new ApiResponse { status = false, Data = "", Error = "Invalid request. Id should greater than zero." });
                }
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("sp_supplier_delete", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@supplierId", supplierId);
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
                throw;
            }

        }
    }
}
