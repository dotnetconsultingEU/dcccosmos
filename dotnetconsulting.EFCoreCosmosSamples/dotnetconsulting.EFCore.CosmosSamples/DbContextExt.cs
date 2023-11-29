// Disclaimer
// Dieser Quellcode ist als Vorlage oder als Ideengeber gedacht. Er kann frei und ohne 
// Auflagen oder Einschränkungen verwendet oder verändert werden.
// Jedoch wird keine Garantie übernommen, dass eine Funktionsfähigkeit mit aktuellen und 
// zukünftigen API-Versionen besteht. Der Autor übernimmt daher keine direkte oder indirekte 
// Verantwortung, wenn dieser Code gar nicht oder nur fehlerhaft ausgeführt wird.
// Für Anregungen und Fragen stehe ich jedoch gerne zur Verfügung.

// Thorsten Kansy, www.dotnetconsulting.eu

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;
using static System.Console;

namespace dotnetconsulting.EFCore.CosmosSamples
{
    public static class DbContextExt
    {
        public static void DumpMetadataRelational(this DbContext Context, ILogger Logger = null)
        {
            // Benötigt "Microsoft.EntityFrameworkCore.Relational"

            writeLine($"=== {Context.GetType()} Metadata ===");
            writeLine($"Provider {Context.Database.ProviderName}");

            foreach (IEntityType entityType in Context.Model.GetEntityTypes())
            {
                writeLine("");
                string name = entityType.Name;
                string tableName = entityType.GetTableName()!;
                writeLine($"{name} ({tableName})");

                foreach (IProperty propertyType in entityType.GetProperties())
                {
                    string columnName = propertyType.GetColumnName(StoreObjectIdentifier.Table(
                            entityType.GetTableName()!,
                            entityType.GetSchema()!));

                    Type columnType = propertyType.ClrType;
                    writeLine($"\t{columnName} ({columnType})");
                }
            }

            writeLine("===");

            void writeLine(string line)
            {
                if (Logger != null)
                    Logger.LogInformation("line: {line}", line);
                else
                    WriteLine(line);
            }
        }

        public static void DumpMetadataNoSql(this DbContext Context, ILogger Logger = null)
        {
            // Benötigt "Microsoft.EntityFrameworkCore.Cosmos"

            writeLine($"=== {Context.GetType()} Metadata ===");
            writeLine($"Provider {Context.Database.ProviderName}");

            foreach (IEntityType entityType in Context.Model.GetEntityTypes())
            {
                writeLine("");
                string name = entityType.Name;
                string containerName = entityType.GetContainer()!;
                writeLine($"{name} ({containerName})");

                foreach (IProperty propertyType in entityType.GetProperties())
                {
                    string columnName = propertyType.GetColumnName(StoreObjectIdentifier.Table(
                            entityType.GetTableName()!,
                            entityType.GetSchema()!));

                    Type columnType = propertyType.ClrType;
                    writeLine($"\t{columnName} ({columnType})");
                }
            }

            writeLine("===");

            void writeLine(string line)
            {
                if (Logger != null)
                    Logger.LogInformation("line: {line}", line);
                else
                    WriteLine(line);
            }
        }

        [DbFunction("StringLike")]
#pragma warning disable IDE0060 // Remove unused parameter
        public static bool StringLike(string @string, string Pattern)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            throw new Exception("No direct Call");
        }
    }
}