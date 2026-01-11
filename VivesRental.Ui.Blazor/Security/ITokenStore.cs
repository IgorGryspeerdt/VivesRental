namespace VivesRental.Ui.Blazor.Security
{
    public interface ITokenStore
    {
        ValueTask<string?> GetToken();
        ValueTask SetToken(string token);
        ValueTask Clear();
    }
}