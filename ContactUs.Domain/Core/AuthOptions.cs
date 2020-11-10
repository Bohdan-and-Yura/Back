using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConnectUs.Domain.Core
{
    public class AuthOptions
    {
        public const string ISSUER = "MyAuthServer"; // token creator
        public const string AUDIENCE = "MyAuthClient"; // token user
        const string KEY = "someSecretKey123!@";   // key
        public const int LIFETIME = 7; // lifetime 7 
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
