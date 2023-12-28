using Microsoft.AspNetCore.Mvc;

namespace PersonalProject.Models.Interfaces
{
    public interface ICategory
    {
        Task<List<Category>> GetCategories();
        Task<Category> GetCategoryById(int id);
        Task<Product> AddProductToCategories(int categoriesId, Product product  );
        Task<Category> GetFile(IFormFile file, Category category);
        Task<Category> Create(Category category  );
        Task<Product> GetFile(IFormFile file, Product product);
        Task<Category> UpdateCategoryAsync(int id, Category category);

    }
}
