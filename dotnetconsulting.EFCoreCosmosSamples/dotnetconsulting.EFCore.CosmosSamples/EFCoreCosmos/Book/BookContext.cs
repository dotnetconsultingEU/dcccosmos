// Disclaimer
// Dieser Quellcode ist als Vorlage oder als Ideengeber gedacht. Er kann frei und ohne 
// Auflagen oder Einschränkungen verwendet oder verändert werden.
// Jedoch wird keine Garantie übernommen, dass eine Funktionsfähigkeit mit aktuellen und 
// zukünftigen API-Versionen besteht. Der Autor übernimmt daher keine direkte oder indirekte 
// Verantwortung, wenn dieser Code gar nicht oder nur fehlerhaft ausgeführt wird.
// Für Anregungen und Fragen stehe ich jedoch gerne zur Verfügung.

// Thorsten Kansy, www.dotnetconsulting.eu

using Microsoft.EntityFrameworkCore;

namespace dotnetconsulting.EFCore.CosmosSamples
{
    public class BookContext(DbContextOptions < BookContext > options) : DbContext(options)
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

        internal static void SeedDemoData()
        {
            // Aktuell kein Inhalt
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultContainer("Books");

            modelBuilder.Entity<Book>()
                .OwnsOne<Author>("Author")
                .WithOwner();

            modelBuilder.Entity<Book>()
                    .OwnsMany<Chapter>("Chapters")
                    .WithOwner();
        }

        public DbSet<Book> Books { get; set; }
    }
}