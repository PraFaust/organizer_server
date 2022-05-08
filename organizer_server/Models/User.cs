using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace organizer_server.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Firstname { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Pass { get; set; }
        public string Sex { get; set; }
        public bool Newsletter { get; set; }
    }
}
