using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ConnectUs.Domain.DTO.AccountDTO;
using ConnectUs.Domain.Entities;
using ConnectUs.Domain.IRepositories.Public;
using Microsoft.EntityFrameworkCore;

namespace ConnectUs.Infrastructure.Repositories.Public
{
    public class AccountService : IAccountService
    {
        private readonly BaseDbContext _context;
        private readonly IMapper _mapper;

        public AccountService(BaseDbContext baseDbContext, IMapper mapper)
        {
            _context = baseDbContext;
            _mapper = mapper;
        }

        public async Task<User> GetUserById(string? id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return await _context.Users.Include(c => c.MeetupsJoined).FirstOrDefaultAsync(c => c.Id == id);
        }

        public User Authenticate(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return null;
            var user = _context.Users.Include(c => c.MeetupsJoined).SingleOrDefault(x => x.Email == email);

            // check if user exists
            if (user == null)
                return null;

            // check if password is correct
            if (!VerifyPasswordHash(password, user.PasswordHashByte, user.PasswordSaltByte))
                return null;

            // authentication successful
            return user;
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

        public async Task<EditUserDTO> UpdateAsync(string id, EditUserDTO editModel)
        {
            var user = await GetUserById(id);
            if (user != null)
            {
                var result = _mapper.Map(editModel, user);
                _context.Users.Update(result);
                await _context.SaveChangesAsync();
                return editModel;
            }

            return null;
        }

        public async Task DeleteAsync(string id)
        {
            var user = await GetUserById(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64)
                throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128)
                throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (var i = 0; i < computedHash.Length; i++)
                    //password verifications
                    if (computedHash[i] != storedHash[i])
                        return false;
            }

            return true;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
    }
}