using Employee.API.Data; // <-- 1. Import our DataContext
using Microsoft.EntityFrameworkCore; // <-- 2. Import Entity Framework
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

// --- Add this section ---
// This is where we register our services

// 3. Get the connection string from appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 4. Register the DataContext as a service
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

// --- End of new section ---


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/api/employees", async (DataContext context) =>
{
    var employees = await context.Employees.ToListAsync();
    return Results.Ok(employees);
});

app.MapPost("/api/employees", async (DataContext context, Employee.API.Models.Employee employee) =>
{
    context.Employees.Add(employee);
    await context.SaveChangesAsync();

    return Results.Created($"/api/employees/{employee.Id}", employee);
});

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}