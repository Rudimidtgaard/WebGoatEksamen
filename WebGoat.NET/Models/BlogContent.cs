using System.Text.RegularExpressions;
using System;
<<<<<<< Updated upstream
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
=======
using System.ComponentModel.DataAnnotations;
using AngleSharp.Io;
using Ganss.Xss;
>>>>>>> Stashed changes

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

<<<<<<< Updated upstream
            string pattern = @"^[a-zA-ZæøåÆØÅ.,!?]+$";

            if (!Regex.IsMatch(blogContents, pattern))
=======
            // Initialize the HtmlSanitizer from Ganss.XSS
            var sanitizer = new HtmlSanitizer();
            // Sanitize the incoming content to prevent XSS
            string sanitizedContent = sanitizer.Sanitize(blogContents);

            // Validate length (max 5000 characters)
            if (sanitizedContent.Length > 5000)
>>>>>>> Stashed changes
            {

                throw new ArgumentException("XSS not allowed 🤬");
            }
        }

    }
}
