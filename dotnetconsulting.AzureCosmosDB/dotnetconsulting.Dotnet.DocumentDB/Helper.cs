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
using Microsoft.Azure.Documents.Client;

namespace dotnetconsulting.Dotnet.DocumentDB
{
    public static class Helper
    {
        public static (string endPoint, string key, string databaseId, string collectionId) GetConfigurationValues()
        {
            //const string enviromentName = "cloud";
            const string enviromentName = "emulator";

            string endPoint = ConfigurationManager.AppSettings[$"{enviromentName}:endPoint"];
            string key = ConfigurationManager.AppSettings[$"{enviromentName}:key"];

            string databaseId = ConfigurationManager.AppSettings[$"{enviromentName}:databaseId"];
            string collectionId = ConfigurationManager.AppSettings[$"{enviromentName}:collectionId"];

            return (endPoint, key, databaseId, collectionId);
        }

        public static DocumentClient CreateDocumentClient(string endPoint, string key)
        {
            ConnectionPolicy connectionPolicy = new ConnectionPolicy
            {
                // Empfohlene Standards
                ConnectionMode = ConnectionMode.Direct,
                ConnectionProtocol = Protocol.Tcp,
                UseMultipleWriteLocations = true,
            };

            // Bevorzugte Regionen
            // Die Namen stammen aus dem Azure Portal
            connectionPolicy.PreferredLocations.Add("West Europe");
            connectionPolicy.PreferredLocations.Add("North Europe");

            // Client erzeugen
            return new DocumentClient(new Uri(endPoint), key, connectionPolicy);
        }
    }
}