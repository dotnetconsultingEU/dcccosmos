// Disclaimer
// Dieser Quellcode ist als Vorlage oder als Ideengeber gedacht. Er kann frei und ohne 
// Auflagen oder Einschränkungen verwendet oder verändert werden.
// Jedoch wird keine Garantie übernommen, dass eine Funktionsfähigkeit mit aktuellen und 
// zukünftigen API-Versionen besteht. Der Autor übernimmt daher keine direkte oder indirekte 
// Verantwortung, wenn dieser Code gar nicht oder nur fehlerhaft ausgeführt wird.
// Für Anregungen und Fragen stehe ich jedoch gerne zur Verfügung.

// Thorsten Kansy, www.dotnetconsulting.eu

using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using dotnetconsulting.DotnetCore.DocumentDB.Models;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Newtonsoft.Json;

namespace dotnetconsulting.DotnetCore.DocumentDB
{
    public static class Demos
    {
        public static async void Run(DocumentDBContext DocumentDBContext)
        {
            Debugger.Break();

            Guid personId = Guid.NewGuid();
            Guid carId = Guid.NewGuid();

            await Task.Run(() => EnsureDatabaseExists(DocumentDBContext));
            await Task.Run(() => EnsureDocumentCollectionExists(DocumentDBContext));
            await Task.Run(() => CreateAndUpdatePerson(DocumentDBContext, personId));
            await Task.Run(() => CreateAndUpdateCar(DocumentDBContext, carId));
            await Task.Run(() => CreateCrowd(DocumentDBContext, 5));
            await Task.Run(() => QueryPersons(DocumentDBContext));
            await Task.Run(() => DeletePerson(DocumentDBContext, personId));

            await Task.Run(() => AdjustRUOffer(DocumentDBContext, 500));
        }

        private static async Task EnsureDatabaseExists(DocumentDBContext DocumentDBContext)
        {
            Debugger.Break();

            Console.WriteLine("EnsureDatabaseExists()");

            ResourceResponse<Database> response;

            try
            {
                // Zugriff auf Datenbank um zu prüfen, ob diese existiert
                response = await DocumentDBContext.Client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(DocumentDBContext.DatabaseId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    // Datenbank anlegen
                    response = await DocumentDBContext.Client.CreateDatabaseAsync(
                        new Database { Id = DocumentDBContext.DatabaseId });
                    Console.WriteLine("Database created");
                }
                else
                {
                    throw;
                }
            }

            // Details ausgeben
            Console.WriteLine(response.Resource);
        }

        private static async Task EnsureDocumentCollectionExists(DocumentDBContext DocumentDBContext)
        {
            Debugger.Break();

            Console.WriteLine("EnsureDocumentCollectionExists()");

            ResourceResponse<DocumentCollection> response;

            string databaseId = DocumentDBContext.DatabaseId;
            string collectionId = DocumentDBContext.CollectionId;

            try
            {
                // Zugriff auf Collection um zu prüfen, ob diese existiert               
                response = await DocumentDBContext.Client.ReadDocumentCollectionAsync(
                    UriFactory.CreateDocumentCollectionUri(databaseId, collectionId)
                );
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    // Collection anlegen
                    response = await DocumentDBContext.Client.CreateDocumentCollectionAsync(
                         UriFactory.CreateDatabaseUri(databaseId),
                         new DocumentCollection { Id = collectionId },
                         new RequestOptions { OfferThroughput = 400 });
                    Console.WriteLine("Collection created");
                }
                else
                {
                    throw;
                }
            }

            // Details ausgeben
            Console.WriteLine(response.Resource);
        }

        private static async Task CreateAndUpdatePerson(DocumentDBContext DocumentDBContext, Guid PersonId)
        {
            Debugger.Break();

            Console.WriteLine($"CreateAndUpdatePerson({PersonId})");

            ResourceResponse<Document> response;

            string databaseId = DocumentDBContext.DatabaseId;
            string collectionId = DocumentDBContext.CollectionId;

            // POCO erzeugen
            Person item1 = new()
            {
                Id = PersonId.ToString(),
                Firstname = "Harry",
                Name = "Hirsch",
                BirthDay = DateTime.Today.AddDays(-12302)
            };

            // Anlegen
#pragma warning disable IDE0059 // Unnecessary assignment of a value
            response = await DocumentDBContext.Client
                .CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(databaseId, collectionId), 
                item1);
#pragma warning restore IDE0059 // Unnecessary assignment of a value

            // Ändern
            item1.Name = "Kron";
#pragma warning disable IDE0059 // Unnecessary assignment of a value
            response = await DocumentDBContext.Client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(databaseId, collectionId, item1.Id), item1);
#pragma warning restore IDE0059 // Unnecessary assignment of a value
        }

        private static async Task CreateAndUpdateCar(DocumentDBContext DocumentDBContext, Guid CarId)
        {
            Debugger.Break();

            Console.WriteLine($"CreateAndUpdateCar({CarId})");

            ResourceResponse<Document> response;

            string databaseId = DocumentDBContext.DatabaseId;
            string collectionId = DocumentDBContext.CollectionId;

            // Poco anlegen
            Car item1 = new()
            {
                Id = CarId.ToString(),
                Brand = "Landrover Defender 110",
                Color = "Silver",
                Year = 1991,
                Name = "Monster"
            };

            // Anlegen
#pragma warning disable IDE0059 // Unnecessary assignment of a value
            response = await DocumentDBContext.Client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(databaseId, collectionId), item1);
#pragma warning restore IDE0059 // Unnecessary assignment of a value

            // Ändern
            item1.Name = "Monster 2";
#pragma warning disable IDE0059 // Unnecessary assignment of a value
            response = await DocumentDBContext.Client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(databaseId, collectionId, item1.Id), item1);
#pragma warning restore IDE0059 // Unnecessary assignment of a value
        }

        private static async void CreateCrowd(DocumentDBContext DocumentDBContext, int NumberOfPersons)
        {
            Debugger.Break();

            Console.WriteLine($"CreateCrowd({NumberOfPersons})");

            string databaseId = DocumentDBContext.DatabaseId;
            string collectionId = DocumentDBContext.CollectionId;

            // Elemente erzeugen
            for (int i = 0; i < NumberOfPersons; i++)
            {
                Person item = new()
                {
                    Id = Guid.NewGuid().ToString(),
                    Firstname = $"Harry Nr. {i + 1}",
                    Name = $"Hirsch #{i + 1}",
                    BirthDay = DateTime.Today.AddDays(-12302 - 10 * i),
                    Weight = 80 + i * 2
                };

                // Und einfügen
                await DocumentDBContext.Client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(databaseId, collectionId), item);
            }
        }

        private static async void DeletePerson(DocumentDBContext DocumentDBContext, Guid PersonId)
        {
            Debugger.Break();

            Console.WriteLine($"DeletePerson({PersonId})");

            string databaseId = DocumentDBContext.DatabaseId;
            string collectionId = DocumentDBContext.CollectionId;

            try
            {
                await DocumentDBContext.Client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(databaseId, collectionId, PersonId.ToString()));
            }
            catch (DocumentClientException ex) when (ex.StatusCode == HttpStatusCode.TooManyRequests)
            {
                throw;
            }
        }

        private static async void QueryPersons(DocumentDBContext DocumentDBContext)
        {
            Debugger.Break();

            Console.WriteLine($"QueryPersons()");

            string databaseId = DocumentDBContext.DatabaseId;
            string collectionId = DocumentDBContext.CollectionId;

            // Abfrage definieren
            IDocumentQuery<Person> query = DocumentDBContext.Client.CreateDocumentQuery<Person>(
                UriFactory.CreateDocumentCollectionUri(databaseId, collectionId),
                new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true })
                .Where(p => p.Name == "Hirsch #2")
                .AsDocumentQuery();

            // Ergebnis durchlaufen
            foreach (Person item in (await query.ExecuteNextAsync<Person>()))
            {
                Console.WriteLine($"Person: {JsonConvert.SerializeObject(item, Formatting.Indented)}");
            }
        }

        private static async void AdjustRUOffer(DocumentDBContext DocumentDBContext, int offerThroughput)
        {
            Debugger.Break();

            Console.WriteLine($"AdjustRUOffer()");

            // SelfLink der gewünschten Collection abfragen
            string collectionSelfLink = DocumentDBContext.CollectionSelfLink;

            // Offer abfragen
            Offer offer = (DocumentDBContext.Client.CreateOfferQuery()
                .Where(r => r.ResourceLink == collectionSelfLink)).ToList().SingleOrDefault();

            // Sollte nicht auftreten
            Debug.Assert(offer != null, "Offer == null");

            // Offer Throughput setzen
            OfferV2 offer2 = new(offer, offerThroughput);
            await DocumentDBContext.Client.ReplaceOfferAsync(offer);
        }
    }
}