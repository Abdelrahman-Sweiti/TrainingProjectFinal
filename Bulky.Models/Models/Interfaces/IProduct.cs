namespace PersonalProject.Models.Interfaces
{
    public interface IProduct
    {
        Task<Product> Create(Product product  );
        Task<ProductsCategory> AddCategoryToProduct(int categoryId, int productId);
        Task<Product> GetFile(IFormFile file, Product product);
        Task<Product> GetProductById(int productId);
        Task<Product> UpdateProductAsync(int id, Product product  );

    }
}
