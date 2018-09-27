using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ServiceInterfaces.Abstractions.Common.Identity;
using ServiceInterfaces.Messages.Common.Identity;
using ServiceInterfaces.ViewModels.Common.Identity;

namespace SirmiumERPWeb.Controllers.Identity
{
    public class UserController
    {

        IUserService userRepository { get; set; }

        public UserController(IServiceProvider provider)
        {
            userRepository = provider.GetRequiredService<IUserService>();

        }
        // GET: api/Box
        [HttpGet]
        public JsonResult GetUsers()
        {
            UserListResponse response;
            try
            {
                response = userRepository.GetUsers();
            }
            catch (Exception ex)
            {
                response = null;
                Console.WriteLine(ex.Message);
            }
            return new JsonResult(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }

        [HttpGet]
        public JsonResult GetUser(int id)
        {
            UserResponse user;
            try
            {
                user = userRepository.GetUser(id);
            }
            catch (Exception ex)
            {
                user = null;
                Console.WriteLine(ex.Message);
            }
            return new JsonResult(user, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }

        [HttpPost]
        public JsonResult Create([FromBody] UserViewModel c)
        {
            UserResponse response;
            try
            {
                response = this.userRepository.Create(c);
            }
            catch (Exception ex)
            {
                response = null;
                Console.WriteLine(ex.Message);
            }

            return new JsonResult(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }

        [HttpPost]
        public JsonResult Update([FromBody] UserViewModel c)
        {
            UserResponse response;
            try
            {
                response = this.userRepository.Update(c);
            }
            catch (Exception ex)
            {
                response = null;
                Console.WriteLine(ex.Message);
            }

            return new JsonResult(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }

        [HttpGet]
        public JsonResult Delete(int id)
        {
            UserResponse response;
            try
            {
                response = this.userRepository.Delete(id);
            }
            catch (Exception ex)
            {
                response = null;
                Console.WriteLine(ex.Message);
            }

            return new JsonResult(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }
    }
}
