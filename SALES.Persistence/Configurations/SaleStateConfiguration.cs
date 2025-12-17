
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SALES.Persistence.Configurations
{
    public class SaleStateConfiguration : IEntityTypeConfiguration<SaleState>
    {
        public void Configure(EntityTypeBuilder<SaleState> entity)
        {
            entity.HasKey(e => e.IdSaleState).HasName("PK__SaleStat__BDD1E03C76F6970E");
            entity.ToTable("SaleState");
            entity.Property(e => e.StateName).HasMaxLength(50);
        }
    }
}
