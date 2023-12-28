namespace Bulky.Models.Models
{
    public class ProductsCart
    {
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }


        public Product? product { get; set; }
        public Cart? cart { get; set; }
    }
}
