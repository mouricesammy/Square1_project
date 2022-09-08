using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebBlogApi.Data.Interfaces;
using WebBlogApi.Models;
using Microsoft.EntityFrameworkCore;


using System.Linq;
using System.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


using System.IO;
using System.Data;
using System.Text;
using System.Net.Mime;

using System.Security.Cryptography;

using System.Web;



namespace WebBlogApi.Data.Repo
{
    public class BlogRepository : IBlogRepository
    {
        private readonly WebBlogApiDbContext db;

        public BlogRepository(WebBlogApiDbContext db)
        {
            this.db = db;
        }
        public async Task<IEnumerable<Blogs>> GetAllBlogsAsync()
        {
            return await db.Blogs.ToListAsync();
        }

        public async Task<IEnumerable<Blogs>> FindClientBlogs(int UserId)
        {
            return (IEnumerable<Blogs>) await db.Blogs.FindAsync(UserId);
        }
        //public UrlBlogs GetBlogsFromUrl(string url)
        //{
        //    RestClient rclient = new RestClient
        //    {
        //        endPoint = url,
        //        httpMethod = HttpVerb.GET,
        //        postJSON = @"{}"
        //    };

        //    string strResponse = rclient.MakeRequest();

        //    UrlBlogs blogs = JsonConvert.DeserializeObject<UrlBlogs>(strResponse);

        //    return  blogs;
        //}

        

    }
}

