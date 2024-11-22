using System.Text.RegularExpressions;
using System;

namespace WebGoat.NET.Models
{
    public class BlogContents
    {
        private string blogContents;

        public BlogContents(string blogContents)
        {
            IsBlogContentValid(blogContents);
            this.blogContents = blogContents;
        }
        
        public string GetValue()
        {
            return this.blogContents;
        }
        
        private void IsBlogContentValid(string blogContents)
        {
            string pattern = @"^[a-zA-ZæøåÆØÅ.,!?]+$";

            if (!Regex.IsMatch(blogContents, pattern))
            {
                throw new ArgumentException("XSS not allowed 🤬");
            }
        }
    }
}
