using Taxdown.Domain.Entities;
using Taxdown.DomainServices;

namespace Taxdown.ApplicationServices.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<List<Customer>> GetAllAsync()
    {
        return await _customerRepository.GetAllAsync();
    }

    public async Task<Customer?> GetByIdAsync(string id)
    {
        return await _customerRepository.GetByIdAsync(id);
    }

    public async Task<Customer> CreateAsync(Customer customer)
    {
        // Generate ID if needed
        if (string.IsNullOrEmpty(customer.Id))
            customer.Id = Guid.NewGuid().ToString();

        await _customerRepository.SaveAsync(customer);
        return customer;
    }

    public async Task UpdateAsync(Customer customer)
    {
        var existing = await GetByIdAsync(customer.Id);
        if (existing == null)
            throw new ArgumentException($"Customer with Id = {customer.Id} not found.");

        await _customerRepository.SaveAsync(customer);
    }

    public async Task DeleteAsync(string id)
    {
        await _customerRepository.DeleteAsync(id);
    }

    public async Task AddCreditAsync(string id, decimal creditToAdd)
    {
        var customer = await GetByIdAsync(id);
        if (customer == null)
            throw new ArgumentException($"Customer with Id = {id} not found.");

        customer.AvailableCredit += creditToAdd;
        await _customerRepository.SaveAsync(customer);
    }

    public async Task<List<Customer>> GetAllSortedByCreditAsync()
    {
        var customers = await _customerRepository.GetAllAsync();
        return customers
            .OrderByDescending(c => c.AvailableCredit)
            .ToList();
    }
}