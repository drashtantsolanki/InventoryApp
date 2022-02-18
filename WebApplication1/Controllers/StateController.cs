using Inventory.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.DTOs;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StateController : ControllerBase
    {
        private readonly IStateRepository _stateRepository;
        public StateController(IStateRepository stateRepository)
        {
            _stateRepository = stateRepository;
        }

        #region Add State
        [HttpPost]
        [Route("addState")]
        public string AddState(StateDto stateDto)
        {
            try
            {
                return _stateRepository.AddState(stateDto.StateName, stateDto.CountryId, Convert.ToInt32(stateDto.CreatedBy));
            }
            catch (Exception ex)
            {
                JsonConvert.SerializeObject(ex.Message.ToString());
                throw;
            }
        }
        #endregion

        #region Get States
        [HttpGet("{deleteFlag}")]
        [Route("getStates/{deleteFlag}")]
        public string getStates(int deleteFlag)
        {
            try
            {
                return _stateRepository.GetStates(deleteFlag);
            }
            catch (Exception ex)
            {
                JsonConvert.SerializeObject(ex.Message.ToString());
                throw;
            }
        }
        #endregion

        #region Get state By Id
        [HttpGet("{stateId}")]
        [Route("getStateById/{stateId}")]
        public string getStateById(int stateId)
        {
            try
            {
                return _stateRepository.GetStateById(stateId);
            }
            catch (Exception ex)
            {
                JsonConvert.SerializeObject(ex.Message.ToString());
                throw;
            }
        }
        #endregion

        #region Update State
        [HttpPut]
        [Route("updateState")]
        public string updateState(StateDto stateDto)
        {
            try
            {
                return _stateRepository.ModifyState(stateDto.Id, stateDto.CountryId, stateDto.StateName, Convert.ToInt32(stateDto.UpdatedBy));
            }
            catch (Exception ex)
            {
                JsonConvert.SerializeObject(ex.Message.ToString());
                throw;
            }
        }
        #endregion

        #region Delete State
        [HttpDelete]
        [Route("deleteState")]
        public string deleteState(StateDto stateDto)
        {
            try
            {
                return _stateRepository.DeleteState(stateDto.Id, Convert.ToInt32(stateDto.DeletedBy), stateDto.IsDeleted);
            }
            catch (Exception ex)
            {
                JsonConvert.SerializeObject(ex.Message.ToString());
                throw;
            }
        } 
        #endregion
    }
}
