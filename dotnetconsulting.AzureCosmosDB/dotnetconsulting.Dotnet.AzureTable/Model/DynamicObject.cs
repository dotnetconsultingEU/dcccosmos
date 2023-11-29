// Disclaimer
// Dieser Quellcode ist als Vorlage oder als Ideengeber gedacht. Er kann frei und ohne 
// Auflagen oder Einschränkungen verwendet oder verändert werden.
// Jedoch wird keine Garantie übernommen, dass eine Funktionsfähigkeit mit aktuellen und 
// zukünftigen API-Versionen besteht. Der Autor übernimmt daher keine direkte oder indirekte 
// Verantwortung, wenn dieser Code gar nicht oder nur fehlerhaft ausgeführt wird.
// Für Anregungen und Fragen stehe ich jedoch gerne zur Verfügung.

// Thorsten Kansy, www.dotnetconsulting.eu

using Microsoft.WindowsAzure.Storage.Table;
using System.Dynamic;

namespace dotnetconsulting.Dotnet.AzureTable
{
    public class DynamicObject: TableEntity
    {
        public DynamicObject(string lastName, string firstName)
        {
            Properties = new ExpandoObject();
            PartitionKey = lastName;
            RowKey = firstName;
        }
        public int Id { get; set; }
        public dynamic Properties { get; set; }
    }
}