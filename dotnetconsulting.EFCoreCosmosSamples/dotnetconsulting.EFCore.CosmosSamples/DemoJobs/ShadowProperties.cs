// Disclaimer
// Dieser Quellcode ist als Vorlage oder als Ideengeber gedacht. Er kann frei und ohne 
// Auflagen oder Einschränkungen verwendet oder verändert werden.
// Jedoch wird keine Garantie übernommen, dass eine Funktionsfähigkeit mit aktuellen und 
// zukünftigen API-Versionen besteht. Der Autor übernimmt daher keine direkte oder indirekte 
// Verantwortung, wenn dieser Code gar nicht oder nur fehlerhaft ausgeführt wird.
// Für Anregungen und Fragen stehe ich jedoch gerne zur Verfügung.

// Thorsten Kansy, www.dotnetconsulting.eu

using dotnetconsulting.EFCore.CosmosSamples.EFCoreCosmos.ShadowProperties;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace dotnetconsulting.EFCore.CosmosSamples.DemoJobs;

public class ShadowProperties(ILogger<ShadowProperties> logger, ShadowPropertyContext efContext) : IDemoJob
{
#pragma warning disable IDE0052 // Remove unread private members
    private readonly ILogger _logger = logger;
#pragma warning restore IDE0052 // Remove unread private members
    private readonly ShadowPropertyContext _efContext = efContext;

    public string Title => "Shadow Properties";

    public void Run()
    {
        // Debugger.Break();

        // Sicherstellen, das die DB vorhanden ist
        _efContext.Database.EnsureCreated();
        // _efContext.DumpMetadataNoSql(_logger);

        #region Entität mit Shadow Property erzeugen
        Speaker speaker = new()
        {
            SpeakerId = Guid.NewGuid(),
            Name = "Thorsten Kansy",
            Homepage = "www.dotnetcosulting.eu",
            Twitter = "@tkansy"
        };
        _efContext.Entry(speaker).Property("LastRating").CurrentValue = 1.7m;

        _efContext.Add(speaker);
        _efContext.SaveChanges();
        #endregion

        #region Abfragen mit Shadow Property
        speaker = _efContext.Speakers.First();

        // Wert lesen
        // Im Container eindeutige Azure Comos DB Id
        string id = (string)_efContext.Entry(speaker).Property("id").CurrentValue;
        // Interne Felder Azure Comos DB
        string rid = (string)_efContext.Entry(speaker).Property("_rid").CurrentValue;
        string self = (string)_efContext.Entry(speaker).Property("_self").CurrentValue;
        string etag = (string)_efContext.Entry(speaker).Property("_etag").CurrentValue;
        string attachements = (string)_efContext.Entry(speaker).Property("_attachments").CurrentValue;
        long ts = (long)_efContext.Entry(speaker).Property("_ts").CurrentValue;

        // Sogar das komplette Json-Objekt liegt vor
        JObject jobject = (JObject)_efContext.Entry(speaker).Property("__jObject").CurrentValue;

        // Aktuell ist immer noch ein Discriminator vorhanden (3.1, Stand 23.12.19),
        // ebenfalls eine Shadow Property ist

        decimal lastRating = (decimal)_efContext.Entry(speaker).Property("LastRating").CurrentValue;

        // Exception, wenn Eigenschaft nicht im Model vorhanden oder Datentyp unpassen
        // string invalidName = (string)_efContext.Entry(speaker).Property("invalidName").CurrentValue;

        // Werte verändern
        _efContext.Entry(speaker).Property("LastRating").CurrentValue = 1.1m;

        // Änderungen erkannt?
        bool hasChanges = _efContext.ChangeTracker.HasChanges();

        _efContext.SaveChanges();
        #endregion

        #region Abfrage via Shadow Property 
        speaker = _efContext.Speakers
                    .Where(w => EF.Property<string>(w, "_rid") == "Qn1fAI+gvBQCAAAAAAAAAA==")
                    .FirstOrDefault();

        var speakers = _efContext.Speakers
                         .OrderBy(o => EF.Property<long>(o, "_ts"))
                         .ToList();
        #endregion
    }
}