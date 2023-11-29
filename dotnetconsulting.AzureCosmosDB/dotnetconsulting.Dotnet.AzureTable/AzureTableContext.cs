// Disclaimer
// Dieser Quellcode ist als Vorlage oder als Ideengeber gedacht. Er kann frei und ohne 
// Auflagen oder Einschränkungen verwendet oder verändert werden.
// Jedoch wird keine Garantie übernommen, dass eine Funktionsfähigkeit mit aktuellen und 
// zukünftigen API-Versionen besteht. Der Autor übernimmt daher keine direkte oder indirekte 
// Verantwortung, wenn dieser Code gar nicht oder nur fehlerhaft ausgeführt wird.
// Für Anregungen und Fragen stehe ich jedoch gerne zur Verfügung.

// Thorsten Kansy, www.dotnetconsulting.eu

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace dotnetconsulting.Dotnet.AzureTable
{
    public class AzureTableContext
    {
        private readonly string _connectionString;
        private readonly string _tableName;
        public readonly CloudStorageAccount StorageAccount;
        public readonly CloudTableClient TableClient;
        public readonly CloudTable Table;

        public AzureTableContext(string ConnectionString, string TableName)
        {
            _connectionString = ConnectionString;
            _tableName = TableName;

            // Storage Account erstellen
            StorageAccount = CloudStorageAccount.Parse(_connectionString);

            // Cloud Table Client erstellen
            TableClient = StorageAccount.CreateCloudTableClient();

            // Cloud Tabelle
            Table = TableClient.GetTableReference(TableName);
        }
    }
}