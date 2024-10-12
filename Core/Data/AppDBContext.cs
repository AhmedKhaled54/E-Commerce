using Core.Entity;
using Core.Entity.CartEntity;
using Core.Entity.OrderEntity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data
{
    public  class AppDBContext:IdentityDbContext<User>
    {
        public AppDBContext()
        {
             
        }
        public AppDBContext(DbContextOptions<AppDBContext> options):base(options) 
        {
             
        }

        protected override void OnModelCreating(ModelBuilder model)
        {
            model.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(model);
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductBrand> ProductBrands { get; set; } 
        public DbSet<ProductType>  ProductTypes{ get; set; } 
        public DbSet<Category>  Categories { get; set; } 
        public DbSet<Cart> Carts { get; set; } 
        public DbSet<CartItem> CartItems { get; set; }  
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<WishList> WishList { get; set; }   
        public DbSet<Rate> Rate { get; set; }

    }
}
