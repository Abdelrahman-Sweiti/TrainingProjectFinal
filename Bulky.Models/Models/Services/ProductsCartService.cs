using PersonalProject.Data;
using PersonalProject.Models;
using PersonalProject.Models.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace PersonalProject.Models.Services
{
    public class ProductsCartService : IProductsCart
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProductsCartService(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<bool> AddProductToCartAsync(int cartId, int productId, int quantity)
        {
            // Find the cart by ID
            var cart = await _context.carts
                .Include(c => c.productsCarts)
                .FirstOrDefaultAsync(c => c.Id == cartId);

            if (cart == null)
            {
                return false; // Cart not found
            }

            // Find the product by ID
            var product = await _context.products.FindAsync(productId);
            if (product == null)
            {
                return false; // Product not found
            }

            // Check if the product is already in the cart; if so, update the quantity
            var existingProductCart = cart.productsCarts.FirstOrDefault(pc => pc.ProductId == productId);
            if (existingProductCart != null)
            {
                existingProductCart.Quantity += quantity;
            }
            else
            {
                // Create a new ProductsCart entry
                var productsCart = new ProductsCart
                {
                    CartId = cart.Id,
                    ProductId = product.Id,
                    Quantity = quantity
                };

                // Add the ProductsCart entry to the cart
                cart.productsCarts.Add(productsCart);
            }

            // Update the total price and count of the cart
            cart.TotalPrice += product.Price * quantity;
            cart.Count += quantity;

            await _context.SaveChangesAsync();
            return true;
        }

    }
}
