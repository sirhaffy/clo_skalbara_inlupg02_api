using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Lambda.Serialization.SystemTextJson;

var builder = WebApplication.CreateBuilder(args);

// Add logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add AWS Lambda support with proper JSON serialization
builder.Services.AddAWSLambdaHosting(LambdaEventSource.RestApi);

// Add DynamoDB
builder.Services.AddAWSService<IAmazonDynamoDB>();
builder.Services.AddScoped<IDynamoDBContext, DynamoDBContext>();

// Add services
builder.Services.AddScoped<IItemService, ItemService>();

// Add CORS for web requests
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Add logging to see if app starts
var logger = app.Logger;
logger.LogInformation("Lambda application starting...");

// Configure the HTTP request pipeline
app.UseCors();
app.UseRouting();

// Health check endpoint
app.MapGet("/", () =>
{
    logger.LogInformation("Health check endpoint called");
    return Results.Ok(new { status = "healthy", timestamp = DateTime.UtcNow });
});

// API Routes for CRUD operations
app.MapGet("/items", async (IItemService itemService, ILogger<Program> logger) =>
{
    logger.LogInformation("GET /items called");
    try
    {
        var items = await itemService.GetAllItemsAsync();
        logger.LogInformation($"Retrieved {items?.Count() ?? 0} items");
        return Results.Ok(items);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error in GET /items");
        return Results.Problem("Internal server error");
    }
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