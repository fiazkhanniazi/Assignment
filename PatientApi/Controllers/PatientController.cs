using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OA.DataDomain;
using OA.Infrastructure;
using OA.Web.Models;

namespace PatientApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class PatientController : Controller
    {

        private readonly OA.Infrastructure.IUserService userService;
        private readonly IUserProfileService userProfileService;

        public PatientController(IUserService userService, IUserProfileService userProfileService)
        {
            this.userService = userService;
            this.userProfileService = userProfileService;
        }

        [HttpGet]
        public List<UserViewModel> Index()
        {
            List<UserViewModel> model = new List<UserViewModel>();
            userService.GetUsers().ToList().ForEach(u =>
            {
                UserProfile userProfile = userProfileService.GetUserProfile(u.Id);
                UserViewModel user = new UserViewModel
                {
                    Id = u.Id,
                    Name = $"{userProfile.FirstName} {userProfile.LastName}",
                    Email = u.Email,
                    Address = userProfile.Address,
                    AddedDate= u.AddedDate,
                    ModifiedDate = u.ModifiedDate,
                    IPAddress = u.IPAddress
                };
                model.Add(user);
            });

            return model;
        }

        [HttpGet("AddUser")]
        public UserViewModel AddUser()
        {
            UserViewModel model = new UserViewModel();

            return model;
        }

        [HttpPost("AddUser")]
        public UserViewModel AddUser([FromBody]  UserViewModel model)
        {
            User userEntity = new User
            {
                UserName = model.UserName,
                Email = model.Email,
                Password = model.Password,
                AddedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
                IPAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                UserProfile = new UserProfile
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Address = model.Address,
                    AddedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow,
                    IPAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString()
                }
            };
            userService.InsertUser(userEntity);
            if (userEntity.Id > 0)
            {
                return model;
            }
            return model;
        }
        [HttpGet("EditUser")]
        public UserViewModel EditUser(int? id)
        {
            UserViewModel model = new UserViewModel();
            if (id.HasValue && id != 0)
            {
                User userEntity = userService.GetUser(id.Value);
                UserProfile userProfileEntity = userProfileService.GetUserProfile(id.Value);
                model.FirstName = userProfileEntity.FirstName;
                model.LastName = userProfileEntity.LastName;
                //model.Address = userProfileEntity.Address;
                //model.Email = userEntity.Email;
            }
            return model;
        }

        [HttpPost]
        public UserViewModel EditUser([FromBody]UserViewModel model)
        {
            User userEntity = userService.GetUser(model.Id);
          //  userEntity.Email = model.Email;
            userEntity.ModifiedDate = DateTime.UtcNow;
            userEntity.IPAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            UserProfile userProfileEntity = userProfileService.GetUserProfile(model.Id);
            userProfileEntity.FirstName = model.FirstName;
            userProfileEntity.LastName = model.LastName;
           // userProfileEntity.Address = model.Address;
            userProfileEntity.ModifiedDate = DateTime.UtcNow;
            userProfileEntity.IPAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            userEntity.UserProfile = userProfileEntity;
            userService.UpdateUser(userEntity);
            if (userEntity.Id > 0)
            {
                return model;
            }
            return model;
        }

        [HttpGet("{id}")]
        public string DeleteUser(int id)
        {
            //UserProfile userProfile = userProfileService.GetUserProfile(id);
            //string name = $"{userProfile.FirstName} {userProfile.LastName}";
            //return name;
            return null;
        }

        //[HttpPost]
        //public void DeleteUser(long id, FormCollection form)
        //{

        //}
    }
}