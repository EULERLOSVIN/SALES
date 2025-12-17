using SALES.Application.DTOs;
using System.Threading.Tasks;

namespace SALES.Application.Interfaces
{
    public interface ISalesService
    {
        Task<bool> ProcessSaleAsync(CreateSaleDto saleDto);
    }
}