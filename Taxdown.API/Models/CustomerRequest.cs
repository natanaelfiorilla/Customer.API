namespace Taxdown.API.Models;

public class CustomerRequest
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public decimal AvailableCredit { get; set; }
}