using System.ComponentModel.DataAnnotations;

namespace VivesRental.Ui.Blazor.Sdk.Models
{
    public class SignInRequest
    {
        [Required]
        public required string Username { get; set; }

        [Required]
        public required string Password { get; set; }
    }
}