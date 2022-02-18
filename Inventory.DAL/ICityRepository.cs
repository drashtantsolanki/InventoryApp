using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.DAL
{
    public interface ICityRepository
    {
        public string AddState(string name,int stateId, int countryId, int createdBy);
        public string GetCities(int deleteFlag = 0);
        public string GetCityId(int cityId);
        public string ModifyCity(int cityId, int stateId, int countryId, string cityName, int updatedBy);
        public string DeleteCity(int cityId, int deletedBy, bool isDeleted);

    }
}
