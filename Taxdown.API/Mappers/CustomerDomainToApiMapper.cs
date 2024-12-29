using Taxdown.API.Models;
using Taxdown.Domain.Entities;

namespace Taxdown.API.Mappers;

public static class CustomerDomainToApiMapper
{
    public static Customer ToDomainModel(this CustomerRequest request, string? existingId = null)
    {
        return new Customer
        {
            Id = existingId ?? string.Empty,
            Name = request.Name ?? string.Empty,
            Email = request.Email ?? string.Empty,
            AvailableCredit = request.AvailableCredit
        };
    }

    public static CustomerResponse ToResponseModel(this Customer domainCustomer)
    {
        return new CustomerResponse
        {
            Id = domainCustomer.Id,
            Name = domainCustomer.Name,
            Email = domainCustomer.Email,
            AvailableCredit = domainCustomer.AvailableCredit
        };
    }
}