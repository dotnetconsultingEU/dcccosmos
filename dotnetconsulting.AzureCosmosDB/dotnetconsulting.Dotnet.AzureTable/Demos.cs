// Disclaimer
// Dieser Quellcode ist als Vorlage oder als Ideengeber gedacht. Er kann frei und ohne 
// Auflagen oder Einschränkungen verwendet oder verändert werden.
// Jedoch wird keine Garantie übernommen, dass eine Funktionsfähigkeit mit aktuellen und 
// zukünftigen API-Versionen besteht. Der Autor übernimmt daher keine direkte oder indirekte 
// Verantwortung, wenn dieser Code gar nicht oder nur fehlerhaft ausgeführt wird.
// Für Anregungen und Fragen stehe ich jedoch gerne zur Verfügung.

// Thorsten Kansy, www.dotnetconsulting.eu

// https://docs.microsoft.com/en-us/azure/cosmos-db/table-storage-how-to-use-dotnet

using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System;

namespace dotnetconsulting.Dotnet.AzureTable
{
    public static class Demos
    {
        public static void Run(AzureTableContext AzureTableContext)
        {
#pragma warning disable CS0219 // Variable is assigned but its value is never used
#pragma warning disable IDE0059 // Unnecessary assignment of a value
            int personId = 1;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
            int carId = 2;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning restore CS0219 // Variable is assigned but its value is never used

            //ResetAzureTables(AzureTableContext);
            //CreatePersonAndUpdate(AzureTableContext, personId);
            //CreateCarAndUpdate(AzureTableContext, carId);
            QueryPersons(AzureTableContext);
            //DeletePerson(AzureTableContext, personId);
        }

        private static void ResetAzureTables(AzureTableContext AzureTableContext)
        {
            Console.WriteLine("ResetAzureTables()");

            AzureTableContext.Table.DeleteIfExists();
            AzureTableContext.Table.CreateIfNotExists();
        }

        private static void CreatePersonAndUpdate(AzureTableContext AzureTableContext, int PersonId)
        {
            Console.WriteLine($"CreatePersonAndUpdate({PersonId})");

            // Entität erzeugen
            Person item = new Person(PersonId)
            {
                Firstname = "Harry",
                Name = "Hirsch",
                BirthDay = DateTime.Today.AddDays(-12302)
            };

            // Anlegen
            TableOperation insertOperation = TableOperation.Insert(item);
#pragma warning disable IDE0059 // Unnecessary assignment of a value
            TableResult result = AzureTableContext.Table.Execute(insertOperation);
#pragma warning restore IDE0059 // Unnecessary assignment of a value

            // Ändern
            item.Name = "Kron";
            TableOperation replaceOperation = TableOperation.Replace(item);
#pragma warning disable IDE0059 // Unnecessary assignment of a value
            result = AzureTableContext.Table.Execute(replaceOperation);
#pragma warning restore IDE0059 // Unnecessary assignment of a value
        }

        private static void CreateCarAndUpdate(AzureTableContext AzureTableContext, int CarId)
        {
            Console.WriteLine($"CreateCarAndUpdate({CarId})");

            // Entität erzeugen
            Car item = new Car(CarId)
            {
                Brand = "Landrover Defender 110",
                Color = "Silver",
                Year = 1991,
                Name = "Monster"
            };

            // Anlegen oder ersetzen
            TableOperation insertOperation = TableOperation.InsertOrReplace(item);
#pragma warning disable IDE0059 // Unnecessary assignment of a value
            TableResult result = AzureTableContext.Table.Execute(insertOperation);
#pragma warning restore IDE0059 // Unnecessary assignment of a value

            // Ändern
            item.Name = "Monstern 2";
            TableOperation replaceOperation = TableOperation.Replace(item);
#pragma warning disable IDE0059 // Unnecessary assignment of a value
            result = AzureTableContext.Table.Execute(replaceOperation);
#pragma warning restore IDE0059 // Unnecessary assignment of a value
        }

        private static void QueryPersons(AzureTableContext AzureTableContext)
        {
            Console.WriteLine("QueryPersons()");

            // Abfrae via Partition-und RowKey durchführen
            int personId = 1;
            (string, string) keys = Person.CreateKeys(personId);
            TableOperation retrieveOperation = TableOperation.Retrieve<Person>(keys.Item1, keys.Item2);
            TableResult result = AzureTableContext.Table.Execute(retrieveOperation);

            Console.WriteLine($"Person: {JsonConvert.SerializeObject(result.Result, Formatting.Indented)}");
        }

        private static void DeletePerson(AzureTableContext AzureTableContext, int PersonId)
        {
            Console.WriteLine($"DeletePerson({PersonId})");

            // Filter?
            Person item = new Person(PersonId)
            {
                Firstname = "Harry",
                Name = "Hirsch",
                BirthDay = DateTime.Today.AddDays(-12302)
            };

            // Löschen
            TableOperation deleteOperation = TableOperation.Delete(item);
            AzureTableContext.Table.Execute(deleteOperation);
        }
    }
}