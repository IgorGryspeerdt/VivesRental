using System;

namespace VivesRental.Api.Settings
{
    public class JwtSettings
    {
        public required string Secret { get; set; }
        public TimeSpan ExpirationPeriod { get; set; } = TimeSpan.FromMinutes(60);
        public string Issuer { get; set; } = "VivesRental";
        public string Audience { get; set; } = "VivesRental";
    }
}