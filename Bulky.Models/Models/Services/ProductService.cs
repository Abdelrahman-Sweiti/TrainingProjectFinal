using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using PersonalProject.Data;
using PersonalProject.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace PersonalProject.Models.Services
{
    public class ProductService  : IProduct
    {
        private readonly IConfiguration _configration;
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context, IConfiguration configration)
        { 
            _context = context;
            _configration = configration;

        }

        public async Task<Product> Create(Product product)
        {
            _context.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }



        public async Task<Product> GetProductById(int productId)
        {
            // You can use your DbContext or any other method to retrieve the product by its ID.
            // Replace "YourDbContext" with your actual DbContext class.
            var product = await _context.products.FindAsync(productId);

            return product;
        }


        public async Task<ProductsCategory> AddCategoryToProduct(int categoryId, int productId)
        {



            ProductsCategory categoryProduct = new ProductsCategory()
            {
                ProductId = productId,
                CategoryId = categoryId
            };

            _context.Entry(categoryProduct).State = EntityState.Added;

            await _context.SaveChangesAsync();

            return categoryProduct;
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



        public async Task<Product> UpdateProductAsync(int id, Product product  )
        {
            var existingProduct = await _context.products.FindAsync(id);

            if (existingProduct == null)
            {
                return null; // Or throw an exception indicating not found
            }

            // Update product properties
            existingProduct.Price = product.Price;
            existingProduct.ProductName = product.ProductName;
            existingProduct.Description = product.Description;

            // Only update the image URL if a new image is provided
            if (product.ImageUri != null)
            {
                existingProduct.ImageUri = product.ImageUri;
            }

            // Assuming DepartmentID is a foreign key, update it if provided
           

            _context.Entry(existingProduct).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return existingProduct;
        }

    }



}

