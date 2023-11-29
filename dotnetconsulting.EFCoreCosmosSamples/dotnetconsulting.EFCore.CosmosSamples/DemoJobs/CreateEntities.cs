// Disclaimer
// Dieser Quellcode ist als Vorlage oder als Ideengeber gedacht. Er kann frei und ohne 
// Auflagen oder Einschränkungen verwendet oder verändert werden.
// Jedoch wird keine Garantie übernommen, dass eine Funktionsfähigkeit mit aktuellen und 
// zukünftigen API-Versionen besteht. Der Autor übernimmt daher keine direkte oder indirekte 
// Verantwortung, wenn dieser Code gar nicht oder nur fehlerhaft ausgeführt wird.
// Für Anregungen und Fragen stehe ich jedoch gerne zur Verfügung.

// Thorsten Kansy, www.dotnetconsulting.eu

using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace dotnetconsulting.EFCore.CosmosSamples.DemoJobs;

public class CreateEntities(ILogger<CreateEntities> logger, TechEventContext efContext) : IDemoJob
{
#pragma warning disable IDE0052 // Remove unread private members
    private readonly ILogger _logger = logger;
#pragma warning restore IDE0052 // Remove unread private members
    private readonly TechEventContext _efContext = efContext;

    public string Title => "Create Entities";

    public void Run()
    {
        Debugger.Break();

        // Sicherstellen, das die DB vorhanden ist
        _efContext.Database.EnsureCreated();
        // _efContext.DumpMetadataNoSql(_logger);

        // Entitäten erstellen und befüllen
        Session session = new()
        {
            SessionId = Guid.NewGuid(),
            Title = " First EFCore Cosmos Workshop",
            Difficulty = DifficultyLevel.Level3,
            Begin = new DateTime(2019, 9, 23, 9, 0, 0),
            End = new DateTime(2019, 9, 23, 9, 0, 0),
            Created = DateTime.Now
        };

        Speaker speaker = new()
        {
            SpeakerId = Guid.NewGuid(),
            Name = "Thorsten Kansy",
            Homepage = "www.dotnetcosulting.eu",
            Twitter = "@tkansy"
        };

        // Relationen setzen
        speaker.Sessions.Add(session);
        session.Speaker = speaker;

        // An den Context anfügen
        _efContext.Sessions.Add(session);

        // Speichern
        _efContext.SaveChanges();
    }
}