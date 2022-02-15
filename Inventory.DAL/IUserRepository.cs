using Inventory.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.DAL
{
    public interface IUserRepository
    {
        public bool Registration(string username, string email, string password, int createdBy = 0);
        public DataTable Login(string username, string password);
        public DataTable GetUsers(int deleteflag=0);
        public DataTable GetUserById(int userId);
        public bool ModifyUser(int userId,string email,string username,int updatedBy);
        public bool DeleteUser(int userId,int deletedBy, bool isDeleted);

    }
}
