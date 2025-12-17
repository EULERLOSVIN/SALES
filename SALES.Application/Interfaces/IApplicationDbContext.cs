using Microsoft.EntityFrameworkCore;
using SALES.Persistence;

namespace SALES.Application.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<PaymentMethod> PaymentMethods { get; }
        DbSet<SaleOrder> SaleOrders { get; }
        DbSet<SalePayment> SalePayments { get; }
        DbSet<SaleState> SaleStates { get; }
        DbSet<Shipping> Shippings { get; }
        DbSet<ShoppingProvider> ShoppingProviders { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
