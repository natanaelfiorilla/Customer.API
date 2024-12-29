namespace Taxdown.API.Models;

public class CustomerResponse
{
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public decimal AvailableCredit { get; set; }
}