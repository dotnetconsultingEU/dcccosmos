// Disclaimer
// Dieser Quellcode ist als Vorlage oder als Ideengeber gedacht. Er kann frei und ohne 
// Auflagen oder Einschränkungen verwendet oder verändert werden.
// Jedoch wird keine Garantie übernommen, dass eine Funktionsfähigkeit mit aktuellen und 
// zukünftigen API-Versionen besteht. Der Autor übernimmt daher keine direkte oder indirekte 
// Verantwortung, wenn dieser Code gar nicht oder nur fehlerhaft ausgeführt wird.
// Für Anregungen und Fragen stehe ich jedoch gerne zur Verfügung.

// Thorsten Kansy, www.dotnetconsulting.eu

using System;
using System.Threading.Tasks;
using dotnetconsulting.Dotnet.DocumentDB.Models;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System.Linq;

namespace dotnetconsulting.Dotnet.DocumentDB
{
    class Program
    {
#pragma warning disable IDE0060 // Remove unused parameter
        static void Main(string[] args)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            var config = Helper.GetConfigurationValues();

            // DemoListDbCollDocs(config);
            // DemoQuery1(config);
            // DemoQuery2(config);
            // DemoQuery3(config);
            Task.Run(() => DemoCreateDoc(config)).Wait();
            // Task.Run(() => DemoUpdateDoc(config)).Wait();
            Task.Run(() => DemoDeleteDoc(config)).Wait();

            Console.WriteLine("== Fertig ==");
            Console.ReadKey();
        }

        private static void DemoListDbCollDocs((string endPoint, string key, string databaseId, string collectionId) config)
        {
            using (DocumentClient client = Helper.CreateDocumentClient(config.endPoint, config.key))
            {
                // Welche Datenbanken gibt es?
                var databases = client.CreateDatabaseQuery();

                foreach (Database database in databases)
                {
                    Console.WriteLine("- Database -");
                    Console.WriteLine(database);

                    // Welche Collections gibt es?
                    var collections = client.CreateDocumentCollectionQuery(database.SelfLink);

                    foreach (DocumentCollection collection in collections)
                    {
                        Console.WriteLine("- Collection -");
                        Console.WriteLine(collection);

                        // Welche Dokumente gibt es?

                        // Ist ein SELECT involviert, so müssen ggf. Optionen gesetzt werden 
                        FeedOptions options = new FeedOptions()
                        {
                            // PartitionKey = new PartitionKey("New York"), 
                            EnableCrossPartitionQuery = true
                        };

                        var documents = client.CreateDocumentQuery(collection.SelfLink/*, "SELECT * FROM c", options*/);

                        int n = 1;
                        foreach (dynamic document in documents)
                        {
                            Console.WriteLine($"- Document #{n++} -");
                            Console.WriteLine(document);
                            Console.WriteLine();
                        }
                    }
                }
            }
        }

        private static void DemoQuery1((string endPoint, string key, string databaseId, string collectionId) config)
        {
            // DocumentClient erzeugen
            using (DocumentClient client = Helper.CreateDocumentClient(config.endPoint, config.key))
            {
                // CollectionUri erzeugen
                Uri collectionUri = UriFactory.CreateDocumentCollectionUri(config.databaseId, config.collectionId);
                Console.WriteLine($"collectionUri={collectionUri}");

                // Abfrage definieren
                var documents = client.CreateDocumentQuery(collectionUri);

                // Abfrage ausführen
                int n = 1;
                foreach (dynamic document in documents)
                {
                    Console.WriteLine($"- Document #{n++} -");
                    Console.WriteLine(document);
                    Console.WriteLine();
                }
            }
        }

        private static void DemoQuery2((string endPoint, string key, string databaseId, string collectionId) config)
        {
            // DocumentClient erzeugen
            using (DocumentClient client = Helper.CreateDocumentClient(config.endPoint, config.key))
            {
                // CollectionUri erzeugen
                Uri collectionUri = UriFactory.CreateDocumentCollectionUri(config.databaseId, config.collectionId);
                Console.WriteLine($"collectionUri={collectionUri}");

                FeedOptions options = new FeedOptions()
                {
                    // PartitionKey = new PartitionKey("New York"),
                    EnableCrossPartitionQuery = true
                };

                // SQL Abfrage (Ohne PartitionKey muss CrossPartitionQuery aktiviert werden oder PartitionKey angegeben)
                // string sql = "SELECT * FROM c WHERE c.city = 'New York' AND c.FirstName = 'Ken'";
                string sql = "SELECT * FROM c WHERE c.FirstName = 'Ken'";

                // Abfrage definieren
                var documents = client.CreateDocumentQuery(collectionUri, sql, options);

                // Abfrage ausführen
                int n = 1;
                foreach (dynamic document in documents)
                {
                    Console.WriteLine($"- Document #{n++} -");
                    Console.WriteLine(document);
                    Console.WriteLine();
                }
            }
        }

        private static void DemoQuery3((string endPoint, string key, string databaseId, string collectionId) config)
        {
            // DocumentClient erzeugen
            using (DocumentClient client = Helper.CreateDocumentClient(config.endPoint, config.key))
            {
                // CollectionUri erzeugen
                Uri collectionUri = UriFactory.CreateDocumentCollectionUri(config.databaseId, config.collectionId);
                Console.WriteLine($"collectionUri={collectionUri}");

                // Optionen definieren
                FeedOptions options = new FeedOptions()
                {
                    PartitionKey = new PartitionKey("New York"),
                    EnableCrossPartitionQuery = true
                };

                // SQL Abfrage (Ohne PartitionKey muss CrossPartitionQuery aktiviert werden oder PartitionKey angegeben)
                SqlQuerySpec sql = new SqlQuerySpec
                {
                    QueryText = "SELECT * FROM c WHERE c.FirstName = @FirstName",
                    Parameters = new SqlParameterCollection()
                    {
                            new SqlParameter("@FirstName", "Ken")
                    }
                };

                // Abfrage definieren
                IQueryable<dynamic> documents = client.CreateDocumentQuery(collectionUri, sql, options);

                // Abfrage ausführen
                int n = 1;
                foreach (dynamic document in documents)
                {
                    Console.WriteLine($"- Document #{n++} -");
                    Console.WriteLine(document);
                    Console.WriteLine();
                }
            }
        }

        private static async void DemoCreateDoc((string endPoint, string key, string databaseId, string collectionId) config)
        {
            // DocumentClient erzeugen
            using (DocumentClient client = Helper.CreateDocumentClient(config.endPoint, config.key))
            {
                // CollectionUri erzeugen
                Uri collectionUri = UriFactory.CreateDocumentCollectionUri(config.databaseId, config.collectionId);
                Console.WriteLine($"collectionUri={collectionUri}");

                // Poco (oder Expandable-Object) anlegen
                Car item1 = new Car()
                {
                    Id = Guid.NewGuid(), // (darf einmal je Partition leer sein!)
                    Brand = "Landrover Defender 110",
                    Color = "Silver",
                    Year = 1991,
                    Name = "Monster",
                    GarageLocation = "Berlin"
                };

                // Anlegen
                ResourceResponse<Document> response = await client.CreateDocumentAsync(collectionUri, item1);

                Console.WriteLine(response.Resource);
            }
        }

        private static async void DemoUpdateDoc((string endPoint, string key, string databaseId, string collectionId) config)
        {
            string id = "8c1e0bc4-cbac-4e90-b749-c8ed55e87c63";

            // DocumentClient erzeugen
            using (DocumentClient client = Helper.CreateDocumentClient(config.endPoint, config.key))
            {
                // CollectionUri erzeugen
                Uri collectionUri = UriFactory.CreateDocumentCollectionUri(config.databaseId, config.collectionId);
                Console.WriteLine($"collectionUri={collectionUri}");

                // Dokument laden
                FeedOptions options = new FeedOptions()
                {
                    // PartitionKey = new PartitionKey("New York"), 
                    EnableCrossPartitionQuery = true
                };

                // SQL Abfrage (Ohne PartitionKey muss CrossPartitionQuery aktiviert werden oder PartitionKey angegeben)
                SqlQuerySpec sql = new SqlQuerySpec
                {
                    QueryText = "SELECT * FROM c WHERE c.id = @id",
                    Parameters = new SqlParameterCollection()
                    {
                            new SqlParameter("@id", id)
                    }
                };

                // Abfrage und Deserialisierung in POCO
                Car car = client.CreateDocumentQuery(collectionUri, sql, options).AsEnumerable().FirstOrDefault();

                // Gefunden?
                if (car != null)
                {
                    Uri documentUrI = UriFactory.CreateDocumentUri(config.databaseId, config.collectionId, id);

                    // Aktualisieren
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                    ResourceResponse<Document> response = await client.ReplaceDocumentAsync(documentUrI, car);
#pragma warning restore IDE0059 // Unnecessary assignment of a value
                }
            }
        }

        private static async void DemoDeleteDoc((string endPoint, string key, string databaseId, string collectionId) config)
        {
            string id = "95e4db73-3489-4ad9-b361-ff4da766a249";

            // DocumentClient erzeugen
            using (DocumentClient client = Helper.CreateDocumentClient(config.endPoint, config.key))
            {
                // DocumentUri erzeugen
                Uri documentUri = UriFactory.CreateDocumentUri(config.databaseId, config.collectionId, id);
                Console.WriteLine($"documentUri={documentUri}");

                RequestOptions options = new RequestOptions()
                {
                    PartitionKey = new PartitionKey("Berlin")
                };

#pragma warning disable IDE0059 // Unnecessary assignment of a value
                ResourceResponse<Document> result = await client.DeleteDocumentAsync(documentUri, options);
#pragma warning restore IDE0059 // Unnecessary assignment of a value
            }
        }
    }
}