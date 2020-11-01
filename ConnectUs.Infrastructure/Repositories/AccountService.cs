using ConnectUs.Domain.DTO.AccountDTO;
using ConnectUs.Domain.Entities;
using ConnectUs.Domain.Helpers;
using ConnectUs.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Unicode;

namespace ConnectUs.Infrastructure.Repositories
{
    public class AccountService : IAccountService
    {
        private readonly BaseDbContext _context;

        public AccountService(BaseDbContext baseDbContext)
        {
            _context = baseDbContext;
        }
        public User GetUserByEmail(string userName)
        {
            return _context.Users.FirstOrDefault(c => c.Email.ToLower() == userName.ToLower());

        }
        public User Authenticate(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return null;

            var user = _context.Users.SingleOrDefault(x => x.Email == email);

            // check if username exists
            if (user == null)
                return null;

            // check if password is correct
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            // authentication successful
            return user;
        }
        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    //password verifications
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }


        public RegisterDTO Create(RegisterDTO registerDTO, string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new AppException("Password is required");
            if (_context.Users.Any(c => c.Email == registerDTO.Email))
                throw new AppException("Email is already taken");
            byte[] passwordHash;
            byte[] passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);
            registerDTO.PasswordHash = passwordHash;
            registerDTO.PasswordSalt = passwordSalt;
            //Mapper.Map<CreatingModel>(entity))
            return registerDTO;

        }
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
