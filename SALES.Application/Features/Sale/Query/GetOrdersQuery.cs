using MediatR;
using SALES.Application.DTOs;
using SALES.Application.Interfaces;

namespace SALES.Application.Features.Sale.Query
{
    public record GetOrdersQuery(int idUserAccount): IRequest<List<OrderDto>>;

    public class GetOrdersHandler : IRequestHandler<GetOrdersQuery, List<OrderDto>>
    {
        private readonly IGetOrdersService _ordersService;

        public GetOrdersHandler(IGetOrdersService ordersService)
        {
            _ordersService = ordersService;
        }

        public async Task<List<OrderDto>> Handle(GetOrdersQuery query, CancellationToken cancellationToken)
        {
            var orders = await _ordersService.GetOrders(query.idUserAccount);
            return orders;
        }
    }
}