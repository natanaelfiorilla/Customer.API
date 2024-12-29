using Amazon.DynamoDBv2.DataModel;
using Taxdown.Domain.Entities;
using Taxdown.DomainServices;
using Taxdown.Infraestructure.Mappers;
using Taxdown.Infraestructure.Models;

namespace Taxdown.Infraestructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly IDynamoDBContext _context;

    public CustomerRepository(IDynamoDBContext context)
    {
        _context = context;
    }

    public async Task<List<Customer>> GetAllAsync()
    {
        var scanConditions = new List<ScanCondition>();
        var scan = _context.ScanAsync<CustomerDynamoDb>(scanConditions);
        var dynamoRecords = await scan.GetRemainingAsync();

        // Convert from DB model to domain model
        return dynamoRecords
            .Select(d => d.ToDomainModel())
            .ToList();
    }

    public async Task<Customer?> GetByIdAsync(string id)
    {
        var record = await _context.LoadAsync<CustomerDynamoDb>(id);
        return record?.ToDomainModel();
    }

    public async Task SaveAsync(Customer customer)
    {
        // Convert from domain model to DB model
        var dbModel = customer.ToDbModel();
        await _context.SaveAsync(dbModel);
    }

    public async Task DeleteAsync(string id)
    {
        await _context.DeleteAsync<CustomerDynamoDb>(id);
    }
}