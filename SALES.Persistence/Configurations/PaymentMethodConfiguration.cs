
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SALES.Persistence;

namespace SALES.Persistence.Configurations
{
    public class PaymentMethodConfiguration : IEntityTypeConfiguration<PaymentMethod>
    {
        public void Configure(EntityTypeBuilder<PaymentMethod> entity)
        {
            entity.HasKey(e => e.IdPaymentMethod).HasName("PK__PaymentM__92CFFF96D4AFB505");
            entity.ToTable("PaymentMethod");
            entity.Property(e => e.MethodName).HasMaxLength(50);
        }
    }
}
