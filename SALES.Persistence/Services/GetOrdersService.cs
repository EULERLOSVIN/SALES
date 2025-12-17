

using Microsoft.EntityFrameworkCore;
using SALES.Application.DTOs;
using SALES.Application.Interfaces;

namespace SALES.Persistence.Services
{
    public class GetOrdersService : IGetOrdersService
    {
        private readonly IApplicationDbContext _context;
        public GetOrdersService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<OrderDto>> GetOrders(int idUserAccount)
        {
            var query = await _context.SaleOrders
                // 1. Filtro
                .Where(o => o.IdUserAccount == idUserAccount)
                .Include(o => o.IdSaleStateNavigation)
                .Include(o => o.Shippings).ThenInclude(s => s.IdShoppingProviderNavigation)
                .Include(o => o.SalePayments)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            var result = query.Select(order =>
            {
                var shippingDetail = order.Shippings.FirstOrDefault();
                var paymentDetail = order.SalePayments.FirstOrDefault();

                if (shippingDetail == null || paymentDetail == null)
                {
                    return null;
                }

                return new OrderDto
                {
                    IdOrder = order.IdSaleOrder,
                    TrackingNumber = order.TrackingNumber ?? string.Empty,
                    DateTime = order.OrderDate,
                    State = order.IdSaleStateNavigation.StateName,

                    SalePayment = new SalePaymentDto
                    {
                        IdSalePayment = paymentDetail.IdSalePayment,
                        IdCart = paymentDetail.IdCart,
                        SubTotal = paymentDetail.SubTotal,
                        ShippingCost = paymentDetail.ShippingCost,
                        Total = paymentDetail.TotalAmount
                    },

                    DetailShipping = new DetailShipping
                    {
                        IdDetailShipping = shippingDetail.IdShipping,
                        ReceiverName = shippingDetail.ReceiverName!,
                        RceiverLastName = shippingDetail.ReceiverLastName!,
                        PhoneNumber = shippingDetail.PhoneNumber,
                        Dni = shippingDetail.Dni!,
                        City = shippingDetail.City!,
                        Region = shippingDetail.Region!,
                        Province = shippingDetail.Province!,
                        District = shippingDetail.District!,
                        AddressReference = shippingDetail.AddressReference,

                        Provider = new ShippingProviderDTo
                        {
                            IdShoppingProvider = shippingDetail.IdShoppingProviderNavigation.IdShoppingProvider,
                            NameProvider = shippingDetail.IdShoppingProviderNavigation.NameProvider
                        }
                    }
                };
            })

            .Where(o => o != null)
            .ToList()!;

            return result!;
        }
    
    }
}
