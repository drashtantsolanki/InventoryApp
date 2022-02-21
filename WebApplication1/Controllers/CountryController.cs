using Inventory.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.DTOs;

namespace WebApplication1.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        #region Private properties and Constructor
        private readonly ICountryRepository _countryRepository;
        public CountryController(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        } 
        #endregion

        #region Add Country
        [HttpPost]
        [Route("addCountry")]
        public string AddCountry(CountryDto countryDto)
        {
            try
            {
                return _countryRepository.AddCountry(countryDto.Name, Convert.ToInt32(countryDto.CreatedBy));
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
                throw;
            }
        }
        #endregion

        #region Get Countries


        [HttpGet("{deleteflag}")]
        [Route("getCountries/{deleteflag}")]
        public string getCountries(int deleteFlag)
        {
            try
            {
                return _countryRepository.getCountries(deleteFlag);
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new ResponseDto { Status = false, Result = null, Error = ex.Message.ToString() });
                throw;
            }
        }


        #endregion

        #region Get Country By Id
        [HttpGet("{countryId}")]
        [Route("getCountryById/{countryId}")]
        public string getCountryById(int countryId)
        {
            try
            {
                return _countryRepository.getCountryById(countryId);
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(ex.Message.ToString());
                throw;
            }
        }
        #endregion

        #region Update Country
        [HttpPut]
        [Route("UpdateCountry")]
        public string UpdateCountry(CountryDto countryDto)
        {
            try
            {
                return _countryRepository.ModifyCountry(countryDto.Id, countryDto.Name, Convert.ToInt32(countryDto.UpdatedBy));
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(ex.Message.ToString());
                throw;
            }
        }
        #endregion

        #region Delete Country
        [HttpDelete]
        [Route("deleteCountry")]
        public string deleteCountry(CountryDto countryDto)
        {
            try
            {
                
                return _countryRepository.DeleteCountry(countryDto.Id, Convert.ToInt32(countryDto.DeletedBy), Convert.ToBoolean(countryDto.IsDeleted));
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(ex.Message.ToString());
                throw;
            }
        } 
        #endregion
    }
}
