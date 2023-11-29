// Disclaimer
// Dieser Quellcode ist als Vorlage oder als Ideengeber gedacht. Er kann frei und ohne 
// Auflagen oder Einschränkungen verwendet oder verändert werden.
// Jedoch wird keine Garantie übernommen, dass eine Funktionsfähigkeit mit aktuellen und 
// zukünftigen API-Versionen besteht. Der Autor übernimmt daher keine direkte oder indirekte 
// Verantwortung, wenn dieser Code gar nicht oder nur fehlerhaft ausgeführt wird.
// Für Anregungen und Fragen stehe ich jedoch gerne zur Verfügung.

// Thorsten Kansy, www.dotnetconsulting.eu

// Ideenengeber: https://docs.microsoft.com/de-de/azure/cosmos-db/mongodb-samples

using MongoDB.Bson;
using MongoDB.Driver;
using System.Security.Authentication;

namespace dotnetconsulting.DotnetCore.MongoDB
{
    public class MongoDbContext
    {
        private readonly string _connectionString;
        private readonly string _databasename;
        private readonly string _collectionname;

        public readonly IMongoDatabase MongoDatabase;

        public readonly IMongoClient MongoClient;

        public MongoDbContext(string ConnectionString, string DatabaseName, string CollectionName)
        {
            // Werte speichern
            _connectionString = ConnectionString;
            _databasename = DatabaseName;
            _collectionname = CollectionName;

            // Einstellungen festlegen
            MongoClientSettings settings = MongoClientSettings.FromUrl(
                new MongoUrl(ConnectionString)
            );

            settings.SslSettings =
                new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };

            // Client erzeugen
            MongoClient = new MongoClient(settings);
            
            // Zugriff aus Datenbank
            MongoDatabase = MongoClient.GetDatabase(_databasename);
        }

        public void Reset()
        {
            MongoDatabase.DropCollection(_collectionname);
            MongoClient.DropDatabase(_databasename);
        }

        public IMongoCollection<Person> People
        {
            get
            {
                return MongoDatabase.GetCollection<Person>(_collectionname);
            }
        }

        public IMongoCollection<Car> Cars
        {
            get
            {
                return MongoDatabase.GetCollection<Car>(_collectionname);
            }
        }

        public IMongoCollection<BsonDocument> BsonDocuments
        {
            get
            {
                return MongoDatabase.GetCollection<BsonDocument>(_collectionname);
            }
        }

        public IMongoCollection<DynamicObject> DynamicObject
        {
            get
            {
                return MongoDatabase.GetCollection<DynamicObject>(_collectionname);
            }
        }
    }
}