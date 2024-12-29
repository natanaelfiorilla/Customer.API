using Taxdown.API.Mappers;
using Taxdown.API.Models;
using Taxdown.Domain.Entities;

namespace Taxdown.Tests.Mappers;

public class CustomerDomainToApiMapperTests
{
    [Fact]
    public void CustomerRequest_ToDomainModel_MapsProperly()
    {
        // Arrange
        var request = new CustomerRequest
        {
            Name = "Test Name",
            Email = "test@example.com",
            AvailableCredit = 500m
        };

        // Act
        var domainCustomer = request.ToDomainModel(existingId: "some-id");

        // Assert
        Assert.Equal("some-id", domainCustomer.Id);
        Assert.Equal("Test Name", domainCustomer.Name);
        Assert.Equal("test@example.com", domainCustomer.Email);
        Assert.Equal(500m, domainCustomer.AvailableCredit);
    }

    [Fact]
    public void Customer_ToResponseModel_MapsProperly()
    {
        // Arrange
        var domainCustomer = new Customer
        {
            Id = "123",
            Name = "Response Name",
            Email = "response@example.com",
            AvailableCredit = 123.45m
        };

        // Act
        var response = domainCustomer.ToResponseModel();

        // Assert
        Assert.Equal("123", response.Id);
        Assert.Equal("Response Name", response.Name);
        Assert.Equal("response@example.com", response.Email);
        Assert.Equal(123.45m, response.AvailableCredit);
    }
}