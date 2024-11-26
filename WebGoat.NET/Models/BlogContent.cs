using System.Text.RegularExpressions;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WebGoat.NET.Models
{
    public class BlogContent
    {
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
            // Define the regular expression pattern to allow only valid characters (letters, Danish characters, Spaces, punctuation and b, i, p, ol, li html-tags)
            string pattern = @"^(<(/?)(b|i|p|ol|li)>)|[a-zA-ZæøåÆØÅ.,!? ]+$";

            if (!Regex.IsMatch(blogContents, pattern))
            {

                throw new ArgumentException("XSS not allowed 🤬");
            }
        }

    }
}
