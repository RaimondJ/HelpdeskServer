using Microsoft.EntityFrameworkCore;
using HelpdeskServer.Data;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using HelpdeskServer.Models;
using HelpdeskServer.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<PostRepository>();
builder.Services.AddScoped<HelpdeskService>();
builder.Services.AddDbContext<HelpdeskDbContext>(options => {

    var connectionString = builder.Configuration.GetConnectionString("DatabaseConnection");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});
var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseCors(builder => builder
               .AllowAnyHeader()
               .AllowAnyMethod()
               .SetIsOriginAllowed((host) => true)
               .AllowCredentials()
           );
app.Run();

public partial class Program { }
