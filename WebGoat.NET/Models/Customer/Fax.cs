#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
using System.Text.RegularExpressions;
using System;

namespace WebGoatCore.Models
{
    public class Fax
    {
        private string value;

        public Fax(string value)
        {
            IsFaxValid(value);
            this.value = value;
        }
        protected Fax() { }
        public string GetValue()
        {
            return this.value;
        }

        private void IsFaxValid(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("Fax cannot be empty");
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
