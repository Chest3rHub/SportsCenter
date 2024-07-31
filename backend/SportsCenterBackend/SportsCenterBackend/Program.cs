using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SportsCenterBackend.Context;
using SportsCenterBackend.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<IRegisterDbService, RegisterDbService>();
builder.Services.AddTransient<ILoginDbService, LoginDbService>();

// Configure DbContext
builder.Services.AddDbContext<SportsCenterDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("mssqlConnString")));

// Register ProductDbService
builder.Services.AddScoped<IProductDbService, ProductDbService>();

// Add controllers
builder.Services.AddControllers();

// Configure Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "SportsCenter API",
        Version = "v1"
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
