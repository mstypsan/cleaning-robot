using System.Text.Json.Serialization;
using CleaningApi.Services;
using Repositories;

var builder = WebApplication.CreateBuilder(args);

var connectionString = Environment.GetEnvironmentVariable("ENV_CONNECTION_STRING");
if(string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("ENV_CONNECTION_STRING should not be empty");
}

builder.Services.ConfigureDatabase(connectionString);
builder.Services.AddScoped<CleaningService>();

builder.Services.AddControllers().AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// The database creation should be moved to a separate project and perhaps include migrations
using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<ExecutionsContext>();
dbContext.Database.EnsureCreated();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
