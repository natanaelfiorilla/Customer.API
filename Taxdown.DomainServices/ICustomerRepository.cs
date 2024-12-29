using Taxdown.Domain.Entities;

namespace Taxdown.DomainServices;

public interface ICustomerRepository
{
    Task<List<Customer>> GetAllAsync();
    Task<Customer?> GetByIdAsync(string id);
    Task SaveAsync(Customer customer);
    Task DeleteAsync(string id);
}