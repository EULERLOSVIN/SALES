
using Microsoft.EntityFrameworkCore;
using SALES.Application.Interfaces;
namespace SALES.Persistence.Context;

public partial class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }

    public virtual DbSet<SaleOrder> SaleOrders { get; set; }

    public virtual DbSet<SalePayment> SalePayments { get; set; }

    public virtual DbSet<SaleState> SaleStates { get; set; }

    public virtual DbSet<Shipping> Shippings { get; set; }

    public virtual DbSet<ShoppingProvider> ShoppingProviders { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
