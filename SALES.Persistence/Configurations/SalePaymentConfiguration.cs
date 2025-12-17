using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SALES.Persistence.Configurations
{
    public class SalePaymentConfiguration : IEntityTypeConfiguration<SalePayment>
    {
        public void Configure(EntityTypeBuilder<SalePayment> entity)
        {
            entity.HasKey(e => e.IdSalePayment).HasName("PK__SalePaym__BA969F459EDB05AE");

            entity.ToTable("SalePayment");

            entity.Property(e => e.PaymentDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ShippingCost).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.SubTotal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TransactionId).HasMaxLength(100);

            entity.HasOne(d => d.IdPaymentMethodNavigation).WithMany(p => p.SalePayments)
                .HasForeignKey(d => d.IdPaymentMethod)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalePayment_PaymentMethod");

            entity.HasOne(d => d.IdSaleOrderNavigation).WithMany(p => p.SalePayments)
                .HasForeignKey(d => d.IdSaleOrder)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalePayment_SaleOrder");
        }
    }
}
