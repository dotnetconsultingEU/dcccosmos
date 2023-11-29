// Disclaimer
// Dieser Quellcode ist als Vorlage oder als Ideengeber gedacht. Er kann frei und ohne 
// Auflagen oder Einschränkungen verwendet oder verändert werden.
// Jedoch wird keine Garantie übernommen, dass eine Funktionsfähigkeit mit aktuellen und 
// zukünftigen API-Versionen besteht. Der Autor übernimmt daher keine direkte oder indirekte 
// Verantwortung, wenn dieser Code gar nicht oder nur fehlerhaft ausgeführt wird.
// Für Anregungen und Fragen stehe ich jedoch gerne zur Verfügung.

// Thorsten Kansy, www.dotnetconsulting.eu

using dotnetconsulting.EFCore.CosmosSamples;
using dotnetconsulting.EFCore.CosmosSamples.DemoJobs;
using dotnetconsulting.EFCore.CosmosSamples.EFCoreCosmos.ShadowProperties;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

// Konfiguration vorbereiten
IConfigurationRoot config = createConfigurationRoot();

// Konfiguration und IoC initialisieren
IServiceProvider iocContainer = createDependencyInjectionContainer(config);

// DemoApp starten
DemoApplication app = new(iocContainer);
app.Run();

static IConfigurationRoot createConfigurationRoot()
{
    IConfigurationBuilder configBuilder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .AddUserSecrets<Program>();

    return configBuilder.Build();
}

static IServiceProvider createDependencyInjectionContainer(IConfigurationRoot Configuration)
{
    IServiceCollection iocContainer = new ServiceCollection();

    // Logging einrichtgen
    iocContainer.AddLogging((builder) =>
    {
        builder.SetMinimumLevel(LogLevel.Trace)
               .AddDebug()
               .AddConsole();
    })

    // Konfiguration
    .AddSingleton(Configuration);

    // LoggerFactory für EF einrichten
    // EFLoggerFactory.SetupFactory(LogLevel.Trace);
    EFLoggerFactory.SetupFactory(iocContainer.BuildServiceProvider());

    // EF Contexte konfigurieren
    iocContainer
        .AddDbContextWithDefaultConfiguration<TechEventContext>(Configuration)
        .AddDbContextWithDefaultConfiguration<BookContext>(Configuration)
        .AddDbContextWithDefaultConfiguration<ShadowPropertyContext>(Configuration)

    // Demos hinzufügen
    .AddTransient<CreateEntities>()
    .AddTransient<CreateNestedEntity>()
    .AddTransient<ShadowProperties>()
    .AddTransient<QueryEntities>()
    ;

    // Rückgabe
    IServiceProvider serviceProvider = iocContainer.BuildServiceProvider();

    return serviceProvider;
}