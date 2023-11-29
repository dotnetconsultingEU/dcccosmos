// Disclaimer
// Dieser Quellcode ist als Vorlage oder als Ideengeber gedacht. Er kann frei und ohne 
// Auflagen oder Einschränkungen verwendet oder verändert werden.
// Jedoch wird keine Garantie übernommen, dass eine Funktionsfähigkeit mit aktuellen und 
// zukünftigen API-Versionen besteht. Der Autor übernimmt daher keine direkte oder indirekte 
// Verantwortung, wenn dieser Code gar nicht oder nur fehlerhaft ausgeführt wird.
// Für Anregungen und Fragen stehe ich jedoch gerne zur Verfügung.

// Thorsten Kansy, www.dotnetconsulting.eu

using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Newtonsoft.Json;

namespace dotnetconsulting.DotnetCore.MongoDB
{
    public static class DemosSynchron
    {
        public static void Run(MongoDbContext MongoDbContext)
        {
            QueryMongoVersion(MongoDbContext);

            ResetMongoDb(MongoDbContext);
            CreateAndUpdatePerson(MongoDbContext, 99);
            CreateAndUpdateCar(MongoDbContext, 100);
            CreateAndUpdateDynamicEntity(MongoDbContext, 101);

            CreateCrowd(MongoDbContext, 25);
            QueryPersonWithLinq1(MongoDbContext);
            // QueryPersonWithLinq2(MongoDbContext); // Achtung, nicht lauffähig!

            CreateAndUpdateDynamicEntity(MongoDbContext, 9999);
            DeletePerson(MongoDbContext, 99);

            UpdateUsingBson(MongoDbContext);
            DeleteUsingBson(MongoDbContext);
            Debugger.Break();
        }

        private static void ResetMongoDb(MongoDbContext MongoDbContext)
        {
            Debugger.Break();

            Console.WriteLine("ResetMongoDb()");

            MongoDbContext.Reset();
        }

        private static void QueryMongoVersion(MongoDbContext MongoDbContext)
        {
            Debugger.Break();

            Console.WriteLine("QueryMongoVersion()");

            // Command zur Abfrage der MongoDb-Version
            BsonDocument buildInfoCommand = new("buildinfo", 1);

            // Ausführen
            BsonDocument buildInfo = MongoDbContext.MongoDatabase.RunCommand<BsonDocument>(buildInfoCommand);

            // Ausgabe
            buildInfo.DumpToConsole("BuildInfo");
        }

        private static void CreateAndUpdatePerson(MongoDbContext MongoDbContext, int PersonId)
        {
            Debugger.Break();

            Console.WriteLine($"CreateAndUpdatePerson({PersonId})");

            // POCO erzeugen
            Person item1 = new()
            {
                Id = PersonId,
                Firstname = "Harry",
                Name = "Hirsch",
                BirthDay = DateTime.Today.AddDays(-12302)
            };

            // Anlegen
            MongoDbContext.People.InsertOne(item1);

            // Ersetzen
            item1.Name = "Kron";
            item1.Firstname = "Maria";
            MongoDbContext.People.ReplaceOne(m => m.Id == PersonId, item1);

            // Finden
            Person item2 = MongoDbContext.People.Find(m => m.Id == PersonId).First();

            Console.WriteLine($"People: {item2.ToJson()}");
        }

        private static void CreateAndUpdateCar(MongoDbContext MongoDbConext, int CarId)
        {
            Debugger.Break();

            Console.WriteLine($"CreateAndUpdateCar({CarId})");

            // Poco anlegen
            Car item1 = new()
            {
                Id = CarId,
                Brand = "Landrover Defender 110",
                Color = "Silver",
                Year = 1991,
                Name = "Monster"
            };

            // Anlegen
            MongoDbConext.Cars.InsertOne(item1);

            // Ersetzen
            item1.Brand = "Ford Mustang";
            item1.Color = "Blue";
            MongoDbConext.Cars.ReplaceOne(m => m.Id == CarId, item1);

            // Finden
            Car item2 = MongoDbConext.Cars.Find(m => m.Id == CarId).First();

            Console.WriteLine($"Cars: {item2.ToJson()}");
        }

        private static void CreateCrowd(MongoDbContext MongoDbConext, int NumberOfPersons)
        {
            Debugger.Break();

            Console.WriteLine($"CreateCrowd({NumberOfPersons})");

            // Liste erzeugen und befüllen
            IList<Person> people = new List<Person>(NumberOfPersons);

            // Elemente erzeugen
            for (int i = 0; i < NumberOfPersons; i++)
            {
                Person item = new()
                {
                    Id = 1000 + i,
                    Firstname = $"Harry Nr. {i + 1}",
                    Name = $"Hirsch #{i + 1}",
                    BirthDay = DateTime.Today.AddDays(-12302 - 10 * i),
                    Weight = 80 + i * 2
                };

                people.Add(item);
            }

            // Und einfügen
            MongoDbConext.People.InsertMany(people);
        }

        private static void QueryPersonWithLinq1(MongoDbContext MongoDbConext)
        {
            Debugger.Break();

            Console.WriteLine($"QueryCrowd()");

            // LINQ-Abfrage definieren
            var result = MongoDbConext.People.AsQueryable()
                                .Where(w => w.Weight > 100)
                                .OrderBy(o => o.Name)
                                .ThenBy(o => o.Firstname);

            // Ausführen und ausgeben
            foreach (Person person in result)
                Console.WriteLine($"Id={person.Id}, Name={person.Name}, Firstname={person.Firstname}");
        }

        private static void QueryPersonWithLinq2(MongoDbContext MongoDbConext)
        {
            Debugger.Break();

            Console.WriteLine($"QueryPersonWithLinq()");

            Debugger.Break();
            // LINQ-Abfrage ausführen
            // Aggregate werden leider nicht unterstützt
            var result = MongoDbConext.People.AsQueryable().Any();

            Console.WriteLine($"result={result}");
        }

        private static void CreateAndUpdateDynamicEntity(MongoDbContext MongoDbConext, int Id)
        {
            Debugger.Break();

            Console.WriteLine($"CreateAndUpdateDynamicEntity({Id})");

            DynamicObject item1 = new()
            {
                Id = Id
            };
            item1.Properties.Description = "Walle walle manche Strecke";
            item1.Properties.Number = 4711;
            item1.Properties.Bool = true;
            item1.Properties.Decimal = 19.8m;
            item1.Properties.Created = DateTime.Now;

            // Anlegen
            MongoDbConext.DynamicObject.InsertOneAsync(item1).Wait();

            // Ersetzen
            item1.Properties.Brand = "Ford Mustang";
            item1.Properties.Color = "Blue";
            MongoDbConext.DynamicObject.ReplaceOne(m => m.Id == Id, item1);

            // Finden
            dynamic item2 = MongoDbConext.DynamicObject.Find(m => m.Id == Id).First();

            // item2.ToJson() erzeugt eine Exception, daher: NewtonSoft
            Console.WriteLine($"Dynamic: {JsonConvert.SerializeObject(item2, Formatting.Indented)}");
        }

        private static void DeletePerson(MongoDbContext MongoDbContext, int PersonId)
        {
            Debugger.Break();

            Console.WriteLine($"DeletePerson({PersonId})");

            // Löschen
            FilterDefinition<Person> filter1 = Builders<Person>.Filter.Eq("id", PersonId.ToString());

            DeleteResult result = MongoDbContext.People.DeleteOne(filter1);

            Console.WriteLine($"DeleteCount={result.DeletedCount}");
        }

        private static void UpdateUsingBson(MongoDbContext MongoDbConext)
        {
            Debugger.Break();

            Console.WriteLine($"UpdateUsingBson()");

            // Filter erstellen
            FilterDefinition<BsonDocument> filter1 = Builders<BsonDocument>.Filter.Eq("Name", "Kron");
            FilterDefinition<BsonDocument> filter2 = Builders<BsonDocument>.Filter.Eq("Name", "Monster");
            FilterDefinition<BsonDocument> filterAll = Builders<BsonDocument>.Filter.Or(filter1, filter2);

            // Update definieren
            UpdateDefinition<BsonDocument> update = Builders<BsonDocument>.Update
                .Set("Firstname", "Mike");

            // Update Ausführen
            UpdateResult result = MongoDbConext.BsonDocuments.UpdateMany(filterAll, update);
            Console.WriteLine($"MatchedCount={result.MatchedCount}, ModifiedCount={result.ModifiedCount}");
        }

        private static void DeleteUsingBson(MongoDbContext MongoDbConext)
        {
            Debugger.Break();

            Console.WriteLine($"DeleteUsingBson()");

            // Filter erstellen
            FilterDefinition<BsonDocument> filter1 = Builders<BsonDocument>.Filter.Gt("Year", 1990);
            FilterDefinition<BsonDocument> filter2 = Builders<BsonDocument>.Filter.Eq("Brand", "Landrover");
            FilterDefinition<BsonDocument> filterAll = Builders<BsonDocument>.Filter.And(filter1, filter2);

            // Delete Ausführen
            DeleteResult result = MongoDbConext.BsonDocuments.DeleteMany(filterAll);

            Console.WriteLine($"DeletedCount={result.DeletedCount}");
        }
    }
}