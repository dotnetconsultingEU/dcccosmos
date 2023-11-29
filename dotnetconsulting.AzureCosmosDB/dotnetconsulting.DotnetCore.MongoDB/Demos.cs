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
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace dotnetconsulting.DotnetCore.MongoDB
{
    public static class Demos
    {
        public static async void Run(MongoDbContext MongoDbContext)
        {
            Debugger.Break();

            await Task.Run(() => QueryMongoVersion(MongoDbContext));

            await Task.Run(() => ResetMongoDb(MongoDbContext));
            await Task.Run(() => CreateAndUpdatePerson(MongoDbContext, 99));
            await Task.Run(() => CreateAndUpdateCar(MongoDbContext, 100));
            await Task.Run(() => CreateAndUpdateDynamicEntity(MongoDbContext, 101));

            await Task.Run(() => CreateCrowd(MongoDbContext, 25));
            await Task.Run(() => QueryPersonWithLinq1(MongoDbContext));
            // await Task.Run(() => QueryPersonWithLinq2(MongoDbContext)); // Achtung, nicht lauffähig!

            await Task.Run(() => CreateAndUpdateDynamicEntity(MongoDbContext, 9999));
            await Task.Run(() => DeletePerson(MongoDbContext, 99));

            await Task.Run(() => UpdateUsingBson(MongoDbContext));
            await Task.Run(() => DeleteUsingBson(MongoDbContext));
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

        private static async void CreateAndUpdatePerson(MongoDbContext MongoDbContext, int PersonId)
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
            await MongoDbContext.People.InsertOneAsync(item1);

            // Ersetzen
            item1.Name = "Kron";
            item1.Firstname = "Maria";
            await MongoDbContext.People.ReplaceOneAsync(m => m.Id == PersonId, item1);

            // Finden
            Person item2 = await MongoDbContext.People.Find(m => m.Id == PersonId).FirstAsync();

            Console.WriteLine($"People: {item2.ToJson()}");
        }

        private static async void CreateAndUpdateCar(MongoDbContext MongoDbConext, int CarId)
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
            await MongoDbConext.Cars.InsertOneAsync(item1);

            // Ersetzen
            item1.Brand = "Ford Mustang";
            item1.Color = "Blue";
            await MongoDbConext.Cars.ReplaceOneAsync(m => m.Id == CarId, item1);

            // Finden
            Car item2 = await MongoDbConext.Cars.Find(m => m.Id == CarId).FirstAsync();

            Console.WriteLine($"Cars: {item2.ToJson()}");
        }

        private static async void CreateCrowd(MongoDbContext MongoDbConext, int NumberOfPersons)
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
            await MongoDbConext.People.InsertManyAsync(people);
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

        private static async void CreateAndUpdateDynamicEntity(MongoDbContext MongoDbConext, int Id)
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
            await MongoDbConext.DynamicObject.InsertOneAsync(item1);

            // Ersetzen
            item1.Properties.Brand = "Ford Mustang";
            item1.Properties.Color = "Blue";
            await MongoDbConext.DynamicObject.ReplaceOneAsync(m => m.Id == Id, item1);

            // Finden
            dynamic item2 = await MongoDbConext.DynamicObject.Find(m => m.Id == Id).FirstAsync();

            // item2.ToJson() erzeugt eine Exception, daher: NewtonSoft
            Console.WriteLine($"Dynamic: {JsonConvert.SerializeObject(item2, Formatting.Indented)}");
        }

        private static async void DeletePerson(MongoDbContext MongoDbContext, int PersonId)
        {
            Debugger.Break();

            Console.WriteLine($"DeletePerson({PersonId})");

            // Löschen
            FilterDefinition<Person> filter1 = Builders<Person>.Filter.Eq("id", PersonId.ToString());

            DeleteResult result = await MongoDbContext.People.DeleteOneAsync(filter1);

            Console.WriteLine($"DeleteCount={result.DeletedCount}");
        }

        private static async void UpdateUsingBson(MongoDbContext MongoDbConext)
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
            UpdateResult result = await MongoDbConext.BsonDocuments.UpdateManyAsync(filterAll, update);
            Console.WriteLine($"MatchedCount={result.MatchedCount}, ModifiedCount={result.ModifiedCount}");
        }

        private static async void DeleteUsingBson(MongoDbContext MongoDbConext)
        {
            Debugger.Break();

            Console.WriteLine($"DeleteUsingBson()");

            // Filter erstellen
            FilterDefinition<BsonDocument> filter1 = Builders<BsonDocument>.Filter.Gt("Year", 1990);
            FilterDefinition<BsonDocument> filter2 = Builders<BsonDocument>.Filter.Eq("Brand", "Landrover");
            FilterDefinition<BsonDocument> filterAll = Builders<BsonDocument>.Filter.And(filter1, filter2);

            // Delete Ausführen
            DeleteResult result = await MongoDbConext.BsonDocuments.DeleteManyAsync(filterAll);

            Console.WriteLine($"DeletedCount={result.DeletedCount}");
        }
    }
}
