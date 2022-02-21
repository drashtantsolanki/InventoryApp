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
    public class CityController : ControllerBase
    {
        private readonly ICityRepository _cityRepository;
        public CityController(ICityRepository cityRepository)
        {
            _cityRepository = cityRepository;
        }

        #region Add City
        [HttpPost]
        [Route("addCity")]
        public string addCity(CityDto cityDto)
        {
            try
            {
                return _cityRepository.AddCity(cityDto.CityName, cityDto.StateId, cityDto.CountryId, Convert.ToInt32(cityDto.CreatedBy));
            }
            catch (Exception ex)
            {
                JsonConvert.SerializeObject(ex.Message.ToString());
                throw;
            }
        }
        #endregion

        #region Get Cities
        [HttpGet("{deleteFlag}")]
        [Route("getCities/{deleteFlag}")]
        public string getCities(int deleteFlag)
        {
            try
            {
                return _cityRepository.GetCities(deleteFlag);
            }
            catch (Exception ex)
            {
                JsonConvert.SerializeObject(ex.Message.ToString());
                throw;
            }
        }
        #endregion

        #region Get City By Id
        [HttpGet("{cityId}")]
        [Route("getCityById/{cityId}")]
        public string getCityById(int cityId)
        {
            try
            {
                return _cityRepository.GetCityId(cityId);
            }
            catch (Exception ex)
            {
                JsonConvert.SerializeObject(ex.Message.ToString());
                throw;
            }
        }
        #endregion

        #region Update City
        [HttpPut]
        [Route("updateCity")]
        public string updateCity(CityDto cityDto)
        {
            try
            {
                return _cityRepository.ModifyCity(cityDto.Id, cityDto.StateId, cityDto.CountryId, cityDto.CityName, Convert.ToInt32(cityDto.UpdatedBy));
            }
            catch (Exception ex)
            {
                JsonConvert.SerializeObject(ex.Message.ToString());
                throw;
            }
        }
        #endregion

        #region Delete City
        [HttpDelete]
        [Route("deleteCity")]
        public string deleteCity(CityDto cityDto)
        {
            try
            {
                return _cityRepository.DeleteCity(cityDto.Id, Convert.ToInt32(cityDto.DeletedBy), cityDto.IsDeleted);
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
