namespace PersonalProject.Models.Interfaces
{
    public interface ICart
    {
          Task<Cart> GetOrCreateCartAsync(string userId);
        Task<List<ProductsCart>> GetProductsInCartAsync(string userId);
        Task<int> GetCartItemCountAsync(string username);
        Task<bool> RemoveProductFromCartAsync(int productId, string userId);

    }
}
