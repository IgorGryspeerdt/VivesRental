using System.Net;
using System.Net.Http.Json;
using VivesRental.Services.Model.Requests;
using VivesRental.Services.Model.Results;
using VivesRental.Ui.Blazor.Sdk.Models;

namespace VivesRental.Ui.Blazor.Sdk
{
    public class ProductSdkService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductSdkService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private HttpClient CreateClient() => _httpClientFactory.CreateClient("VivesRentalApi");

        public async Task<List<ProductResult>> GetAll()
        {
            var client = CreateClient();
            var result = await client.GetFromJsonAsync<List<ProductResult>>("/api/Product");
            return result ?? new List<ProductResult>();
        }

        public async Task<ProductResult?> Get(Guid id)
        {
            var client = CreateClient();
            var response = await client.GetAsync($"/api/Product/{id}");
            if (response.StatusCode == HttpStatusCode.NotFound) return null;
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ProductResult>();
        }

        public async Task<ProductResult?> Create(ProductRequest request)
        {
            var client = CreateClient();
            var response = await client.PostAsJsonAsync("/api/Product", request);
            if (!response.IsSuccessStatusCode) return null;
            return await response.Content.ReadFromJsonAsync<ProductResult>();
        }

        public async Task<ProductResult?> Update(Guid id, ProductRequest request)
        {
            var client = CreateClient();
            var response = await client.PutAsJsonAsync($"/api/Product/{id}", request);
            if (!response.IsSuccessStatusCode) return null;
            return await response.Content.ReadFromJsonAsync<ProductResult>();
        }

        public async Task<bool> Delete(Guid id)
        {
            var client = CreateClient();
            var response = await client.DeleteAsync($"/api/Product/{id}");
            if (response.StatusCode == HttpStatusCode.NoContent) return true;
            return response.IsSuccessStatusCode;
        }
    }
}
