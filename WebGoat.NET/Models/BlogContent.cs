using System.Text.RegularExpressions;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WebGoat.NET.Models
{
    public class BlogContent
    {
        private string blogContents;

        public BlogContent(string blogContents)
        {
            IsBlogContentValid(blogContents);
            this.blogContents = blogContents;
        }
        protected BlogContent() { }

        public string Value // Public property for EF Core mapping
        {
            get => blogContents;
            private set => blogContents = value; // Required for EF Core to set the value during materialization
        }
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

            string pattern = @"^[a-zA-ZæøåÆØÅ.,!? ]+$";


            if (!Regex.IsMatch(blogContents, pattern))
            {

                throw new ArgumentException("XSS not allowed 🤬");
            }
        }

    }
}
