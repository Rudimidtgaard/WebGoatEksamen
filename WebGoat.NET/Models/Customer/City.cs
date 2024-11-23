#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
using System.Text.RegularExpressions;
using System;

namespace WebGoatCore.Models
{
    public class City
    {
        private string value;

        public City(string value)
        {
            IsCityValid(value);
            this.value = value;
        }
        protected City() { }
        public string GetValue()
        {
            return this.value;
        }

        private void IsCityValid(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("City cannot be empty");
            }

            if (value.Length >= 2 && value.Length <= 30)
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
