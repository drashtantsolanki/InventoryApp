using Inventory.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.DAL
{
    public interface ISupplierRepository
    {
        public string AddSupplier(Supplier supplier);
        public string GetSuppliers(int deleteFlag = 0);
        public string GetSupplierById(int supplierId);
        public string ModifySupplier(Supplier supplier);

        public string DeleteSupplier(int supplierId, int deletedBy, bool isDeleted);
    }
}
