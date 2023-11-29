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

public class CreateNestedEntity(ILogger<CreateEntities> logger, BookContext efContext) : IDemoJob
{
#pragma warning disable IDE0052 // Remove unread private members
    private readonly ILogger _logger = logger;
#pragma warning restore IDE0052 // Remove unread private members
    private readonly BookContext _efContext = efContext;

    public string Title => "Create nested Entity";

    public void Run()
    {
        Debugger.Break();

        // Sicherstellen, das die DB vorhanden ist
        _efContext.Database.EnsureCreated();
        // _efContext.DumpMetadataNoSql(_logger);

        // POCO erstellen
        Book book = new()
        {
            Title = "How to EF Core",
            Author = new Author()
            {
                Name = "Thorsten Kansy"
            },
            Chapters = new List<Chapter>()
                {
                    new() { Index = 1, Title = "Chapter #1", Content = "..."},
                    new() { Index = 2, Title = "Chapter #2", Content = "..."},
                    new() { Index = 3, Title = "Chapter #3", Content = "..."},
                    new() { Index = 4, Title = "Chapter #4", Content = "..."}
                }
        };

        _efContext.Add(book);

        // Speichern
        _efContext.SaveChanges();
    }
}