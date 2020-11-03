using ConnectUs.Domain.DTO.AccountDTO;
using ConnectUs.Domain.Entities;
using ConnectUs.Domain.Helpers;
using ConnectUs.Domain.IRepositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Unicode;
using System.Threading.Tasks;

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
            if (!VerifyPasswordHash(password, user.PasswordHashByte, user.PasswordSaltByte))
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


        public async Task<User> CreateAsync(User newUser)
        {
            
            if (_context.Users.Any(c => c.Email == newUser.Email))
                return null;
            byte[] passwordHash;
            byte[] passwordSalt;
            CreatePasswordHash(newUser.Password, out passwordHash, out passwordSalt);
            newUser.PasswordHashByte = passwordHash;
            newUser.PasswordSaltByte = passwordSalt;
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            return newUser;

        }
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<EditUserDTO> UpdateAsync(EditUserDTO editModel)
        {
            User user = GetUserByEmail(editModel.Email);
            if (user != null)
            {
                user.BirthDay = editModel.BirthDay;
                user.UserName = editModel.UserName;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return editModel;
            }
            return null;
        }

        public async Task DeleteAsync(string email)
        {
            User user = GetUserByEmail(email);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

        }
    }
}