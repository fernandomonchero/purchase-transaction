using Microsoft.EntityFrameworkCore;
using PurchaseTransaction.Api.Startup;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var urlTreasuryApi = builder.Configuration.GetSection("UrlTreasuryApi").Value;
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.SolveDependencies(urlTreasuryApi)
    .ConfigureRoutesAndSwagger()
    .ConfigureEntityFramework(connectionString);

var app = builder.Build();

await app.UseEntityFramework();

// Configure the HTTP request pipeline.

app.UseSwagger();

app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();