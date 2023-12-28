namespace PersonalProject.Models
{
    public class ProductsOrder
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public Order order { get; set; }
        public Product product { get; set; }
    }
}
