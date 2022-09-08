using WebBlogApi.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebBlogApi.Data.Repo
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly WebBlogApiDbContext db;

        

        public UnitOfWork (WebBlogApiDbContext db)
        {
            this.db = db;
        }
       

        public IUserRepository UserRepository => new UserRepository(db);

        public IBlogRepository BlogRepository => throw new NotImplementedException();

        public async Task<bool> SaveAsync()
        {
            return await db.SaveChangesAsync()>0;
        }

        
    }
}
