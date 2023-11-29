// Disclaimer
// Dieser Quellcode ist als Vorlage oder als Ideengeber gedacht. Er kann frei und ohne 
// Auflagen oder Einschränkungen verwendet oder verändert werden.
// Jedoch wird keine Garantie übernommen, dass eine Funktionsfähigkeit mit aktuellen und 
// zukünftigen API-Versionen besteht. Der Autor übernimmt daher keine direkte oder indirekte 
// Verantwortung, wenn dieser Code gar nicht oder nur fehlerhaft ausgeführt wird.
// Für Anregungen und Fragen stehe ich jedoch gerne zur Verfügung.

// Thorsten Kansy, www.dotnetconsulting.eu

using System;
using Microsoft.Azure.Documents.Client;
using System.Linq;

namespace dotnetconsulting.DotnetCore.DocumentDB
{
    public class DocumentDBContext
    {
        private readonly string _endpoint;
        private readonly string _key;
        public readonly string DatabaseId;
        public readonly string CollectionId;

        public readonly DocumentClient Client;

        public DocumentDBContext(string EndEndpoint, string Key, string DatabaseId, string CollectionId)
        {
            // Werte speichern
            _endpoint = EndEndpoint;
            _key = Key;
            this.DatabaseId = DatabaseId;
            this.CollectionId = CollectionId;

            // Connection Policy erzeugen
            ConnectionPolicy connectionPolicy = new()
            {
                // Empfohlene Standards
                ConnectionMode = ConnectionMode.Direct,
                ConnectionProtocol = Protocol.Tcp,
                UseMultipleWriteLocations = true,
            };

            // Bevorzugte Regionen
            // Die Namen stammen aus dem Azure Portal
            //connectionPolicy.PreferredLocations.Add("West Europe");
            //connectionPolicy.PreferredLocations.Add("North Europe");

            // Client erzeugen
            Client = new DocumentClient(new Uri(_endpoint), _key, connectionPolicy);
        }

        private string _databaseSelfLink;
        public string DatabaseSelfLink
        {
            get
            {
                if (_databaseSelfLink == null)
                {
                    // SelfLink abfragen
                    _databaseSelfLink = Client.CreateDatabaseQuery().Where(c => c.Id == DatabaseId).AsEnumerable().FirstOrDefault()?.SelfLink;
        
                    // Database vorhanden?
                    if (_databaseSelfLink == null)
                        throw new ArgumentOutOfRangeException("DatabaseId");
                }

                return _databaseSelfLink;
            }
        }

        private string _CollectionSelfLink;
        public string CollectionSelfLink
        {
            get
            {
                if (_CollectionSelfLink == null)
                {
                    // SelfLink abfragen
                    _CollectionSelfLink = Client.CreateDocumentCollectionQuery(DatabaseSelfLink).Where(c => c.Id == CollectionId).AsEnumerable().FirstOrDefault()?.SelfLink;

                    // Collection vorhanden?
                    if (_CollectionSelfLink == null)
                        throw new ArgumentOutOfRangeException("CollectionId");
                }

                return _CollectionSelfLink;
            }
        }
    }
}