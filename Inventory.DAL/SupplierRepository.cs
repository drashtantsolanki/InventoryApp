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
        private readonly CustomValidations customValidations;
        public SupplierRepository(CustomValidations validations)
        {
            customValidations = validations;
        }

        public string AddSupplier(Supplier supplier)
        {
            try
            {

                if(!customValidations.ValidateGst(supplier.GSTNo))
                {
                    return JsonConvert.SerializeObject(new ApiResponse { status = false, Data = "fail", Error = "Invalid GST Number." });
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
                if (sqlEx.Number == 2627)
                {
                    if (sqlEx.Message.Contains("UNIQUE_EMAIL"))
                    {
                        return JsonConvert.SerializeObject(new ApiResponse { status = false, Data = "Fail.", Error = "Email is already Registered.  =>" +sqlEx.Message.ToString() });
                    }
                    else if(sqlEx.Message.Contains("UNIQUE_CODE"))
                    {
                        return JsonConvert.SerializeObject(new ApiResponse { status = false, Data = "Fail.", Error = "Code should be unique.  =>" + sqlEx.Message.ToString() });
                    }
                    else if(sqlEx.Message.Contains("UNIQUE_GST"))
                    {
                        return JsonConvert.SerializeObject(new ApiResponse { status = false, Data = "Fail.", Error = "GST Number is already registered.  =>" + sqlEx.Message.ToString() });
                    }
                    else
                    {
                        return JsonConvert.SerializeObject(new ApiResponse { status = false, Data = "Fail.", Error = sqlEx.Message.ToString() });
                    }
                }
                else
                {
                    return JsonConvert.SerializeObject(new ApiResponse { status = false, Data = "Fail.", Error = sqlEx.Message.ToString() });
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.ExceptionLogging.WriteException(ex);
                return JsonConvert.SerializeObject(new ApiResponse { status = true, Data = "", Error = ex.Message.ToString() });
                throw;
            }
        }

    }
}
