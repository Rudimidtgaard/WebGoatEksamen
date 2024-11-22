using System.Text.RegularExpressions;
using System;

namespace WebGoat.NET.Models
{
    public class BlogContents
    {
        private string _blogContents { get; set; }

        public BlogContents(string _blogContents)
        {
            IsBlogContentValid(_blogContents);
            this._blogContents = _blogContents;
        }

        public string GetValue()
        {
            return _blogContents;
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
