using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.DAL
{
    public interface IStateRepository
    {
        public string AddState(string name, int countryId, int createdBy);
        public string GetStates(int deleteFlag = 0);
        public string GetStateById(int stateId);
        public string ModifyState(int stateId, int countryId, string stateName, int updatedBy);
        public string DeleteState(int stateId, int deletedBy, bool isDeleted);



    }
}
