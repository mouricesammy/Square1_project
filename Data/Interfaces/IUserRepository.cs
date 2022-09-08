using WebBlogApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebBlogApi.Data.Interfaces
{
    public interface IUserRepository
    {
        Task<User> Authenticate(string userName, string password);
        void Register(string userName, string password);

        Task<bool> UserAlreadyExist(string userName);

        Task<IEnumerable<User>> GetAllUsersAsync();
    }
}
