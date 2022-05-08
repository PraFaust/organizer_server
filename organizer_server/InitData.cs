using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using organizer_server.Models;

namespace organizer_server
{
    public class InitData
    {
        public static void Initialize(UserContext context)
        {
            var admin = context.Users
                    .Where(b => b.Username == "Admin")
                    .FirstOrDefault();

            if (admin == null)
            {
                context.Users.AddRange(
                    new User
                    {
                        UserId = 0,
                        Firstname = "Maxim",
                        Username = "Admin",
                        Email = "maximnsuslov@gmail.com",
                        Pass = "12345678",
                        Sex = "male",
                        Newsletter = true
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
