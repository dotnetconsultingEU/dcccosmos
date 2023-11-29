﻿// Disclaimer
// Dieser Quellcode ist als Vorlage oder als Ideengeber gedacht. Er kann frei und ohne 
// Auflagen oder Einschränkungen verwendet oder verändert werden.
// Jedoch wird keine Garantie übernommen, dass eine Funktionsfähigkeit mit aktuellen und 
// zukünftigen API-Versionen besteht. Der Autor übernimmt daher keine direkte oder indirekte 
// Verantwortung, wenn dieser Code gar nicht oder nur fehlerhaft ausgeführt wird.
// Für Anregungen und Fragen stehe ich jedoch gerne zur Verfügung.

// Thorsten Kansy, www.dotnetconsulting.eu

using MongoDB.Bson;
using MongoDB.Bson.IO;
using System;

namespace dotnetconsulting.DotnetCore.MongoDB
{
    public static class MongoExtentions
    {
        public static void DumpToConsole(this BsonDocument bsonDocument, string prefix)
        {
            string buildInfoAsJson = bsonDocument.ToJson(new JsonWriterSettings() { Indent = true });
            Console.WriteLine($"{prefix}: {buildInfoAsJson}");
        }
    }
}
