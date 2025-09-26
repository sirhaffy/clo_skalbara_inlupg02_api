using Amazon.DynamoDBv2.DataModel;

[DynamoDBTable("clofresva-skalbara-upg02-items")]  // Use the actual table name directly
public class Item
{
    [DynamoDBHashKey]
    public string Id { get; set; } = string.Empty;

    [DynamoDBProperty]
    public string Name { get; set; } = string.Empty;

    [DynamoDBProperty]
    public string Description { get; set; } = string.Empty;

    [DynamoDBProperty]
    public DateTime CreatedAt { get; set; }

    [DynamoDBProperty]
    public DateTime UpdatedAt { get; set; }
}

public record ItemRequest(string Name, string Description);

public interface IItemService
{
    Task<IEnumerable<Item>> GetAllItemsAsync();
    Task<Item?> GetItemAsync(string id);
    Task<Item> CreateItemAsync(ItemRequest request);
    Task<Item?> UpdateItemAsync(string id, ItemRequest request);
    Task<bool> DeleteItemAsync(string id);
}