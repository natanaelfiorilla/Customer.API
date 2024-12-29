using Taxdown.Domain.Entities;

namespace Taxdown.ApplicationServices;

public interface ICustomerService
{
    Task<List<Customer>> GetAllAsync();
    Task<Customer?> GetByIdAsync(string id);
    Task<Customer> CreateAsync(Customer customer);
    Task UpdateAsync(Customer customer);
    Task DeleteAsync(string id);
    Task AddCreditAsync(string id, decimal creditToAdd);
    Task<List<Customer>> GetAllSortedByCreditAsync();
}