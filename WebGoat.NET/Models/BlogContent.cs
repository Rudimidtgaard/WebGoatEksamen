using System.Text.RegularExpressions;
using System;
using System.ComponentModel.DataAnnotations;

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
            // Define the regular expression pattern to allow only valid characters
            // (Alphanumeric, Spaces, punctuation and b, p, i, ol, li HTML-Tags)
            string pattern = @"^(?:[a-zA-Z0-9.,!?'<b></b><p></p><i></i><ol></ol><li></li>]|\s)*$";

                if (!Regex.IsMatch(blogContents, pattern))
                {

                    throw new ArgumentException("XSS not allowed 🤬");
                }

            if (blogContents.Length > 5000)
            {
                throw new ArgumentException("Only 5000 characters allowed per blog post");
            }
        }
    }
}
