using SALES.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SALES.Application.Interfaces
{
    public interface IProductIntegrationService
    {
        Task<bool> UpdateStockAsync(List<SaleItemDto> items);
    }
}