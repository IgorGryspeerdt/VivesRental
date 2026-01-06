namespace VivesRental.Services.Model.Requests;

public class ProductPatchRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Manufacturer { get; set; }
    public string? Publisher { get; set; }
    public int? RentalExpiresAfterDays { get; set; }
}