using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace organizer_server
{
    public class AuthOptions
    {
        public const string ISSUER = "OrganizerServer";     // издатель токена
        public const string AUDIENCE = "OrganizerClient";   // потребитель токена
        const string KEY = "organizer!net!+abracadabra6";   // ключ для шифрования
        public const int LIFETIME = 1;                      // время жизни токена - 1 минута
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
