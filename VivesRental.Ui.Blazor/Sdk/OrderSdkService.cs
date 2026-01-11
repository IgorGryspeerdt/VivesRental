using System.Net;
using System.Net.Http.Json;
using VivesRental.Services.Model.Results;

namespace VivesRental.Ui.Blazor.Sdk
{
    public class OrderSdkService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public OrderSdkService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private HttpClient CreateClient() => _httpClientFactory.CreateClient("VivesRentalApi");

        public async Task<List<OrderResult>> GetAll()
        {
            var client = CreateClient();
            var result = await client.GetFromJsonAsync<List<OrderResult>>("/api/Order");
            return result ?? new List<OrderResult>();
        }

        public async Task<OrderResult?> Get(Guid id)
        {
            var client = CreateClient();
            var response = await client.GetAsync($"/api/Order/{id}");
            if (response.StatusCode == HttpStatusCode.NotFound) return null;
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<OrderResult>();
        }

        public async Task<OrderResult?> Create(Guid customerId)
        {
            var client = CreateClient();
            var response = await client.PostAsync($"/api/Order?customerId={customerId}", null);
            if (!response.IsSuccessStatusCode) return null;
            return await response.Content.ReadFromJsonAsync<OrderResult>();
        }
    }
}
