// Disclaimer
// Dieser Quellcode ist als Vorlage oder als Ideengeber gedacht. Er kann frei und ohne 
// Auflagen oder Einschränkungen verwendet oder verändert werden.
// Jedoch wird keine Garantie übernommen, dass eine Funktionsfähigkeit mit aktuellen und 
// zukünftigen API-Versionen besteht. Der Autor übernimmt daher keine direkte oder indirekte 
// Verantwortung, wenn dieser Code gar nicht oder nur fehlerhaft ausgeführt wird.
// Für Anregungen und Fragen stehe ich jedoch gerne zur Verfügung.

// Thorsten Kansy, www.dotnetconsulting.eu

using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace dotnetconsulting.DotnetCore.MongoDB
{
    class Program
    {
#pragma warning disable IDE0060 // Remove unused parameter
        static void Main(string[] args)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            //const string enviromentName = "cloud";
            const string enviromentName = "emulator";

            // Konfiguration vorbereiten & Einsatz bereit machen
            IConfigurationRoot config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.{enviromentName}.json")
                .AddUserSecrets<Program>()
                .Build();

            // MongoDbContext erzeugen
            MongoDbContext mongoDbContext = new(
                config.GetValue<string>("MongoDB:ConnectionString"),
                config.GetValue<string>("MongoDB:DatabaseName"),
                config.GetValue<string>("MongoDB:Collectionname")
                );
            
            // Demos ausführen
            Demos.Run(mongoDbContext);

            // Da es zu unter Umständen zu "Race Conditions" kommen kann, 
            // d.h. die asynchrone Aufrufe nicht in der benötigten Reihenfolge 
            // fertig werden, hier eine synchrone Alternative:
            // DemosSynchron.Run(mongoDbContext);

            Console.WriteLine("== Warte auf Ende der Async-Calls ==");
            Console.ReadKey();
        } 
    }
}