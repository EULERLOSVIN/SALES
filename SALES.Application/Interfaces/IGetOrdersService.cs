

using SALES.Application.DTOs;

namespace SALES.Application.Interfaces
{
    public interface IGetOrdersService
    {
        Task<List<OrderDto>> GetOrders(int idUserAccount);
    }
}
