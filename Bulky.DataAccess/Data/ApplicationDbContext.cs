using PersonalProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PersonalProject.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {


        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            SeedRole(modelBuilder, "Admin");
            SeedRole(modelBuilder, "Editor");
            SeedRole(modelBuilder, "Users");

            modelBuilder.Entity<ProductsCart>().HasKey(
               ProductsCart => new
               {
                   ProductsCart.CartId,
                   ProductsCart.ProductId

               }
               );


            modelBuilder.Entity<ProductsCategory>().HasKey(
               ProductsCategory => new
               {
                   ProductsCategory.CategoryId,
                   ProductsCategory.ProductId
               }
               );



            modelBuilder.Entity<ProductsOrder>().HasKey(
               ProductsOrder => new
               {
                   ProductsOrder.OrderId,
                   ProductsOrder.ProductId
               }
               );

        }
        private void SeedRole(ModelBuilder modelBuilder, string roleName)
        {
            var role = new IdentityRole
            {
                Id = roleName.ToLower(),
                Name = roleName,
                NormalizedName = roleName.ToUpper(),
                ConcurrencyStamp = Guid.Empty.ToString()
            };
            modelBuilder.Entity<IdentityRole>().HasData(role);
        }
      
    


    public DbSet<Product> products { get; set; }
        public DbSet<Cart> carts { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<ProductsCart> ProductsCarts { get; set; }
        public DbSet<ProductsCategory> productsCategories { get; set; }
        public DbSet<ProductsOrder> productsOrders { get; set; }

      

    }
}
