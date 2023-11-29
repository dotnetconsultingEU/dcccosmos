// Disclaimer
// Dieser Quellcode ist als Vorlage oder als Ideengeber gedacht. Er kann frei und ohne 
// Auflagen oder Einschränkungen verwendet oder verändert werden.
// Jedoch wird keine Garantie übernommen, dass eine Funktionsfähigkeit mit aktuellen und 
// zukünftigen API-Versionen besteht. Der Autor übernimmt daher keine direkte oder indirekte 
// Verantwortung, wenn dieser Code gar nicht oder nur fehlerhaft ausgeführt wird.
// Für Anregungen und Fragen stehe ich jedoch gerne zur Verfügung.

// Thorsten Kansy, www.dotnetconsulting.eu
#nullable disable

namespace dotnetconsulting.EFCore.CosmosSamples;

public class Chapter
{
#pragma warning disable IDE1006 // Naming Styles
    public Guid id { get; set; }
#pragma warning restore IDE1006 // Naming Styles

    public int Index { get; set; }

    public string Title { get; set; }

    public string Content { get; set; }
}