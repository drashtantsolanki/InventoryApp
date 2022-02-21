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
    }
}
