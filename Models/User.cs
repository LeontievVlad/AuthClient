using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace AuthClient.Models
{
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class LoginResponse
    {
        public string Message { get; set; }
        public User User { get; set; }

        public LoginResponse(string content)
        {
            var jsonObject = JsonObject.Parse(content);
            Message = (string)jsonObject["message"];

            var jsonObjectUser = jsonObject["user"];
            User = new User()
            {
                Id = (int)jsonObjectUser["id"],
                Email = (string)jsonObjectUser["email"],
                Username = (string)jsonObjectUser["username"],
                Password = (string)jsonObjectUser["password"]
            };
        }
    }

    public class ErrorResponse
    {
        public string Message { get; set; }
    }

    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
