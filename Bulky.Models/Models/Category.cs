namespace PersonalProject.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? CategoryCover { get; set; }
        public List<ProductsCategory>? productsCategories { get; set; }

    }
}
