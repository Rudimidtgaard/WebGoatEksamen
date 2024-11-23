#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
using System.Text.RegularExpressions;
using System;

namespace WebGoatCore.Models
{
    public class PostalCode
    {
        private string value;

        public PostalCode(string value)
        {
            IsPostalCodeValid(value);
            this.value = value;
        }
        protected PostalCode() { }
        public string GetValue()
        {
            return this.value;
        }

        private void IsPostalCodeValid(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("PostalCode cannot be empty");
            }

            if (value.Length >= 2 && value.Length <= 60)
            {
                string pattern = @"^([A-Za-zÆØÅæøåÀ-ÖØ-öø-ÿ\s\.\'\-]+)\s+(\d+[A-Za-z]?)\s*,?\s*(\d+[A-Za-z]?\s*(th|tv|st|etg|fl|fl.|etc|[A-Za-z])*)?$";

                if (!Regex.IsMatch(value, pattern))
                {

                    throw new ArgumentException("XSS not allowed 🤬");
                }
            }

        }
    }
}
