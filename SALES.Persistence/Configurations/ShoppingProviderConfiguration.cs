
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SALES.Persistence.Configurations
{
    public class ShoppingProviderConfiguration : IEntityTypeConfiguration<ShoppingProvider>
    {
        public void Configure(EntityTypeBuilder<ShoppingProvider> entity)
        {
            entity.HasKey(e => e.IdShoppingProvider).HasName("PK__Shopping__14A39A7408CEEC86");
            entity.ToTable("ShoppingProvider");
            entity.Property(e => e.NameProvider).HasMaxLength(100);
        }
    }
}

