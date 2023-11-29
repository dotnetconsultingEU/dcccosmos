// Disclaimer
// Dieser Quellcode ist als Vorlage oder als Ideengeber gedacht. Er kann frei und ohne 
// Auflagen oder Einschränkungen verwendet oder verändert werden.
// Jedoch wird keine Garantie übernommen, dass eine Funktionsfähigkeit mit aktuellen und 
// zukünftigen API-Versionen besteht. Der Autor übernimmt daher keine direkte oder indirekte 
// Verantwortung, wenn dieser Code gar nicht oder nur fehlerhaft ausgeführt wird.
// Für Anregungen und Fragen stehe ich jedoch gerne zur Verfügung.

// Thorsten Kansy, www.dotnetconsulting.eu

using Microsoft.Extensions.Logging;

namespace dotnetconsulting.EFCore.CosmosSamples.DemoJobs;

public class DemoJob1(ILogger<DemoJob1> logger, TechEventContext efContext) : IDemoJob
{
#pragma warning disable IDE0052 // Remove unread private members
    private readonly ILogger<DemoJob1> _logger = logger;
#pragma warning restore IDE0052 // Remove unread private members
#pragma warning disable IDE0052 // Remove unread private members
    private readonly TechEventContext _efContext = efContext;

    public string Title => "DemoJob1";

    public void Run()
    {

    }
}