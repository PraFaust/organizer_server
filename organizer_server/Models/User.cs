using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace organizer_server.Models
{
    public class User
    {
        public int id { get; set; }
        public string firstname { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string pass { get; set; }
        public string sex { get; set; }
        public bool newsletter { get; set; }
    }
}
