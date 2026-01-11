using System.Net;
using System.Net.Http.Json;
using VivesRental.Services.Model.Results;

namespace VivesRental.Ui.Blazor.Sdk
{
    public class OrderLineSdkService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public OrderLineSdkService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private HttpClient CreateClient() => _httpClientFactory.CreateClient("VivesRentalApi");

        public async Task<List<OrderLineResult>> GetAll(Guid orderId)
        {
            var client = CreateClient();
            var response = await client.GetAsync($"/api/OrderLine?OrderId={orderId}");
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadFromJsonAsync<List<OrderLineResult>>();
            return data ?? new List<OrderLineResult>();
        }

        public async Task<bool> Rent(Guid orderId, Guid articleId)
        {
            var client = CreateClient();
            var response = await client.PostAsync($"/api/OrderLine/rent?orderId={orderId}&articleId={articleId}", null);
            if (response.StatusCode == HttpStatusCode.NoContent) return true;
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RentMany(Guid orderId, IEnumerable<Guid> articleIds)
        {
            var client = CreateClient();
            var response = await client.PostAsJsonAsync($"/api/OrderLine/rentMany?orderId={orderId}", articleIds);
            if (response.StatusCode == HttpStatusCode.NoContent) return true;
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Return(Guid orderLineId, DateTime returnedAt)
        {
            var client = CreateClient();
            var response = await client.PatchAsync($"/api/OrderLine/{orderLineId}/return", JsonContent.Create(returnedAt));
            return response.IsSuccessStatusCode;
        }
    }
}
