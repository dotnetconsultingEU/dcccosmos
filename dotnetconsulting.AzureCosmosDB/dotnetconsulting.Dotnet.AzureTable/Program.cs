// Disclaimer
// Dieser Quellcode ist als Vorlage oder als Ideengeber gedacht. Er kann frei und ohne 
// Auflagen oder Einschränkungen verwendet oder verändert werden.
// Jedoch wird keine Garantie übernommen, dass eine Funktionsfähigkeit mit aktuellen und 
// zukünftigen API-Versionen besteht. Der Autor übernimmt daher keine direkte oder indirekte 
// Verantwortung, wenn dieser Code gar nicht oder nur fehlerhaft ausgeführt wird.
// Für Anregungen und Fragen stehe ich jedoch gerne zur Verfügung.

// Thorsten Kansy, www.dotnetconsulting.eu

using System;
using System.Configuration;

namespace dotnetconsulting.Dotnet.AzureTable
{
    /// <summary>
    /// This sample program shows how to use the Azure storage SDK to work with premium tables (created using the Azure Cosmos DB service)
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Run common Table CRUD and query operations using the Azure Cosmos DB endpoints ("premium tables")
        /// </summary>
        /// <param name="args">Command line arguments</param>
#pragma warning disable IDE0060 // Remove unused parameter
        public static void Main(string[] args)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            //const string enviromentName = "cloud";
            const string enviromentName = "emulator";

            string connectionString = ConfigurationManager.AppSettings[$"{enviromentName}:StorageConnectionString"];
            string tableName = ConfigurationManager.AppSettings[$"{enviromentName}:TableName"];

            // AzureTableContext erzeugen
            AzureTableContext AzureTableContext = new AzureTableContext(connectionString, tableName);

            // Demos laufen lassen
            Demos.Run(AzureTableContext);

            Console.WriteLine("== Fertig ==");
            Console.ReadKey();
        }
    }    
}