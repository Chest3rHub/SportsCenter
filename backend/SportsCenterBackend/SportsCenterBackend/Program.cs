using Microsoft.EntityFrameworkCore;
using SportsCenterBackend.Context;
using SportsCenterBackend.Controllers;
using SportsCenterBackend.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<IRegisterDbService,RegisterDbService>();
builder.Services.AddTransient<ILoginDbService,LoginDbService>();

builder.Services.AddDbContext<SportsCenterDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("mssqlConnString")));


builder.Services.AddControllers();
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

app.UseAuthorization();

app.MapControllers();

app.Run();