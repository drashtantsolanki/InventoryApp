using Inventory.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.DTOs;
using WebApplication1.Extensions;

namespace WebApplication1.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        #region Constructor & Readonly property
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        public UsersController(IUserRepository usersRepository, IConfiguration configuration)
        {
            _userRepository = usersRepository;
            _configuration = configuration;
        }
        #endregion

        #region Registration
        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public string Registration(RegisterUserDto registerUserDto)
        {
            try
            {
                bool result = _userRepository.Registration(registerUserDto.Username, registerUserDto.Email, registerUserDto.password, registerUserDto.createdBy > 0 ? registerUserDto.createdBy : 0);
                if (result)
                {
                    return JsonConvert.SerializeObject(new ResponseDto() { Status = true, Result = "Success", Error = "" });
                }
                else
                {
                    return JsonConvert.SerializeObject(new ResponseDto() { Status = false, Result = "Fail", Error = "Something went wrong.😟" });
                }
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new ResponseDto() { Status = true, Result = "Success", Error = "" });
                throw;
            }
        }
        #endregion

        #region LOGIN
        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public string Login(LoginDto loginDto)
        {
            try
            {
                DataTable dataTable = _userRepository.Login(loginDto.Username, loginDto.password);
                if (dataTable != null)
                {
                    Token ObjToken = new Token(_configuration);

                    var token = ObjToken.GenerateToken(dataTable.Rows[0]["Id"].ToString(), loginDto.Username);
                    return JsonConvert.SerializeObject(new ResponseDto() { Status = true, Result = token, Error = "" });
                }
                else
                {
                    return JsonConvert.SerializeObject(new ResponseDto() { Status = false, Result = "fail", Error = "Something went wrong." });

                }
            }
            catch (Exception ex)
            {
                JsonConvert.SerializeObject(new ResponseDto() { Status = false, Result = "", Error = ex.Message.ToString() });
                throw;
            }
        }
        #endregion

        /// <summary>
        /// this will fetch users according to delete flag
        /// delete flag values 0=all users with deleted, 1=not deleted users only, 2=deleted users only
        /// </summary>
        /// <param name="deleteFlag"></param>
        /// <returns></returns>
        #region GetAllUsers

        [HttpGet("{deleteflag}")]
        [Route("users/{deleteflag}")]
        public string getUsers(int deleteFlag)
        {
            try
            {
                DataTable dtUsers = _userRepository.GetUsers(deleteFlag);
                return JsonConvert.SerializeObject(new ResponseDto { Status = true, Result = dtUsers, Error = "" });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new ResponseDto { Status = false, Result = null, Error = ex.Message.ToString() });
                throw;
            }
        }
        #endregion

        #region UserById
        [HttpGet("{userId}")]
        [Route("userById/{userId}")]
        public string getUserById(int userId)
        {
            try
            {
                var response = new ResponseDto
                {
                    Status = true,
                    Error = "",
                    Result = _userRepository.GetUserById(userId)
                };
                return JsonConvert.SerializeObject(response);
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(ex.Message.ToString());
                throw;
            }
        }
        #endregion

        #region UpdateUser
        [HttpPut]
        [Route("Update")]
        public string UpdateUser(UpdateUserDto user)
        {
            try
            {
                bool result = _userRepository.ModifyUser(user.Id, user.Email, user.Username, user.UpdatedBy);
                var response = new ResponseDto();
                if (result)
                {
                    response.Status = true;
                    response.Result = result;
                    response.Error = "";
                }
                else
                {
                    response.Status = false;
                    response.Result = result;
                    response.Error = "Something went wrong";
                }
                return JsonConvert.SerializeObject(response);
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(ex.Message.ToString());
                throw;
            }
        }
        #endregion

        #region DeleteUser
        [HttpDelete]
        [Route("delete")]
        public string deleteUser(DeleteDto deleteObj)
        {
            try
            {
                bool result = _userRepository.DeleteUser(deleteObj.Id, deleteObj.DeletedBy, deleteObj.IsDeleted);
                var response = new ResponseDto();
                if (result)
                {
                    response.Status = true;
                    response.Result = result;
                    response.Error = "";
                }
                else
                {
                    response.Status = false;
                    response.Result = result;
                    response.Error = "Something went wrong";
                }
                return JsonConvert.SerializeObject(response);
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
