using PersonalProject.Data;
using PersonalProject.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace PersonalProject.Models.Services
{
    public class CartService : ICart
    {
        private readonly ApplicationDbContext _context;

        public CartService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Cart> GetOrCreateCartAsync(string userId)
        {
            // Attempt to retrieve the user's cart from the database.
            var cart = await _context.carts
                .Include(c => c.productsCarts)
                .ThenInclude(pc => pc.product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            // If the cart does not exist, create a new one.
            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId
                };

                // Add the new cart to the context and save changes.
                _context.carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            return cart;
        }

        public async Task<List<ProductsCart>> GetProductsInCartAsync(string userId)
        {
            // Find the user's cart
            var cart = await _context.carts
                .Include(c => c.productsCarts)
                .ThenInclude(pc => pc.product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                return new List<ProductsCart>(); // Return an empty list if the user has no cart
            }

            // Extract the products from the cart
            var productsCarts = cart.productsCarts.ToList();
            return productsCarts;
        }
        public async Task<int> GetCartItemCountAsync(string userId)
        {
            // Find the user's cart
            var cart = await _context.carts
                .Include(c => c.productsCarts)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                return 0; // Return 0 if the user has no cart
            }

            // Calculate the total quantity of products in the cart
            var totalQuantity = cart.productsCarts.Sum(pc => pc.Quantity);

            return totalQuantity;
        }
        public async Task<bool> RemoveProductFromCartAsync(int productId, string userId)
        {
            // Find the user's cart
            var cart = await _context.carts
                .Include(c => c.productsCarts)
                .ThenInclude(pc => pc.product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                return false; // Cart not found
            }

            // Find the product in the cart
            var productCart = cart.productsCarts.FirstOrDefault(pc => pc.product.Id == productId);

            if (productCart == null)
            {
                return false; // Product not found in the cart
            }

            // Remove the product from the cart
            cart.productsCarts.Remove(productCart);

            // Save changes to the database
            await _context.SaveChangesAsync();

            return true; // Product successfully removed from the cart
        }


    }
}
