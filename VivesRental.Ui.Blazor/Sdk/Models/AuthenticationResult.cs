namespace VivesRental.Ui.Blazor.Sdk.Models
{
    public class AuthenticationResult
    {
        public string? Token { get; set; }
        public DateTime? Expires { get; set; }
        public List<ServiceMessage>? Messages { get; set; }

        public class ServiceMessage
        {
            public string? Code { get; set; }
            public string? Description { get; set; }
            public string? Type { get; set; }
        }
    }
}