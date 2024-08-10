using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Products
    {
        public int ID { get; set; }
        public string PName { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountedPrice { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; }

    }
}
