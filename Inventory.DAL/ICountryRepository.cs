using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.DAL
{
    public interface ICountryRepository
    {
        public string AddCountry(string countryname, int createdBy);
        public string getCountries(int deleteFlag=0);
        public string getCountryById(int countryId);
        public string ModifyCountry(int countryId, string countryName, int updatedBy);
        public string DeleteCountry(int countryId, int DeletedBy, bool isDeleted);
    }
}
