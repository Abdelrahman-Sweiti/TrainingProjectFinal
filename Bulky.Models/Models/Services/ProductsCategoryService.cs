using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using PersonalProject.Data;
using PersonalProject.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace PersonalProject.Models.Services
{
    public class ProductsCategoryService : IProductsCategory
    {

        private readonly ApplicationDbContext _context;
        IConfiguration _configration;


        public ProductsCategoryService(ApplicationDbContext context, IConfiguration configration)
        {
            _context = context;
            _configration = configration;

        }
        public async Task<List<ProductsCategory>> GetAllProductsForCategory(int categoryId)
        {
            var productsForCategory = await _context.productsCategories
                .Include(pc => pc.product)
                .Where(pc => pc.CategoryId == categoryId)
                .ToListAsync();

            return productsForCategory;
        }


        public async Task<ProductsCategory> GetProductCategoryById(int id)
        {
            var productCategory = await _context.productsCategories.FirstOrDefaultAsync(x => x.CategoryId == id);
            return productCategory;
        }


        public async Task<Uri> GetFile(IFormFile file)
        {
            if (file == null)
            {
                Uri defaultImg = new Uri("https://faststorestorage.blob.core.windows.net/images/DefaultIMG.png");
                return defaultImg;
            }
            BlobContainerClient container = new BlobContainerClient(_configration.GetConnectionString("AzureBlob"), "images");
            await container.CreateIfNotExistsAsync();
            BlobClient blob = container.GetBlobClient(file.FileName);

            using var stream = file.OpenReadStream();
            BlobUploadOptions options = new BlobUploadOptions()
            {
                HttpHeaders = new BlobHttpHeaders() { ContentType = file.ContentType }
            };
            if (!blob.Exists())
            {
                await blob.UploadAsync(stream, options);
            }
            return blob.Uri;

        }



    }
}
