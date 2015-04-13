using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Gnosis.Entities.Examples.WebApplication.Areas.Login.Models;
using Gnosis.Entities.Testing.Data;
using Gnosis.Entities.Examples.Users;
using Gnosis.Entities.Requests;

namespace Gnosis.Entities.Examples.WebApplication.Areas.Login.Controllers
{
    public class DataController : Controller
    {
        private class UserCreateRequest : EntityCreateRequest, IUser
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }
        
        public class MemoryUserDataManager : MemoryEntityDataManager, IUserDataManager
        {
            public Guid? FindUserByCredentials(IUserCredentials credentials)
            {
                IEnumerable<Guid> revisions = savedFieldValues
                    .Where(x => x.Value.ContainsKey("Username") && (string)x.Value["Username"] == credentials.Username && x.Value.ContainsKey("Password") && (string)x.Value["Password"] == credentials.Password)
                    .Select(x => x.Key);
            }
        }
        
        public ActionResult Login(LoginRequestModel request)
        {
            LoginResponseModel response = new LoginResponseModel();

            return Json(response);
        }

    }
}
