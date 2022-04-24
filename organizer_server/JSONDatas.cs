using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace organizer_server
{
    public class JSONDataLogin
    {
        public string login { get; set; }
        public string pass { get; set; }
        public bool loginValid { get; set; }
        public bool passValid { get; set; }
    }

    public class JSONDataReg
    {
        public string firstname { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string pass { get; set; }
        public string rePass { get; set; }
        public string sex { get; set; }
        public string newsletter { get; set; }
    }

    public class JSONLoginResponse 
    {
        public bool login { get; set; }
        public bool password { get; set; }
        public string token { get; set; }
        public JSONLoginResponse() {
            login = false;
            password = false;
            token = null;
        }
    }

    public class JSONRegResponse
    {
        public bool login { get; set; }

        public JSONRegResponse()
        {
            login = false;
        }
    }
}
