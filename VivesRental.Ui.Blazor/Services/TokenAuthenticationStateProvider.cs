using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using VivesRental.Ui.Blazor.Security;

namespace VivesRental.Ui.Blazor.Services
{
    public class TokenAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ITokenStore _tokenStore;

        public TokenAuthenticationStateProvider(ITokenStore tokenStore)
        {
            _tokenStore = tokenStore;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _tokenStore.GetToken();
            var identity = new ClaimsIdentity();

            if (!string.IsNullOrWhiteSpace(token))
            {
                identity = CreateIdentityFromToken(token);
            }

            var user = new ClaimsPrincipal(identity);
            var state = new AuthenticationState(user);
            NotifyAuthenticationStateChanged(Task.FromResult(state));
            return state;
        }

        private ClaimsIdentity CreateIdentityFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwt;
            try
            {
                jwt = handler.ReadJwtToken(token);
            }
            catch
            {
                return new ClaimsIdentity();
            }

            var claims = jwt.Claims.ToList();

            // Ensure we have a Name claim; prefer the "email" claim from the token
            if (!claims.Any(c => c.Type == ClaimTypes.Name || c.Type == "email" || c.Type == "sub"))
            {
                var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email || c.Type == "email")?.Value;
                if (!string.IsNullOrWhiteSpace(email))
                    claims.Add(new Claim("email", email));
            }

            // Create identity mapping the token's claim names:
            // - NameClaimType: "email" (token contains "email")
            // - RoleClaimType: "role" (token contains "role")
            var identity = new ClaimsIdentity(claims, "Bearer", "email", "role");
            return identity;
        }

        public void NotifyUserAuthentication()
        {
            var authState = GetAuthenticationStateAsync();
            NotifyAuthenticationStateChanged(authState);
        }
    }
}