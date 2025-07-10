using Serilog;
using Scalar.AspNetCore;
using FluentValidation;
using Bank.Domain.Entities;
using Bank.Application.Repositories;
using Bank.Application.Validators;
using Bank.Logging;
using Bank.Infrastructure.Repositories;
using Bank.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

try
{
    Logger.Instance.Information("Starting server...");

    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, loggerConfiguration) =>
        {
            loggerConfiguration.WriteTo.Console();
            loggerConfiguration.ReadFrom.Configuration(context.Configuration);
        });

    // Add services to the container.
    builder.Services.AddScoped<IValidator<Account>, AccountValidator>();
    builder.Services.AddScoped<IValidator<Operation>, OperationValidator>();

    builder.Services.AddControllers();
    // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
    builder.Services.AddOpenApi();

    builder.Services.AddDbContext<BankContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("BankDatabase")));

    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<IAccountRepository, AccountRepository>();
    builder.Services.AddScoped<IOperationRepository, OperationRepository>();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.MapScalarApiReference();
        app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "Swagger"));
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "An unexpected exception occurred");
}
finally
{
    Log.CloseAndFlush();
}