using System.Reflection;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SportsCenter.Application.Abstractions;
using SportsCenter.Application.Security;
using SportsCenter.Core.Abstractions;
using SportsCenter.Core.Entities;
using SportsCenter.Core.Repositories;
using SportsCenter.Infrastructure.Abstractions;
using SportsCenter.Infrastructure.Behaviors;
using SportsCenter.Infrastructure.DAL;
using SportsCenter.Infrastructure.DAL.Repositories;
using SportsCenter.Infrastructure.Security;
using SportsCenter.Infrastructure.Time;
using SportsCenterBackend.Context;

namespace SportsCenter.Infrastructure;

public static class Extensions
{
    private const string DalOptionsSectionName = "Database";

    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services,
        IConfiguration configuration)
    {
        //Mediatr
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehavior<,>));

        //DAL
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.Configure<MsSqlServerOptions>(configuration.GetRequiredSection(DalOptionsSectionName));
        var databaseOptions = configuration.GetOptions<MsSqlServerOptions>(DalOptionsSectionName);
        services.AddDbContext<SportsCenterDbContext>(x => x.UseSqlServer(databaseOptions.ConnectionString));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();

        //Security
        services.AddSingleton<IPasswordManager, PasswordManager>();
        services.AddSingleton<IPasswordHasher<Osoba>, PasswordHasher<Osoba>>();

        //Time
        services.AddSingleton<IClock, Clock>();

        return services;
    }

    private static T GetOptions<T>(this IConfiguration configuration, string sectionName) where T : class, new()
    {
        var options = new T();
        var section = configuration.GetRequiredSection(sectionName);
        section.Bind(options);

        return options;
    }
}