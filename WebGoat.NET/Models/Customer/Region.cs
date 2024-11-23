#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
using System.Text.RegularExpressions;
using System;

namespace WebGoatCore.Models
{
    public class Region
    {
        private string value;

        public Region(string value)
        {
            IsRegionValid(value);
            this.value = value;
        }
        protected Region() { }
        public string GetValue()
        {
            return this.value;
        }

        private void IsRegionValid(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("Region cannot be empty");
            }

            if (value.Length >= 8 && value.Length <= 11)
            {
                string pattern = @"^(Region\s)?(Hovedstaden|Midtjylland|Nordjylland|Sjælland|Syddanmark)$";

                if (!Regex.IsMatch(value, pattern))
                {

                    throw new ArgumentException("XSS not allowed 🤬");
                }
            }

        }
    }
}
