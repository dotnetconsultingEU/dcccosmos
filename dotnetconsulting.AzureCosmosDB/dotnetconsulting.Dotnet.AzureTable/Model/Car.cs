// Disclaimer
// Dieser Quellcode ist als Vorlage oder als Ideengeber gedacht. Er kann frei und ohne 
// Auflagen oder Einschränkungen verwendet oder verändert werden.
// Jedoch wird keine Garantie übernommen, dass eine Funktionsfähigkeit mit aktuellen und 
// zukünftigen API-Versionen besteht. Der Autor übernimmt daher keine direkte oder indirekte 
// Verantwortung, wenn dieser Code gar nicht oder nur fehlerhaft ausgeführt wird.
// Für Anregungen und Fragen stehe ich jedoch gerne zur Verfügung.

// Thorsten Kansy, www.dotnetconsulting.eu


using Microsoft.WindowsAzure.Storage.Table;

namespace dotnetconsulting.Dotnet.AzureTable
{
    public class Car : TableEntity
    {
        public readonly int CarId;

        // Entitäten auf 3 Partionen verteilen
        private const int MaxPartitionsCount = 3;

        public Car(int Id)
        {
            CarId = Id;

            (string, string) Keys = CreateKeys(Id);
            PartitionKey = Keys.Item1;
            RowKey = Keys.Item2;
        }

        public static (string , string) CreateKeys(int Id)
        {
            return ((Id % MaxPartitionsCount).ToString(), Id.ToString());
        }

        public string Name { get; set; }

        public string Brand { get; set; }

        public string Color { get; set; }

        public int Year { get; set; }
    }
}
