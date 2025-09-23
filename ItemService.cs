using Amazon.DynamoDBv2.DataModel;

public class ItemService : IItemService
{
    private readonly IDynamoDBContext _dynamoDbContext;

    public ItemService(IDynamoDBContext dynamoDbContext)
    {
        _dynamoDbContext = dynamoDbContext;
    }

    public async Task<IEnumerable<Item>> GetAllItemsAsync()
    {
        var scanConditions = new List<ScanCondition>();
        var search = _dynamoDbContext.ScanAsync<Item>(scanConditions);
        return await search.GetRemainingAsync();
    }

    public async Task<Item?> GetItemAsync(string id)
    {
        return await _dynamoDbContext.LoadAsync<Item>(id);
    }

    public async Task<Item> CreateItemAsync(ItemRequest request)
    {
        var item = new Item
        {
            Id = Guid.NewGuid().ToString(),
            Name = request.Name,
            Description = request.Description,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _dynamoDbContext.SaveAsync(item);
        return item;
    }

    public async Task<Item?> UpdateItemAsync(string id, ItemRequest request)
    {
        var existingItem = await _dynamoDbContext.LoadAsync<Item>(id);
        if (existingItem == null)
            return null;

        existingItem.Name = request.Name;
        existingItem.Description = request.Description;
        existingItem.UpdatedAt = DateTime.UtcNow;

        await _dynamoDbContext.SaveAsync(existingItem);
        return existingItem;
    }

    public async Task<bool> DeleteItemAsync(string id)
    {
        var existingItem = await _dynamoDbContext.LoadAsync<Item>(id);
        if (existingItem == null)
            return false;

        await _dynamoDbContext.DeleteAsync<Item>(id);
        return true;
    }
}