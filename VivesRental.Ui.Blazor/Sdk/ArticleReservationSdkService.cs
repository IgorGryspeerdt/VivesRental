using System.Net;
using System.Net.Http.Json;
using VivesRental.Services.Model.Results;
using VivesRental.Services.Model.Requests;

namespace VivesRental.Ui.Blazor.Sdk
{
    public class ArticleReservationSdkService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ArticleReservationSdkService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private HttpClient CreateClient() => _httpClientFactory.CreateClient("VivesRentalApi");

        public async Task<List<ArticleReservationResult>> GetAll()
        {
            var client = CreateClient();
            var result = await client.GetFromJsonAsync<List<ArticleReservationResult>>("/api/ArticleReservation");
            return result ?? new List<ArticleReservationResult>();
        }

        public async Task<ArticleReservationResult?> Get(Guid id)
        {
            var client = CreateClient();
            var response = await client.GetAsync($"/api/ArticleReservation/{id}");
            if (response.StatusCode == HttpStatusCode.NotFound) return null;
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ArticleReservationResult>();
        }

        public async Task<ArticleReservationResult?> Create(ArticleReservationRequest request)
        {
            var client = CreateClient();
            var response = await client.PostAsJsonAsync("/api/ArticleReservation", request);
            if (!response.IsSuccessStatusCode) return null;
            return await response.Content.ReadFromJsonAsync<ArticleReservationResult>();
        }

        public async Task<bool> Delete(Guid id)
        {
            var client = CreateClient();
            var response = await client.DeleteAsync($"/api/ArticleReservation/{id}");
            if (response.StatusCode == HttpStatusCode.NoContent) return true;
            return response.IsSuccessStatusCode;
        }
    }
}
