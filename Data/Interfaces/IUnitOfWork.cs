using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebBlogApi.Data.Interfaces
{
    public interface IUnitOfWork
    {
  
        IUserRepository UserRepository { get; }
        IBlogRepository BlogRepository { get; }
        Task<bool> SaveAsync();
        
    }
}
