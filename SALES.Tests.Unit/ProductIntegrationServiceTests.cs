using FluentAssertions;
using Moq;
using Moq.Protected;
using SALES.Application.DTOs;
using SALES.Persistence.Services;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace SALES.Tests.Unit
{
    public class ProductIntegrationServiceTests
    {
        [Fact]
        public async Task UpdateStockAsync_WhenApiReturnsOk_ShouldReturnTrue()
        {
            // 1. ARRANGE: Configuramos el manejador de mensajes para simular la respuesta de la API
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               .ReturnsAsync(new HttpResponseMessage
               {
                   StatusCode = HttpStatusCode.OK // Simulamos éxito (200)
               });

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://microservicio-productos/")
            };

            var service = new ProductIntegrationService(httpClient);
            var items = new List<SaleItemDto>
            {
                new SaleItemDto { IdDetail = 1, Quantity = 2 }
            };

            // 2. ACT
            var result = await service.UpdateStockAsync(items);

            // 3. ASSERT
            result.Should().BeTrue(); //

            // Verificamos que se llamó al endpoint correcto con el método PUT y el cuerpo esperado
            handlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Put &&
                    req.RequestUri!.ToString().Contains("/Product/reduce-stock")),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        [Fact]
        public async Task UpdateStockAsync_WhenApiFails_ShouldReturnFalse()
        {
            // 1. ARRANGE
            var handlerMock = new Mock<HttpMessageHandler>();

            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               .ReturnsAsync(new HttpResponseMessage
               {
                   StatusCode = HttpStatusCode.BadRequest // Simulamos error (400)
               });

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://microservicio-productos/")
            };

            var service = new ProductIntegrationService(httpClient);

            // 2. ACT
            var result = await service.UpdateStockAsync(new List<SaleItemDto>());

            // 3. ASSERT
            result.Should().BeFalse(); //
        }

        [Fact]
        public async Task UpdateStockAsync_WhenExceptionOccurs_ShouldReturnFalse()
        {
            // 1. ARRANGE: Simulamos una falla de red (timeout o servidor apagado)
            var handlerMock = new Mock<HttpMessageHandler>();

            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               .ThrowsAsync(new HttpRequestException()); // Lanza excepción que el catch debe capturar

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://microservicio-productos/")
            };

            var service = new ProductIntegrationService(httpClient);

            // 2. ACT
            var result = await service.UpdateStockAsync(new List<SaleItemDto>());

            // 3. ASSERT
            result.Should().BeFalse(); // El bloque catch en tu código retorna false
        }
    }
}