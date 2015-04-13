using System;

using Gnosis.Entities.Examples.Users;

namespace Gnosis.Entities.Examples.WebApplication.Areas.Login.Models
{
    public class LoginRequestModel : IUserCredentials
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}