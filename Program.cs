using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Lambda.Serialization.SystemTextJson;

var builder = WebApplication.CreateBuilder(args);

// Add AWS Lambda support with proper JSON serialization
builder.Services.AddAWSLambdaHosting(LambdaEventSource.RestApi);

// Add DynamoDB
builder.Services.AddAWSService<IAmazonDynamoDB>();
builder.Services.AddScoped<IDynamoDBContext, DynamoDBContext>();

// Add services
builder.Services.AddScoped<IItemService, ItemService>();

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseRouting();

// API Routes for CRUD operations
app.MapGet("/items", async (IItemService itemService) =>
{
    var items = await itemService.GetAllItemsAsync();
    return Results.Ok(items);
});

// REST API endpoints for CRUD operations
app.MapGet("/items/{id}", async (string id, IItemService itemService) =>
{
    var item = await itemService.GetItemAsync(id);
    return item != null ? Results.Ok(item) : Results.NotFound();
});

app.MapPost("/items", async (ItemRequest request, IItemService itemService) =>
{
    var item = await itemService.CreateItemAsync(request);
    return Results.Created($"/items/{item.Id}", item);
});

app.MapPut("/items/{id}", async (string id, ItemRequest request, IItemService itemService) =>
{
    var item = await itemService.UpdateItemAsync(id, request);
    return item != null ? Results.Ok(item) : Results.NotFound();
});

app.MapDelete("/items/{id}", async (string id, IItemService itemService) =>
{
    var success = await itemService.DeleteItemAsync(id);
    return success ? Results.NoContent() : Results.NotFound();
});

// Start the application
app.Run();