using System;
using System.Collections.Generic;

namespace WebBlogApi.Models
{
    public class UrlBlogs
    {
        public List<Datum> data { get; set; }
    }

    public class Datum
    {
        public string title { get; set; }
        public string description { get; set; }
        public string publication_date { get; set; }
    }
}

