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

namespace dotnetconsulting.Gremlin
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
#pragma warning disable IDE0059 // Unnecessary assignment of a value
            IConfigurationRoot config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.{enviromentName}.json")
                .Build();
#pragma warning restore IDE0059 // Unnecessary assignment of a value

            // Stand Mai 2018
            // Noch keine Unterstützung für .NET Core

            Console.WriteLine("== Warte auf Ende der Async-Calls ==");
            Console.ReadKey();
        }
    }
}