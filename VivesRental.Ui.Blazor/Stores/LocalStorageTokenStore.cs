using Blazored.LocalStorage;
using VivesRental.Ui.Blazor.Security;

namespace VivesRental.Ui.Blazor.Stores
{
    public class LocalStorageTokenStore : ITokenStore
    {
        private const string TokenKey = "authToken";
        private readonly ILocalStorageService _localStorage;

        public LocalStorageTokenStore(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public async ValueTask<string?> GetToken()
        {
            return await _localStorage.GetItemAsync<string>(TokenKey);
        }

        public async ValueTask SetToken(string token)
        {
            await _localStorage.SetItemAsync(TokenKey, token);
        }

        public async ValueTask Clear()
        {
            await _localStorage.RemoveItemAsync(TokenKey);
        }
    }
}