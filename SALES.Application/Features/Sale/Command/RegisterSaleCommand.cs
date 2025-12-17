using MediatR;
using SALES.Application.DTOs;
using SALES.Application.Interfaces;

namespace SALES.Application.Features.Sale.Command
{
    public record CreateSaleCommand(CreateSaleDto SaleData) : IRequest<bool>;

    public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, bool> 
    {
        public readonly ISalesService _salesService;
        public CreateSaleHandler(ISalesService salesService)
        {
            _salesService = salesService;
        }

        public async Task<bool> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
        {
            return await _salesService.ProcessSaleAsync(request.SaleData);
        }
    }
}
