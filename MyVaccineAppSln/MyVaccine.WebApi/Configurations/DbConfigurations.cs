using Microsoft.EntityFrameworkCore;
using MyVaccine.WebApi.Models;

namespace MyVaccine.WebApi.Configurations;

public static class DbConfigurations
{
    public static IServiceCollection SetDatabaseConfiguration(this IServiceCollection services)
    {
        //var connectionString = Environment.GetEnvironmentVariable(MyVaccineLiterals.CONNECTION_STRING);
        var connectionString = "Server=localhost,14330;Database=MyVaccineAppDb;User Id=sa;Password=Abc.123456;TrustServerCertificate=true;";
        services.AddDbContext<MyVaccineAppDbContext>(options =>
            options.UseSqlServer(connectionString
                )
            );
        return services;
    }
}
