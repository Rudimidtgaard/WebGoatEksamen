#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
using System.Text.RegularExpressions;
using System;

namespace WebGoatCore.Models
{
    public class Address
    {
        private string value;

        public Address(string value)
        {
            IsAddressValid(value);
            this.value = value;
        }
        protected Address() { }
        public string GetValue()
        {
            return this.value;
        }

        private void IsAddressValid(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("Address cannot be empty");
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
