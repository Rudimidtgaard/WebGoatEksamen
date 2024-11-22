using System.Text.RegularExpressions;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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
        protected BlogContents() { }
        public string GetValue()
        {
            return this.blogContents;
        }
        
        private void IsBlogContentValid(string blogContents)
        {
            if (string.IsNullOrEmpty(blogContents))
            {
                throw new ArgumentException("Blog content cannot be empty");
            }

            string pattern = @"^[a-zA-ZæøåÆØÅ.,!?]+$";

            if (!Regex.IsMatch(blogContents, pattern))
            {

                throw new ArgumentException("XSS not allowed 🤬");
            }
        }
    }
}
