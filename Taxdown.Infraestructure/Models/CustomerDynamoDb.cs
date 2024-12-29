using Amazon.DynamoDBv2.DataModel;

namespace Taxdown.Infraestructure.Models;

[DynamoDBTable("Customers")]
public class CustomerDynamoDb
{
    [DynamoDBHashKey]
    public string Id { get; set; } = default!;

    [DynamoDBProperty]
    public string Name { get; set; } = default!;

    [DynamoDBProperty]
    public string Email { get; set; } = default!;

    [DynamoDBProperty]
    public decimal AvailableCredit { get; set; }
}