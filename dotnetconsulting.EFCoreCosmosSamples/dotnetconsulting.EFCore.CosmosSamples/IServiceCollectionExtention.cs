// Disclaimer
// Dieser Quellcode ist als Vorlage oder als Ideengeber gedacht. Er kann frei und ohne 
// Auflagen oder Einschränkungen verwendet oder verändert werden.
// Jedoch wird keine Garantie übernommen, dass eine Funktionsfähigkeit mit aktuellen und 
// zukünftigen API-Versionen besteht. Der Autor übernimmt daher keine direkte oder indirekte 
// Verantwortung, wenn dieser Code gar nicht oder nur fehlerhaft ausgeführt wird.
// Für Anregungen und Fragen stehe ich jedoch gerne zur Verfügung.

// Thorsten Kansy, www.dotnetconsulting.eu

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace dotnetconsulting.EFCore.CosmosSamples;

static class IServiceCollectionExtention
{
    static public IServiceCollection AddDbContextWithDefaultConfiguration<TContext>
        (this IServiceCollection ServiceCollection,
         IConfigurationRoot Configuration,
         Action<DbContextOptionsBuilder> optionsAction = null!) where TContext : DbContext
    {
        string accountEndpoint = Configuration["CosmosDB:AccountEndpoint"];
        string accountKey = Configuration["CosmosDB:AccountKey"];
        string databaseName = Configuration["CosmosDB:DatabaseName"];

        ServiceCollection.AddDbContext<TContext>(o =>
        {
            o.UseCosmos(accountEndpoint, accountKey, databaseName, options =>
            {
                // options.WebProxy(myWebProxy);
                options.LimitToEndpoint();
                options.RequestTimeout(TimeSpan.FromSeconds(30));
                options.OpenTcpConnectionTimeout(TimeSpan.FromSeconds(60));
                // options.IdleTcpConnectionTimeout(TimeSpan.FromSeconds(30));
                options.GatewayModeMaxConnectionLimit(10);
                options.MaxTcpConnectionsPerEndpoint(10);
                options.MaxRequestsPerTcpConnection(10);
            })
            .UseLoggerFactory(EFLoggerFactory.Instance)
            .EnableSensitiveDataLogging(true)
            .EnableDetailedErrors(true);

            optionsAction?.Invoke(o);
        });

        return ServiceCollection;
    }
}