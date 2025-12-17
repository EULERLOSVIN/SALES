using MassTransit;
using Microsoft.EntityFrameworkCore;
using SALES.Application.Common.Events; // Asegúrate de crear este record en Application
using SALES.Application.DTOs;
using SALES.Application.Interfaces;
using SALES.Persistence.Context;

namespace SALES.Persistence.Services
{
    public class SalesService : ISalesService
    {
        private readonly ApplicationDbContext _context;
        private readonly IPublishEndpoint _publishEndpoint;

        public SalesService(
            ApplicationDbContext context,
            IPublishEndpoint publishEndpoint)
        {
            _context = context;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<bool> ProcessSaleAsync(CreateSaleDto saleData)
        {
            string transactionIdGenerado = "";

            // 1. VALIDACIÓN DE PAGO (Síncrona/Inmediata)
            // Se mantiene aquí porque si el pago falla, no debemos ni crear la orden.
            if (saleData.IdPaymentMethod == 1)
            {
                if (!SimulatePaymentGateway(saleData, out transactionIdGenerado))
                    throw new Exception("El pago fue rechazado. Verifique los datos.");
            }
            else
            {
                transactionIdGenerado = $"EXT-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
            }

            // 2. PERSISTENCIA LOCAL CON TRANSACCIÓN
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Crear Cabecera de Orden
                var order = new SaleOrder
                {
                    IdUserAccount = saleData.IdUserAccount,
                    IdSaleState = 1, // Estado inicial: Pendiente/Pagado
                    OrderDate = DateTime.Now,
                    TrackingNumber = GenerateTrackingNumber()
                };
                _context.SaleOrders.Add(order);
                await _context.SaveChangesAsync();

                // Crear Datos de Envío
                var shipping = new Shipping
                {
                    IdSaleOrder = order.IdSaleOrder,
                    ReceiverName = saleData.ReceiverName,
                    ReceiverLastName = saleData.ReceiverLastName,
                    PhoneNumber = saleData.PhoneNumber,
                    Dni = saleData.Dni,
                    City = saleData.City,
                    Region = saleData.Region,
                    Province = saleData.Province,
                    District = saleData.District,
                    PostalCode = saleData.PostalCode,
                    AddressReference = saleData.AddressReference,
                    IdShoppingProvider = saleData.IdShoppingProvider
                };
                _context.Shippings.Add(shipping);

                // Crear Registro de Pago
                var payment = new SalePayment
                {
                    IdSaleOrder = order.IdSaleOrder,
                    IdPaymentMethod = saleData.IdPaymentMethod,
                    IdCart = saleData.IdCart,
                    SubTotal = saleData.SubTotal,
                    ShippingCost = saleData.ShippingCost,
                    TotalAmount = saleData.TotalAmount,
                    TransactionId = transactionIdGenerado,
                    PaymentDate = DateTime.Now
                };
                _context.SalePayments.Add(payment);

                await _context.SaveChangesAsync();

                // Si todo en nuestra DB local está bien, confirmamos
                await transaction.CommitAsync();

                // =========================================================
                // 3. INICIO DE SAGA (EVENT-DRIVEN)
                // =========================================================
                // Publicamos el evento de integración. MassTransit se encarga
                // de enviarlo a RabbitMQ. Los servicios de CARRITO y PRODUCTOS
                // reaccionarán de forma asíncrona.

                await _publishEndpoint.Publish(new SaleCreatedEvent
                {
                    IdSale = order.IdSaleOrder,
                    IdCart = saleData.IdCart,
                    Items = saleData.SaleItems ?? new List<SaleItemDto>()
                });

                return true;
            }
            catch (Exception)
            {
                // Si algo falla en la persistencia local, revertimos
                await transaction.RollbackAsync();
                throw;
            }
        }

        // --- MÉTODOS AUXILIARES ---

        private bool SimulatePaymentGateway(CreateSaleDto cardData, out string transactionId)
        {
            transactionId = $"TXN-{Guid.NewGuid().ToString().ToUpper()}";
            string cleanCardNumber = cardData.CardNumber?.Replace(" ", "") ?? "";

            if (string.IsNullOrEmpty(cleanCardNumber) || cleanCardNumber.Length < 13) return false;
            if (cardData.TotalAmount > 5000) return false;

            return true;
        }

        private string GenerateTrackingNumber()
        {
            return $"TRK-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }
    }
}