using MediatR;
using Microsoft.AspNetCore.Mvc;
using SALES.Application.DTOs;
using SALES.Application.Features.Sale.Command;
using SALES.Application.Features.Sale.Query;
using System.Threading.Tasks;

namespace SALES.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SaleController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SaleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("process-payment")]
        public async Task<IActionResult> ProcessPayment([FromBody] CreateSaleDto saleDto)
        {
            var command = new CreateSaleCommand(saleDto);
            var result = await _mediator.Send(command);

            if (result)
            {
                return Ok(new
                {
                    Success = true,
                    Message = "Venta procesada correctamente."
                });
            }
            else
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "No se pudo completar la venta."
                });
            }
        }

        [HttpGet("orders/{idUserAccount}")]
        public async Task<IActionResult> GetOrdersByUserId(int idUserAccount)
        {
            var query = new GetOrdersQuery(idUserAccount);
            var orders = await _mediator.Send(query);

            if (orders == null || orders.Count == 0)
            {
                return NotFound("No se encontraron órdenes para este usuario.");
            }

            return Ok(orders);
        }
    }
}