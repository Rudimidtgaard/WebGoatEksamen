#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
using System.Text.RegularExpressions;
using System;

namespace WebGoatCore.Models
{
    public class ContactName
    {
        private string value;

        public ContactName(string value)
        {
            IsContactNameValid(value);
            this.value = value;
        }
        protected ContactName() { }
        public string GetValue()
        {
            return this.value;
        }

        private void IsContactNameValid(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("ContactName cannot be empty");
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
