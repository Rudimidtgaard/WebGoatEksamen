#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
using System.Text.RegularExpressions;
using System;

namespace WebGoatCore.Models
{
    public class ContactTitle
    {
        private string value;

        public ContactTitle(string value)
        {
            IsContactTitleValid(value);
            this.value = value;
        }
        protected ContactTitle() { }
        public string GetValue()
        {
            return this.value;
        }

        private void IsContactTitleValid(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("ContactTitle cannot be empty");
            }

            if (value.Length >= 2 && value.Length <= 15)
            {
                string pattern = @"^[A-Za-zÆØÅæøå\s\-]+$";

                if (!Regex.IsMatch(value, pattern))
                {

                    throw new ArgumentException("XSS not allowed 🤬");
                }
            }

        }
    }
}
