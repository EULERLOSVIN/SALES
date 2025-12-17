using System.Threading.Tasks;

namespace SALES.Application.Interfaces
{
    public interface ICartIntegrationService
    {
        Task<bool> CloseCartAsync(int idCart);
    }
}