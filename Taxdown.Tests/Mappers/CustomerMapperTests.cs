using Taxdown.Domain.Entities;
using Taxdown.Infraestructure.Mappers;
using Taxdown.Infraestructure.Models;

namespace Taxdown.Tests.Mappers;

public class CustomerMapperTests
{
    [Fact]
    public void ToDbModel_MapsAllProperties()
    {
        // Arrange
        var domainCustomer = new Customer
        {
            Id = "123",
            Name = "John Doe",
            Email = "john@example.com",
            AvailableCredit = 100.5m
        };

        // Act
        var dbModel = domainCustomer.ToDbModel();

        // Assert
        Assert.Equal(domainCustomer.Id, dbModel.Id);
        Assert.Equal(domainCustomer.Name, dbModel.Name);
        Assert.Equal(domainCustomer.Email, dbModel.Email);
        Assert.Equal(domainCustomer.AvailableCredit, dbModel.AvailableCredit);
    }

    [Fact]
    public void ToDomainModel_MapsAllProperties()
    {
        // Arrange
        var dbModel = new CustomerDynamoDb
        {
            Id = "abc",
            Name = "Jane Smith",
            Email = "jane@example.com",
            AvailableCredit = 999.99m
        };

        // Act
        var domainCustomer = dbModel.ToDomainModel();

        // Assert
        Assert.Equal(dbModel.Id, domainCustomer.Id);
        Assert.Equal(dbModel.Name, domainCustomer.Name);
        Assert.Equal(dbModel.Email, domainCustomer.Email);
        Assert.Equal(dbModel.AvailableCredit, domainCustomer.AvailableCredit);
    }
}