namespace PersonalProject.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public decimal TotalPrice { get; set; }
        public int Count { get; set; }


        public List<ProductsCart>? productsCarts { get; set; }

    }
}
