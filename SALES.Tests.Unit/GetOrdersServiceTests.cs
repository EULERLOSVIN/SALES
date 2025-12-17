using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using SALES.Application.Interfaces;
using SALES.Persistence;
using SALES.Persistence.Services;
using Xunit;

namespace SALES.Tests.Unit
{
    public class GetOrdersServiceTests
    {
        private readonly Mock<IApplicationDbContext> _contextMock;
        private readonly GetOrdersService _service;

        public GetOrdersServiceTests()
        {
            // ARRANGE: Inicialización del Mock para aislamiento total
            _contextMock = new Mock<IApplicationDbContext>();
            _service = new GetOrdersService(_contextMock.Object);
        }

        [Fact]
        public async Task GetOrders_WhenOrdersHaveShippingAndPayment_ShouldReturnMappedDto()
        {
            // ARRANGE
            int userId = 1;

            // Construimos la jerarquía de entidades requerida por el servicio
            var saleOrder = new SaleOrder
            {
                IdSaleOrder = 100,
                IdUserAccount = userId,
                TrackingNumber = "TRK-123",
                OrderDate = DateTime.Now,
                // Navegación de Estado
                IdSaleStateNavigation = new SaleState { StateName = "Entregado" },
                // Navegación de Envío
                Shippings = new List<Shipping>
                {
                    new Shipping
                    {
                        IdShipping = 1,
                        ReceiverName = "Juan",
                        ReceiverLastName = "Perez",
                        IdShoppingProviderNavigation = new ShoppingProvider
                        {
                            IdShoppingProvider = 1,
                            NameProvider = "DHL"
                        }
                    }
                },
                // Navegación de Pago
                SalePayments = new List<SalePayment>
                {
                    new SalePayment { IdSalePayment = 50, TotalAmount = 150.0m }
                }
            };

            var ordersList = new List<SaleOrder> { saleOrder };
            var mockDbSet = ordersList.AsQueryable().BuildMockDbSet();
            _contextMock.Setup(x => x.SaleOrders).Returns(mockDbSet.Object);

            // ACT
            var result = await _service.GetOrders(userId);

            // ASSERT
            result.Should().NotBeEmpty();
            result.Should().HaveCount(1);

            var orderDto = result.First();
            orderDto.IdOrder.Should().Be(100);
            orderDto.State.Should().Be("Entregado");
            orderDto.DetailShipping.Provider.NameProvider.Should().Be("DHL");
            orderDto.SalePayment.Total.Should().Be(150.0m);
        }

        [Fact]
        public async Task GetOrders_WhenOrderIsMissingShippingOrPayment_ShouldFilterItOut()
        {
            // ARRANGE: Orden incompleta (sin pagos)
            int userId = 2;
            var incompleteOrder = new SaleOrder
            {
                IdUserAccount = userId,
                Shippings = new List<Shipping> { new Shipping() },
                SalePayments = new List<SalePayment>() // LISTA VACÍA
            };

            var mockDbSet = new List<SaleOrder> { incompleteOrder }.AsQueryable().BuildMockDbSet();
            _contextMock.Setup(x => x.SaleOrders).Returns(mockDbSet.Object);

            // ACT
            var result = await _service.GetOrders(userId);

            // ASSERT
            // Según tu lógica: if (shippingDetail == null || paymentDetail == null) return null;
            // Y luego .Where(o => o != null)
            result.Should().BeEmpty();
        }
    }
}