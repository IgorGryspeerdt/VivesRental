using System.Net.Http.Headers;
using VivesRental.Ui.Blazor.Security;

namespace VivesRental.Ui.Blazor.Sdk.DelegatingHandlers
{
    public class AuthorizationHandler : DelegatingHandler
    {
        private readonly ITokenStore _tokenStore;

        public AuthorizationHandler(ITokenStore tokenStore)
        {
            _tokenStore = tokenStore;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await _tokenStore.GetToken();
            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}