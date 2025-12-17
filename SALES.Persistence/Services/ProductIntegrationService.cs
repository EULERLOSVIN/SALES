//using SALES.Application.DTOs;
//using SALES.Application.Interfaces;
//using System.Collections.Generic;
//using System.Net.Http;
//using System.Net.Http.Json;
//using System.Threading.Tasks;

//namespace SALES.Persistence.Services
//{
//    public class ProductIntegrationService : IProductIntegrationService
//    {
//        private readonly HttpClient _httpClient;

//        public ProductIntegrationService(HttpClient httpClient)
//        {
//            _httpClient = httpClient;
//        }

//        public async Task<bool> UpdateStockAsync(List<SaleItemDto> items)
//        {
//            try
//            {
//                var response = await _httpClient.PutAsJsonAsync("/Product/reduce-stock", items);
//                return response.IsSuccessStatusCode;
//            }
//            catch
//            {
//                return false;
//            }
//        }
//    }
//}