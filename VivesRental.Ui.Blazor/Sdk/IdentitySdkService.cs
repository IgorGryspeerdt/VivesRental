using System.Net.Http.Json;
using VivesRental.Ui.Blazor.Sdk.Models;

namespace VivesRental.Ui.Blazor.Sdk
{
    public class IdentitySdkService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public IdentitySdkService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<AuthenticationResult> SignIn(SignInRequest request)
        {
            var client = _httpClientFactory.CreateClient("VivesRentalApi");
            var response = await client.PostAsJsonAsync("/api/Auth/login", request);

            // Return deserialised body even if status code 200 but wrapped
            if (!response.IsSuccessStatusCode)
            {
                return new AuthenticationResult
                {
                    Messages = new List<AuthenticationResult.ServiceMessage>
                    {
                        new() { Code = response.StatusCode.ToString(), Description = "Login failed", Type = "Error" }
                    }
                };
            }

            var result = await response.Content.ReadFromJsonAsync<AuthenticationResult>();
            return result ?? new AuthenticationResult();
        }
    }
}