using Microsoft.AspNetCore.Mvc;
using organizer_server.Models;
using System;
using System.Linq;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Collections.Generic;


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
                        var identity = GetIdentity(users.username, users.pass);
                        var now = DateTime.UtcNow;
                        // создаем JWT-токен
                        var jwt = new JwtSecurityToken(
                                issuer: AuthOptions.ISSUER,
                                audience: AuthOptions.AUDIENCE,
                                notBefore: now,
                                claims: identity.Claims,
                                expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
                        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
                        response.token = encodedJwt;
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
        private ClaimsIdentity GetIdentity(string username, string password)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, username),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, "user")
            };
            ClaimsIdentity claimsIdentity =
            new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;
        }
    }
}
