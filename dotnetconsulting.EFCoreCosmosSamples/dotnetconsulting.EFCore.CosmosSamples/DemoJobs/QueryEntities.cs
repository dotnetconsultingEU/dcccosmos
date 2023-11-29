// Disclaimer
// Dieser Quellcode ist als Vorlage oder als Ideengeber gedacht. Er kann frei und ohne 
// Auflagen oder Einschränkungen verwendet oder verändert werden.
// Jedoch wird keine Garantie übernommen, dass eine Funktionsfähigkeit mit aktuellen und 
// zukünftigen API-Versionen besteht. Der Autor übernimmt daher keine direkte oder indirekte 
// Verantwortung, wenn dieser Code gar nicht oder nur fehlerhaft ausgeführt wird.
// Für Anregungen und Fragen stehe ich jedoch gerne zur Verfügung.

// Thorsten Kansy, www.dotnetconsulting.eu

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace dotnetconsulting.EFCore.CosmosSamples.DemoJobs;

public class QueryEntities(ILogger<CreateEntities> logger,
                     TechEventContext efContext) : IDemoJob
{
    private readonly ILogger _logger = logger;
    private readonly TechEventContext _efContext = efContext;

    public string Title => "Query Entities";

    public void Run()
    {
        Debugger.Break();

        // Sicherstellen, das die DB vorhanden ist
        _efContext.Database.EnsureCreated();
        // _efContext.DumpMetadataNoSql(_logger);

        var query1 = _efContext.Speakers
                        .Where(w => w.Name == "Thorsten Kansy")
                        .Select(w => new { w.SpeakerId, w.Name });

        // Verzögert Ausführung
        foreach (var item in query1)
            _logger.LogInformation("item: {item}", item);

        // ...auch mehrfach
        foreach (var item in query1)
            _logger.LogInformation("item: {item}", item);

        // IQueryable erweitern
        int count = query1.Count();
        _logger.LogInformation("count: {count}", count);

        // LIKE Funktionen - Nicht implmentiert
        var query2 = _efContext.Speakers
                        .Where(w => EF.Functions.Like(w.Name, "%Kansy%"))
                        .Select(w => new { w.SpeakerId, w.Name });

        //foreach (var item in query2)
        //    _logger.LogInformation(item.ToString());

        // User defined function - Nicht implmentiert
        var query3 = _efContext.Speakers
                        .Where(w => DbContextExt.StringLike(w.Name, "%Kansy%"))
                        .Select(w => new { w.SpeakerId, w.Name });

        //foreach (var item in query3)
        //    _logger.LogInformation(item.ToString());
    }
}