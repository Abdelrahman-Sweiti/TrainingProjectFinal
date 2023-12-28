namespace PersonalProject.Models
{
    public class ProductsCategory
    {
        public int CategoryId { get; set; }
        public int ProductId { get; set; }


        public Category? category { get; set; }
        public Product? product { get; set; }

    }
}
