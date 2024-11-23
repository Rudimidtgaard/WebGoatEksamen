#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
using System.Text.RegularExpressions;
using System;

namespace WebGoatCore.Models
{
    public class Country
    {
        private string value;

        public Country(string value)
        {
            IsCountryValid(value);
            this.value = value;
        }
        protected Country() { }
        public string GetValue()
        {
            return this.value;
        }

        private void IsCountryValid(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("Country cannot be empty");
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
