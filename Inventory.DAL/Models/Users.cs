using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.DAL.Models
{
    public class Users
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        private string _password { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public int? DeletedBy { get; set; }
        public bool? IsDeleted { get; set; } = false;

        public string Password
        { 
            get 
            { 
                return this._password; 
            }
            set
            {
                _password = BCrypt.Net.BCrypt.HashPassword(value);
            }
        }

        public bool isValidpassword(string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, _password);
        }

    }
}
