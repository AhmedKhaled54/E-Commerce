using Core.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Confiquration
{
    public class ProductConfiquration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(c => c.Name).IsRequired();
            builder.Property(c => c.Price).IsRequired()
                .HasColumnType("decimal(18,2)");
            builder.Property(c => c.Image).IsRequired();
            builder.Property(c => c.Description).IsRequired();

            builder.HasOne(c => c.ProductBrand)
               .WithMany()
               .HasForeignKey(c => c.Productbrand_Id);
            builder.HasOne(c => c.ProductType)
                .WithMany()
                .HasForeignKey(c => c.productType_Id);

        }
    }
}
