using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SportsCenterBackend.Context;
using SportsCenterBackend.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<IRegisterDbService, RegisterDbService>();
builder.Services.AddTransient<ILoginDbService, LoginDbService>();
builder.Services.AddTransient<IProductDbService, ProductDbService>();

// Configure DbContext
builder.Services.AddDbContext<SportsCenterDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("mssqlConnString")));

// Add controllers
builder.Services.AddControllers();
//builder.Services.AddControllers().AddJsonOptions(options =>
//{
//    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
//});
// "$id": "1", takie pola sa zwracane w jsonie gdy dodam te options, Include wtedy dziala ale tego pola nie bylo

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
