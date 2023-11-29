using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace dotnetconsulting.Dotnet.AzureTable
{
    public class Person: TableEntity
    {
        public readonly int PersonId;

        // Entitäten auf 3 Partionen verteilen
        private const int MaxPartitionsCount = 3;

        public Person(int Id)
        {
            PersonId = Id;

            (string, string) Keys = CreateKeys(Id);
            PartitionKey = Keys.Item1;
            RowKey = Keys.Item2;
        }

        public Person()
        {
        }

        public static (string, string) CreateKeys(int Id)
        {
            return ((Id % MaxPartitionsCount).ToString(), Id.ToString());
        }

        public string Firstname { get; set; }

        public string Name { get; set; }

        public DateTime BirthDay { get; set; }

        public Decimal Weight { get; set; }
    }
}
