using Taxdown.Domain.Entities;
using Taxdown.Infraestructure.Models;

namespace Taxdown.Infraestructure.Mappers;

public static class CustomerDomainToDbMapper
{
    public static CustomerDynamoDb ToDbModel(this Customer domainCustomer)
    {
        return new CustomerDynamoDb
        {
            Id = domainCustomer.Id,
            Name = domainCustomer.Name,
            Email = domainCustomer.Email,
            AvailableCredit = domainCustomer.AvailableCredit
        };
    }

    public static Customer ToDomainModel(this CustomerDynamoDb dbModel)
    {
        return new Customer
        {
            Id = dbModel.Id,
            Name = dbModel.Name,
            Email = dbModel.Email,
            AvailableCredit = dbModel.AvailableCredit
        };
    }
}