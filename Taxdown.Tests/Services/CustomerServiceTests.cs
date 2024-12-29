using Moq;
using Taxdown.ApplicationServices.Services;
using Taxdown.Domain.Entities;
using Taxdown.DomainServices;

namespace Taxdown.Tests.Services;

public class CustomerServiceTests
    {
        private readonly Mock<ICustomerRepository> _repositoryMock;
        private readonly CustomerService _customerService;

        public CustomerServiceTests()
        {
            _repositoryMock = new Mock<ICustomerRepository>();
            _customerService = new CustomerService(_repositoryMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsListOfCustomers()
        {
            // Arrange
            var fakeCustomers = new List<Customer>
            {
                new Customer { Id = "1", Name = "Alice" },
                new Customer { Id = "2", Name = "Bob" }
            };
            _repositoryMock
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(fakeCustomers);

            // Act
            var result = await _customerService.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, c => c.Name == "Alice");
            Assert.Contains(result, c => c.Name == "Bob");
        }

        [Fact]
        public async Task GetByIdAsync_Found_ReturnsCustomer()
        {
            // Arrange
            var fakeCustomer = new Customer { Id = "123", Name = "Test" };
            _repositoryMock
                .Setup(r => r.GetByIdAsync("123"))
                .ReturnsAsync(fakeCustomer);

            // Act
            var result = await _customerService.GetByIdAsync("123");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("123", result.Id);
            Assert.Equal("Test", result.Name);
        }

        [Fact]
        public async Task CreateAsync_AssignsGuidIfEmptyId_SavesCustomer()
        {
            // Arrange
            var inputCustomer = new Customer
            {
                Id = null, // or ""
                Name = "New Customer"
            };

            // Act
            var created = await _customerService.CreateAsync(inputCustomer);

            // Assert
            Assert.False(string.IsNullOrWhiteSpace(created.Id), "Id should be generated if null/empty");
            _repositoryMock.Verify(r => r.SaveAsync(It.IsAny<Customer>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_CustomerNotFound_ThrowsException()
        {
            // Arrange
            var customerToUpdate = new Customer
            {
                Id = "abc",
                Name = "Update Me"
            };

            // The repo will return null => meaning "not found"
            _repositoryMock
                .Setup(r => r.GetByIdAsync("abc"))
                .ReturnsAsync((Customer)null);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(() => _customerService.UpdateAsync(customerToUpdate));
            Assert.Contains("not found", ex.Message);
        }

        [Fact]
        public async Task AddCreditAsync_CustomerFound_IncreasesCredit()
        {
            // Arrange
            var existingCustomer = new Customer
            {
                Id = "xyz",
                AvailableCredit = 100m
            };
            _repositoryMock
                .Setup(r => r.GetByIdAsync("xyz"))
                .ReturnsAsync(existingCustomer);

            // Act
            await _customerService.AddCreditAsync("xyz", 50m);

            // Assert
            Assert.Equal(150m, existingCustomer.AvailableCredit);
            _repositoryMock.Verify(r => r.SaveAsync(It.Is<Customer>(c => c.Id == "xyz" && c.AvailableCredit == 150m)), Times.Once);
        }

        [Fact]
        public async Task GetAllSortedByCreditAsync_ReturnsDescendingOrder()
        {
            // Arrange
            var fakeCustomers = new List<Customer>
            {
                new Customer { Id = "1", AvailableCredit = 200m },
                new Customer { Id = "2", AvailableCredit = 500m },
                new Customer { Id = "3", AvailableCredit = 300m },
            };
            _repositoryMock
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(fakeCustomers);

            // Act
            var result = await _customerService.GetAllSortedByCreditAsync();

            // Assert
            // Expect order: 500, 300, 200
            Assert.Equal(3, result.Count);
            Assert.Equal("2", result[0].Id);
            Assert.Equal("3", result[1].Id);
            Assert.Equal("1", result[2].Id);
        }
    }