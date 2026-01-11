using System.Net;
using System.Net.Http.Json;
using VivesRental.Services.Model.Results;
using VivesRental.Services.Model.Requests;

namespace VivesRental.Ui.Blazor.Sdk
{
    public class CustomerSdkService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CustomerSdkService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private HttpClient CreateClient() => _httpClientFactory.CreateClient("VivesRentalApi");

        public async Task<List<CustomerResult>> GetAll()
        {
            var client = CreateClient();
            var result = await client.GetFromJsonAsync<List<CustomerResult>>("/api/Customer");
            return result ?? new List<CustomerResult>();
        }

        public async Task<CustomerResult?> Get(Guid id)
        {
            var client = CreateClient();
            var response = await client.GetAsync($"/api/Customer/{id}");
            if (response.StatusCode == HttpStatusCode.NotFound) return null;
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<CustomerResult>();
        }

        public async Task<CustomerResult?> Create(CustomerRequest request)
        {
            var client = CreateClient();
            var response = await client.PostAsJsonAsync("/api/Customer", request);
            if (!response.IsSuccessStatusCode) return null;
            return await response.Content.ReadFromJsonAsync<CustomerResult>();
        }

        public async Task<CustomerResult?> Update(Guid id, CustomerRequest request)
        {
            var client = CreateClient();
            var response = await client.PutAsJsonAsync($"/api/Customer/{id}", request);
            if (!response.IsSuccessStatusCode) return null;
            return await response.Content.ReadFromJsonAsync<CustomerResult>();
        }

        public async Task<bool> Delete(Guid id)
        {
            var client = CreateClient();
            var response = await client.DeleteAsync($"/api/Customer/{id}");
            if (response.StatusCode == HttpStatusCode.NoContent) return true;
            return response.IsSuccessStatusCode;
        }
    }
}
