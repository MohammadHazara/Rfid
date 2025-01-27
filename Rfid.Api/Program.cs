using Microsoft.Azure.Cosmos;
using Rfid.Core.Interfaces.Repositories;
using Rfid.Core.Interfaces.Services;
using Rfid.Data.Infrastructure;
using Rfid.Data.Repositories;
using Rfid.Data.Services;
#nullable disable

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// add cosmos client
builder.Services.AddScoped<CosmosContainerConfig>(o =>
{
    var config = o.GetRequiredService<IConfiguration>();
    return config.GetRequiredSection("CosmosClient")?.Get<CosmosContainerConfig>();
});

builder.Services.AddSingleton<CosmosClient>(sp =>
{
    var connectionString = builder.Configuration.GetConnectionString("CosmosDb");
    return new CosmosClient(connectionString);
});


// add repositories
builder.Services.AddScoped<IRfidRepository, RfidRepository>();
builder.Services.AddScoped<IRfidService, RfidService>();
builder.Services.AddScoped<ILogService, LogService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
