#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
using System.Text.RegularExpressions;
using System;

namespace WebGoatCore.Models
{
    public class CompanyName
    {
        private string value;

        public CompanyName(string value)
        {
            IsCompanyNameValid(value);
            this.value = value;
        }
        protected CompanyName() { }
        public string GetValue()
        {
            return this.value;
        }

        private void IsCompanyNameValid(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("CompanyName cannot be empty");
            }

            if (value.Length >= 2 && value.Length <= 25)
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
