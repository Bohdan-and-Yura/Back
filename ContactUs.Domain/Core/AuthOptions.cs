using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ConnectUs.Domain.Core
{
    public class AuthOptions
    {
        public const string ISSUER = "MyAuthServer"; // token creator
        public const string AUDIENCE = "MyAuthClient"; // token user
        private const string KEY = "someSecretKey123!@"; // key
        public const int LIFETIME = 7; // lifetime 7 

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}