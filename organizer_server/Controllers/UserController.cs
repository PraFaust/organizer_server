using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using organizer_server.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;

namespace organizer_server.Controllers
{
    public class UserController : Controller
    {
        UserContext db;
        JSONDataLogin loginJSON;
        JSONDataReg registerJSON;
        public UserController(UserContext context)
        {
            db = context;
        }

        public string login()
        {
            JSONLoginResponse response = new JSONLoginResponse();
            string loginJSONRaw = Request.Query.FirstOrDefault(p => p.Key == "loginData").Value;
            loginJSON = JsonSerializer.Deserialize<JSONDataLogin>(loginJSONRaw);

            foreach (User users in db.Users) {
                if (users.username == loginJSON.login)
                {
                    response.login = true;
                    // correct login and pass
                    if (users.pass == loginJSON.pass)
                    {
                        response.password = true;
                        return JsonSerializer.Serialize<JSONLoginResponse>(response);
                    }
                    // incorrect pass
                    else
                    {
                        response.password = false;
                        return JsonSerializer.Serialize<JSONLoginResponse>(response);
                    }
                }
            }
            // incorrect login
            return JsonSerializer.Serialize<JSONLoginResponse>(response);
        }

        public string register()
        {
            JSONRegResponse response = new JSONRegResponse();
            string registerJSONRaw = Request.Query.FirstOrDefault(p => p.Key == "registerData").Value;
            registerJSON = JsonSerializer.Deserialize<JSONDataReg>(registerJSONRaw);

            foreach (User users in db.Users)
            {
                // username is taken
                if (users.username == registerJSON.username)
                {
                    return JsonSerializer.Serialize<JSONRegResponse>(response);
                }
            }

            response.login = true;

            db.AddRange(
                    new User
                    {
                        id = 0,
                        firstname = registerJSON.firstname,
                        username = registerJSON.username,
                        email = registerJSON.email,
                        pass = registerJSON.pass,
                        sex = registerJSON.sex,
                        newsletter = ((registerJSON.newsletter == "news_accept")?true:false)
                    }
            );
            db.SaveChanges();
            // Registration success;
            return JsonSerializer.Serialize<JSONRegResponse>(response); ;
        }
    }
}
