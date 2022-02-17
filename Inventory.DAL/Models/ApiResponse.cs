using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.DAL.Models
{
    class ApiResponse
    {
        public bool status { get; set; }
        public Object Data { get; set; }
        public string Error { get; set; }
    }
}
