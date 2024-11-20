using WebGoatCore.Models;

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
namespace WebGoatCore.ViewModels
{
    public class OrderEditViewModel
    {
        public bool AddsNew { get; set; }
        public Product? Product { get; set; }
        public Order Order { get; set; }
        public OrderDetail OrderDetail { get; set; }
        public OrderPayment Payment { get; set; }
    }
}
