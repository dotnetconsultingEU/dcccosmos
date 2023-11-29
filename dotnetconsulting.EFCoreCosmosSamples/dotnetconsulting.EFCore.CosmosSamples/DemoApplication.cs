// Disclaimer
// Dieser Quellcode ist als Vorlage oder als Ideengeber gedacht. Er kann frei und ohne 
// Auflagen oder Einschränkungen verwendet oder verändert werden.
// Jedoch wird keine Garantie übernommen, dass eine Funktionsfähigkeit mit aktuellen und 
// zukünftigen API-Versionen besteht. Der Autor übernimmt daher keine direkte oder indirekte 
// Verantwortung, wenn dieser Code gar nicht oder nur fehlerhaft ausgeführt wird.
// Für Anregungen und Fragen stehe ich jedoch gerne zur Verfügung.

// Thorsten Kansy, www.dotnetconsulting.eu

using dotnetconsulting.EFCore.CosmosSamples.DemoJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace dotnetconsulting.EFCore.CosmosSamples;

public class DemoApplication(IServiceProvider iocContainer)
{
    private readonly IServiceProvider iocContainer = iocContainer;

    public void Run()
    {
        ILogger logger = iocContainer.GetService<ILogger<DemoApplication>>();
        logger.LogInformation("== Running ==");

        // Demos
        IDemoJob demoJob;
        demoJob = iocContainer.GetService<CreateEntities>()!;
        //demoJob = iocContainer.GetService<CreateNestedEntity>()!;
        //demoJob = iocContainer.GetService<QueryEntities>()!;
        // demoJob = iocContainer.GetService<ShadowProperties>()!;

        // Und Action!
        Console.WriteLine($"=== {demoJob.Title} ===");
        // _efContext.Database.EnsureDeleted(); // Ggf einkommentieren
        demoJob.Run();

        logger.LogInformation("== Fertig ==");
    }
}