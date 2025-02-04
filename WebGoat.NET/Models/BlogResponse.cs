﻿using System;
using WebGoat.NET.Models;

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
namespace WebGoatCore.Models
{
    public class BlogResponse
    {
        public int Id { get; set; }
        public int BlogEntryId { get; set; }
        public BlogContent Content { get; set; }
        public string Author { get; set; }
        public DateTime ResponseDate { get; set; }

        public virtual BlogEntry BlogEntry { get; set; }

        public BlogResponse(BlogContent blogContents) 
        { 
            this.Content = blogContents;    
        }
        public BlogResponse()
        {

        }
    }
}
