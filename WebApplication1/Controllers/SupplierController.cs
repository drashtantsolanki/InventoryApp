using Inventory.DAL;
using Inventory.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierRepository _supplierRepository;
        public SupplierController(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        [HttpPost]
        [Route("addSupploer")]
        public string addSupplier(Supplier supplier)
        {
            try
            {
                return _supplierRepository.AddSupplier(supplier);
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(ex.Message.ToString());
                throw;
            }
        }


        [HttpGet("{deleteFlag}")]
        [Route("GetSuppliers/{deleteFlag}")]
        public string GetSuppliers(int deleteFlag)
        {
            try
            {
                return _supplierRepository.GetSuppliers(deleteFlag);
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(ex.Message.ToString());
                throw;
            }
        }

        [HttpGet("{supplierId}")]
        [Route("GetSupplierById/{supplierId}")]
        public string GetSupplierById(int supplierId)
        {
            try
            {
                return _supplierRepository.GetSupplierById(supplierId);
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(ex.Message.ToString());
                throw;
            }
        }

        [HttpPut]
        [Route("UpdateSupplier")]
        public string UpdateSupplier(Supplier supplier)
        {
            try
            {
                return _supplierRepository.ModifySupplier(supplier);
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(ex.Message.ToString());
                throw;
            }
        }

        [HttpDelete]
        [Route("DeleteSupplier")]
        public string DeleteSupplier(Supplier supplier)
        {
            try
            {
                return _supplierRepository.DeleteSupplier(supplier.Id, Convert.ToInt32(supplier.DeletedBy), Convert.ToBoolean(supplier.IsDeleted));
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(ex.Message.ToString());
                throw;
            }
        }
    }
}
