//using SALES.Application.Interfaces;
//using System.Net.Http;
//using System.Net.Http.Json;
//using System.Threading.Tasks;

//namespace SALES.Persistence.Services
//{
//    public class CartIntegrationService : ICartIntegrationService
//    {
//        private readonly HttpClient _httpClient;

//        public CartIntegrationService(HttpClient httpClient)
//        {
//            _httpClient = httpClient;
//        }

//        public async Task<bool> CloseCartAsync(int idCart)
//        {
//            try
//            {
//                var body = new { IdCart = idCart, IdState = 2 };
//                var response = await _httpClient.PutAsJsonAsync("/Cart/update-state", body);
//                return response.IsSuccessStatusCode;
//            }
//            catch
//            {
//                return false;
//            }
//        }
//    }
//}