using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBlogApi.Models
{
    public class Blogs
    {
        public int id { get; set; }

        [ForeignKey("User")]
        public int user_id { get; set; }
        [Required]
        public string title { get; set; }
        [Required]
        public string description { get; set; }
        [Required]
        public DateTime Publication_date { get; set; }
    }
}

