using LedgerAPI.Models;
using LedgerAPI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<LedgerService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Ledger API", Version = "v1" });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ledger API V1");
    });
}

// Version 1 endpoints
app.MapPost("/v1/accounts/{accountId}/transactions", (string accountId, Transaction transaction, LedgerService ledgerService) =>
{
    ledgerService.AddTransaction(accountId, transaction);
    return Results.Created($"/v1/accounts/{accountId}/transactions", transaction);
})
.WithName("AddTransactionV1");

app.MapGet("/v1/accounts/{accountId}/balance", (string accountId, LedgerService ledgerService) =>
{
    var balance = ledgerService.GetBalance(accountId);
    return Results.Ok(balance);
})
.WithName("GetBalanceV1");

app.MapGet("/v1/accounts/{accountId}/transactions", (string accountId, LedgerService ledgerService) =>
{
    var transactions = ledgerService.GetTransactions(accountId);
    return Results.Ok(transactions);
})
.WithName("GetTransactionsV1");

app.Run();
