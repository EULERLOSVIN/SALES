using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SALES.Persistence.Configurations
{
    public class SaleOrderConfiguration : IEntityTypeConfiguration<SaleOrder>
    {
        public void Configure(EntityTypeBuilder<SaleOrder> entity)
        {
            entity.HasKey(e => e.IdSaleOrder).HasName("PK__SaleOrde__F83E4024BFC5781D");

            entity.ToTable("SaleOrder");

            entity.Property(e => e.OrderDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TrackingNumber).HasMaxLength(100);

            entity.HasOne(d => d.IdSaleStateNavigation).WithMany(p => p.SaleOrders)
                .HasForeignKey(d => d.IdSaleState)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SaleOrder_SaleState");
        }
    }
}
