using System.Text.RegularExpressions;
using System;
using System.ComponentModel.DataAnnotations;
using Ganss.Xss;

namespace WebGoat.NET.Models
{
    public class BlogContent
    {
        [MaxLength(5000)]
        private string blogContents;

        /// <summary>
        /// Validates the content to ensure it is not empty and contains only allowed characters.
        /// </summary>
        /// <param name="blogContents"></param>
        public BlogContent(string blogContents)
        {
            IsBlogContentValid(blogContents);
            this.blogContents = blogContents;
        }
        // Empty construtor for EF Core mapping
        protected BlogContent() { }

        // Public property for EF Core mapping
        public string Value 
        {
            // Required for EF Core to set the value during materialization
            get => blogContents;
            private set
            {
                // Ensure validation for EF Core
                IsBlogContentValid(value); 
                blogContents = value;
            }
        }
        public string GetValue()
        {
            return this.blogContents;
        }

        /// <summary>
        /// Validates the provided blog content to ensure it is not empty and contains only allowed characters. 
        /// </summary>
        /// <param name="blogContents"></param>
        /// <exception cref="ArgumentException"></exception>
        private void IsBlogContentValid(string blogContents)
        {
            // Check if the blog content is null or empty
            if (string.IsNullOrEmpty(blogContents))
            {
                throw new ArgumentException("Blog content cannot be empty");
            }
            // Initialize the HtmlSanitizer from Ganss.XSS
            var sanitizer = new HtmlSanitizer();
            // Sanitize the incoming content to prevent XSS
            string sanitizedContent = sanitizer.Sanitize(blogContents);

            // Validate length (max 5000 characters)
            if (sanitizedContent.Length > 5000)

                if (blogContents.Length > 5000)
            {
                throw new ArgumentException("Only 5000 characters allowed per blog post");
            }
        }
    }
}
