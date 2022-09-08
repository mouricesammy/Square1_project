using System;
using WebBlogApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebBlogApi.Data.Interfaces
{
    public interface IBlogRepository
    {
        Task<IEnumerable<Blogs>> FindClientBlogs(int userId);
        Task<IEnumerable<Blogs>> GetAllBlogsAsync();
        //UrlBlogs GetUrlBlogs(string url);
    }
}

