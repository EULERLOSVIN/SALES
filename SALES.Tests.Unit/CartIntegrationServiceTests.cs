using FluentAssertions;
using Moq;
using Moq.Protected;
using SALES.Persistence.Services;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace SALES.Tests.Unit
{
    public class CartIntegrationServiceTests
    {
        [Fact]
        public async Task CloseCartAsync_WhenApiReturnsOk_ShouldReturnTrue()
        {
            // 1. ARRANGE: Configuramos el Mock del HttpMessageHandler
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            // Simulamos una respuesta exitosa (200 OK)
            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               .ReturnsAsync(new HttpResponseMessage
               {
                   StatusCode = HttpStatusCode.OK
               });

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://localhost/")
            };

            var service = new CartIntegrationService(httpClient);

            // 2. ACT
            var result = await service.CloseCartAsync(123);

            // 3. ASSERT
            result.Should().BeTrue();

            // Verificamos que se llamó al endpoint correcto con el método PUT
            handlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Put &&
                    req.RequestUri!.ToString().Contains("/Cart/update-state")),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        [Fact]
        public async Task CloseCartAsync_WhenApiReturnsError_ShouldReturnFalse()
        {
            // 1. ARRANGE
            var handlerMock = new Mock<HttpMessageHandler>();

            // Simulamos un error del servidor (500 Internal Server Error)
            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               .ReturnsAsync(new HttpResponseMessage
               {
                   StatusCode = HttpStatusCode.InternalServerError
               });

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://localhost/")
            };

            var service = new CartIntegrationService(httpClient);

            // 2. ACT
            var result = await service.CloseCartAsync(123);

            // 3. ASSERT
            result.Should().BeFalse();
        }

        [Fact]
        public async Task CloseCartAsync_WhenExceptionOccurs_ShouldReturnFalse()
        {
            // 1. ARRANGE
            var handlerMock = new Mock<HttpMessageHandler>();

            // Simulamos una excepción de red
            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               .ThrowsAsync(new HttpRequestException());

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://localhost/")
            };

            var service = new CartIntegrationService(httpClient);

            // 2. ACT
            var result = await service.CloseCartAsync(1);

            // 3. ASSERT
            result.Should().BeFalse();
        }
    }
}