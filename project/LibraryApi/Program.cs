using LibraryApi.Models;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Library API", Version = "v1" });
});

builder.Services.AddSingleton<LibraryApi.Models.Library>();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy => policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod());
});

var app = builder.Build();

var library = app.Services.GetRequiredService<LibraryApi.Models.Library>();

library.RegisterBook(new LibraryApi.Models.Book("111", "The C# Pro", 1));
library.RegisterBook(new LibraryApi.Models.Book("222", "Learning OOP", 5));
library.RegisterBook(new LibraryApi.Models.Book("333", "ASP.NET Core", 3));

library.RegisterPatron(new Patron(1, "Alice"));
library.RegisterPatron(new Patron(2, "Bob"));


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Library API V1");
    });
}

app.UseRouting();

app.UseCors("AllowReactApp");

app.MapControllers();

app.Run();
