#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
namespace WebGoatCore.Models
{
    public class Customer
    {
        public string CustomerId { get; set; }
        public CompanyName CompanyName { get; set; }
        public ContactName ContactName { get; set; }
        public ContactTitle? ContactTitle { get; set; }
        public Address? Address { get; set; }
        public City? City { get; set; }
        public Region? Region { get; set; }
        public PostalCode? PostalCode { get; set; }
        public Country? Country { get; set; }
        public Phone? Phone { get; set; }
        public Fax? Fax { get; set; }
    }
}
