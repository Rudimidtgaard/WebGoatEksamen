#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
using System.Text.RegularExpressions;
using System;

namespace WebGoatCore.Models
{
    public class Phone
    {
        private string value;

        public Phone(string value)
        {
            IsPhoneValid(value);
            this.value = value;
        }
        protected Phone() { }
        public string GetValue()
        {
            return this.value;
        }

        private void IsPhoneValid(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("Phone cannot be empty");
            }

            if (value.Length >= 8 && value.Length <= 16)
            {
                string pattern = @"^(\+45\s?)?(\(?\d{2}\)?[\s\-]?)?\d{2}[\s\-]?\d{2}[\s\-]?\d{2}$";

                if (!Regex.IsMatch(value, pattern))
                {

                    throw new ArgumentException("XSS not allowed 🤬");
                }
            }

        }
    }
}
