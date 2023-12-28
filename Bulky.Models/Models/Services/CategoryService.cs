using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using PersonalProject.Data;
using PersonalProject.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace PersonalProject.Models.Services
{
    public class CategoryService : ICategory
    {
        private readonly ApplicationDbContext _context;
        IConfiguration _configration;

        public CategoryService(ApplicationDbContext context, IConfiguration configration)
        {
            _context = context;
              _configration = configration;
        }


        public async Task<Category> Create(Category category)
        {
            _context.Add(category);
            await _context.SaveChangesAsync();
            return category;

        }

        public async Task<List<Category>> GetCategories()
        {
            return await _context.categories.Include(x => x.productsCategories).ThenInclude(y => y.product).ToListAsync();
        }


        public async Task<Category> GetCategoryById(int id)
        {
            var category = await _context.categories.FirstOrDefaultAsync(x=>x.Id==id);
            return category;
        }

        public async Task<Product> AddProductToCategories(int categoryId, Product product)
        {

           
            _context.Entry(product).State = EntityState.Added;

            await _context.SaveChangesAsync();
            ProductsCategory categoryProduct = new ProductsCategory()
            {
                ProductId = product.Id,
                CategoryId = categoryId
            };

            _context.Entry(categoryProduct).State = EntityState.Added;

            await _context.SaveChangesAsync();

            return product;
        }

        public async Task<Category> GetFile(IFormFile file, Category category)
        {
            BlobContainerClient container = new BlobContainerClient(_configration.GetConnectionString("StorageConnection"), "images");
            await container.CreateIfNotExistsAsync();
            BlobClient blob = container.GetBlobClient(file.FileName);

            using var stream = file.OpenReadStream();
            BlobUploadOptions options = new BlobUploadOptions()
            {
                HttpHeaders = new BlobHttpHeaders() { ContentType = file.ContentType }
            };

            if (!await blob.ExistsAsync())
            {
                await blob.UploadAsync(stream, options);
            }

            category.CategoryCover = blob.Uri.ToString();

            return category;
        }


        public async Task<Product> GetFile(IFormFile file, Product product)
        {
            BlobContainerClient container = new BlobContainerClient(_configration.GetConnectionString("StorageConnection"), "images");
            await container.CreateIfNotExistsAsync();
            BlobClient blob = container.GetBlobClient(file.FileName);

            using var stream = file.OpenReadStream();
            BlobUploadOptions options = new BlobUploadOptions()
            {
                HttpHeaders = new BlobHttpHeaders() { ContentType = file.ContentType }
            };

            if (!await blob.ExistsAsync())
            {
                await blob.UploadAsync(stream, options);
            }

            product.ImageUri = blob.Uri.ToString();

            return product;
        }


        public async Task<Category> UpdateCategoryAsync(int id, Category category)
        {
            var existingCategory = await _context.categories.FindAsync(id);

            if (existingCategory == null)
            {
                return null; // Or throw an exception indicating not found
            }

            // Update product properties
            existingCategory.Name = category.Name;
            existingCategory.Description = category.Description;

            // Only update the image URL if a new image is provided
            if (category.CategoryCover != null)
            {
                existingCategory.CategoryCover = category.CategoryCover;
            }

            // Assuming DepartmentID is a foreign key, update it if provided


            _context.Entry(existingCategory).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return existingCategory;
        }
    }
}
