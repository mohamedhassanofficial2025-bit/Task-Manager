using TaskManager.API.Middlewares;
using TaskManager.Core;
using TaskManager.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Add Core services to the container
builder.Services.AddCore();
// Add infrastructure services to the container
builder.Services.AddInfrastructure(builder.Configuration);


// Add controllers to the container
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Add Exception Handling Middleware
    app.UseExcptionHandlingMiddleware();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
