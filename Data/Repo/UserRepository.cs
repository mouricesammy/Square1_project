using WebBlogApi.Data.Interfaces;
using WebBlogApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WebBlogApi.Data.Repo
{
    public class UserRepository : IUserRepository
    {
        private readonly WebBlogApiDbContext db;

        public UserRepository(WebBlogApiDbContext db)
        {
            this.db = db;
        }
        public async Task<User> Authenticate(string userName, string passwordText)
        {

            var user = await db.Users.FirstOrDefaultAsync(m => m.Username == userName/*&& m.Password  == password*/);
            if (user == null || user.PasswordKey == null)
                return null;
            if (!MatchPassword(passwordText, user.Password, user.PasswordKey))
                return null;
            return user;
        }

        private bool MatchPassword(string passwordText, byte[] password, byte[] passwordKey)
        {
            using (var hmac = new HMACSHA512(passwordKey))
            {

                var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(passwordText));

                for (int i = 0; i < passwordHash.Length; i++)
                {
                    if (passwordHash[i] != password[i])
                    {
                        return false;
                    }

                }
                return true;

            }
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await db.Users.ToListAsync();
        }

        public void Register(string userName, string password)
        {
            byte[] passwordHash, passwordKey;

            using (var hmac = new HMACSHA512())
            {
                passwordKey = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            }
            User user = new User();
            user.Username = userName;
            user.Password = passwordHash;
            user.PasswordKey = passwordKey;

            db.Users.Add(user);
            db.SaveChanges();
        }

        public async Task<bool> UserAlreadyExist(string userName)
        {
            return await db.Users.AnyAsync(x => x.Username == userName);
        }
    }
}
