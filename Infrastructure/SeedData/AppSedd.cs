using Core.Data;
using Core.Entity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.SeedData
{
    public  class AppSedd
    {
        public static async Task SeedData (AppDBContext context,ILoggerFactory logger)
         {
			try
			{
				if (context.Categories!=null&&!context.Categories.Any())
				{
					var categories = File.ReadAllText("../Infrastructure/SeedData/Category.json");
					//deserialize 
					var category=JsonSerializer.Deserialize<List<Category>>(categories);
					if (category is not null) 
					{
						foreach(var item in category)
						{
							await context.Categories.AddAsync(item);	
							await context.SaveChangesAsync();
						}
					}
				}

				if (context.ProductBrands!=null&&!context.ProductBrands.Any())
				{
					var productbrands = File.ReadAllText("../Infrastructure/SeedData/brands.json");
					//deserialize 
					var brands=JsonSerializer.Deserialize<List<ProductBrand>>(productbrands);
					if (brands is not null) 
					{
						foreach(var item in brands)
						{
							await context.ProductBrands.AddAsync(item);	
							await context.SaveChangesAsync();
						}
					}
				}


				if (context.ProductTypes!=null&&!context.ProductTypes.Any())
				{
					var productTypes = File.ReadAllText("../Infrastructure/SeedData/types.json");
					//deserialize 
					var Type=JsonSerializer.Deserialize<List<ProductType>>(productTypes);
					if (Type is not null) 
					{
						foreach(var item in Type)
						{
							await context.ProductTypes.AddAsync(item);	
							await context.SaveChangesAsync();
						}
					}
				}

				if (context.Products!=null&&!context.Products.Any())
				{
					var Products = File.ReadAllText("../Infrastructure/SeedData/products.json");
					//deserialize 
					var product=JsonSerializer.Deserialize<List<Product>>(Products);
					if (product is not null) 
					{
						foreach(var item in product)
						{
							await context.Products.AddAsync(item);	
							await context.SaveChangesAsync();
						}
					}
				}





			}
			catch (Exception ex )
			{
				var log =logger.CreateLogger<AppDBContext>();
				log.LogError(ex.Message);
			}
        }
    }
}
