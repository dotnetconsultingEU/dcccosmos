// Disclaimer
// Dieser Quellcode ist als Vorlage oder als Ideengeber gedacht. Er kann frei und ohne 
// Auflagen oder Einschränkungen verwendet oder verändert werden.
// Jedoch wird keine Garantie übernommen, dass eine Funktionsfähigkeit mit aktuellen und 
// zukünftigen API-Versionen besteht. Der Autor übernimmt daher keine direkte oder indirekte 
// Verantwortung, wenn dieser Code gar nicht oder nur fehlerhaft ausgeführt wird.
// Für Anregungen und Fragen stehe ich jedoch gerne zur Verfügung.

// Thorsten Kansy, www.dotnetconsulting.eu

using Microsoft.EntityFrameworkCore;

namespace dotnetconsulting.EFCore.CosmosSamples.EFCoreCosmos.ShadowProperties;

public class ShadowPropertyContext (DbContextOptions<ShadowPropertyContext> options) : DbContext(options)
{


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseCosmos("https://localhost:8081",
                                     "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
                                     "EFCosmosDb");
            optionsBuilder.UseLoggerFactory(EFLoggerFactory.Instance);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultContainer("TechEventContainer");

        // Shadow Properties für Azure Comos DB
        modelBuilder.Entity<Speaker>(e =>
        {
            e.HasKey(p => p.SpeakerId);

            // e.Property<string>("id"); Nicht notwendig, da intern verwendet
            // Streng genommen ist ab 3.1 keins dieser Felder mehr notwendig,
            // da sie automatisch angelegt werden. Hier nur zu Demozwecken
            e.Property<string>("_rid");
            e.Property<string>("_self");
            e.Property<string>("_etag");
            e.Property<string>("_attachments");
            e.Property<long>("_ts");

            // Diese FluentAPI kann jedoch auch für eigene Felder verwendet 
            e.Property<decimal>("LastRating").ToJsonProperty("lastRating");
        });

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Speaker> Speakers { get; set; }
}