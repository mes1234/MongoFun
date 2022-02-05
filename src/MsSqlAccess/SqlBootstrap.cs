using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsSqlAccess
{
    public static class SqlDIExtensions
    {
        public static void InstallMsSqlAccess(this IServiceCollection builder, ConfigurationManager configuration)
        {
            builder.AddDbContext<ItemContext>(c => c.UseSqlServer(configuration.GetConnectionString("MsSql")));
        }
    }

    public class BootstrapDb : IHostedService
    {
        public BootstrapDb(IServiceScopeFactory scopeFactory, ILogger<BootstrapDb> logger)
        {
            using var scope = scopeFactory.CreateScope();

            var itemContext = scope.ServiceProvider.GetRequiredService<ItemContext>();

            if (!itemContext.Database.EnsureCreated())
                logger.LogInformation("Database {DbName} created successfully ", itemContext.Database.GetDbConnection().Database);
            else
                logger.LogInformation("Database {DbName} already exists ", itemContext.Database.GetDbConnection().Database);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }



}
