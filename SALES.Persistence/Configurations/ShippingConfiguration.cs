using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SALES.Persistence.Configurations
{
    public class ShippingConfiguration : IEntityTypeConfiguration<Shipping>
    {
        public void Configure(EntityTypeBuilder<Shipping> entity)
        {
            entity.HasKey(e => e.IdShipping).HasName("PK__Shipping__119E7F68872A2FE4");

            entity.ToTable("Shipping");

            entity.Property(e => e.AddressReference)
                  .HasMaxLength(250)
                  .IsRequired();
            // -------------------------

            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.District).HasMaxLength(100);
            entity.Property(e => e.Dni).HasMaxLength(20);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.PostalCode).HasMaxLength(20);
            entity.Property(e => e.Province).HasMaxLength(100);
            entity.Property(e => e.ReceiverLastName).HasMaxLength(100);
            entity.Property(e => e.ReceiverName).HasMaxLength(100);
            entity.Property(e => e.Region).HasMaxLength(100);

            entity.HasOne(d => d.IdSaleOrderNavigation).WithMany(p => p.Shippings)
                .HasForeignKey(d => d.IdSaleOrder)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Shipping_SaleOrder");

            entity.HasOne(d => d.IdShoppingProviderNavigation).WithMany(p => p.Shippings)
                .HasForeignKey(d => d.IdShoppingProvider)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Shipping_ShoppingProvider");
        }
    }
}