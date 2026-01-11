using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using VivesRental.Services.Model.Results;
using VivesRental.Services.Model.Requests;
using VivesRental.Enums;
using VivesRental.Services.Model.Filters;

namespace VivesRental.Ui.Blazor.Sdk
{
    public class ArticleSdkService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ArticleSdkService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private HttpClient CreateClient() => _httpClientFactory.CreateClient("VivesRentalApi");

        public async Task<List<ArticleResult>> GetAll()
        {
            var client = CreateClient();
            var result = await client.GetFromJsonAsync<List<ArticleResult>>("/api/Article");
            return result ?? new List<ArticleResult>();
        }

        public async Task<ArticleResult?> Get(Guid id)
        {
            var client = CreateClient();
            var response = await client.GetAsync($"/api/Article/{id}");
            if (response.StatusCode == HttpStatusCode.NotFound) return null;
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ArticleResult>();
        }

        public async Task<ArticleResult?> Create(ArticleRequest request)
        {
            var client = CreateClient();
            var response = await client.PostAsJsonAsync("/api/Article", request);
            if (!response.IsSuccessStatusCode) return null;
            return await response.Content.ReadFromJsonAsync<ArticleResult>();
        }

        public async Task<bool> UpdateStatus(Guid id, ArticleStatus status)
        {
            var client = CreateClient();
            // PATCH {id}/status expects a body with ArticleStatus (enum) - send as JSON
            var json = JsonSerializer.Serialize(status);
            var message = new HttpRequestMessage(new HttpMethod("PATCH"), $"/api/Article/{id}/status")
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            var response = await client.SendAsync(message);
            if (response.StatusCode == HttpStatusCode.NoContent) return true;
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Delete(Guid id)
        {
            var client = CreateClient();
            var response = await client.DeleteAsync($"/api/Article/{id}");
            if (response.StatusCode == HttpStatusCode.NoContent) return true;
            return response.IsSuccessStatusCode;
        }

        // New dedicated method
        public async Task<List<ArticleResult>> GetAvailable(Guid productId, DateTime? from = null, DateTime? until = null)
        {
            var client = CreateClient();
            var qs = new List<string>
            {
                $"productId={Uri.EscapeDataString(productId.ToString())}"
            };

            if (from.HasValue)
                qs.Add($"availableFromDateTime={Uri.EscapeDataString(from.Value.ToString("o"))}");
            if (until.HasValue)
                qs.Add($"availableUntilDateTime={Uri.EscapeDataString(until.Value.ToString("o"))}");

            var uri = "/api/Article/available";
            if (qs.Count > 0)
                uri += "?" + string.Join("&", qs);

            var result = await client.GetFromJsonAsync<List<ArticleResult>>(uri);
            return result ?? new List<ArticleResult>();
        }
    }
}
