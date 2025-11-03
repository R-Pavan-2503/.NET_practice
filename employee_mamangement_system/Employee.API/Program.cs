using Employee.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MySqlConnector;


var builder = WebApplication.CreateBuilder(args);

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:3000")
                        .AllowAnyHeader().AllowAnyMethod();
                    });
});





var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");


builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(MyAllowSpecificOrigins);
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
    try
    {
        context.Employees.Add(employee);
        await context.SaveChangesAsync();

        return Results.Created($"/api/employees/{employee.Id}", employee);
    }
    catch (DbUpdateException ex)
        when (ex.InnerException is MySqlException mySqlEx && mySqlEx.Number == 1062)
    {

        return Results.Conflict(new { message = $"The email '{employee.Email}' is already in use." });
    }
    catch (Exception ex)
    {

        return Results.Problem($"An unexpected error occurred: {ex.Message}");
    }
});


app.MapGet("/api/employees/{id:int}", async (DataContext context, int id) =>
{
    var employee = await context.Employees.FindAsync(id);

    if (employee == null)
    {
        return Results.NotFound(new { message = $"Employee with ID {id} not found." });
    }

    return Results.Ok(employee);
});


app.MapPut("/api/employees/{id:int}", async (DataContext context, int id, Employee.API.Models.Employee updatedEmployee) =>
{

    var existingEmployee = await context.Employees.FindAsync(id);


    if (existingEmployee == null)
    {
        return Results.NotFound(new { message = $"Employee with ID {id} not found." });
    }


    existingEmployee.FirstName = updatedEmployee.FirstName;
    existingEmployee.LastName = updatedEmployee.LastName;
    existingEmployee.Position = updatedEmployee.Position;
    existingEmployee.Email = updatedEmployee.Email;


    await context.SaveChangesAsync();


    return Results.Ok(existingEmployee);
});


app.MapDelete("/api/employees/{id:int}", async (DataContext context, int id) =>
{

    var employee = await context.Employees.FindAsync(id);


    if (employee == null)
    {
        return Results.NotFound(new { message = $"Employee with ID {id} not found." });
    }


    context.Employees.Remove(employee);


    await context.SaveChangesAsync();


    return Results.NoContent();
});

app.Run();

