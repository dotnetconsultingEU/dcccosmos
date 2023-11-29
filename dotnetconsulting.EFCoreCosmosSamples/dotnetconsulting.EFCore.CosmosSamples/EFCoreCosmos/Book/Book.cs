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

public class Book
{
    public Book()
    {
        Author = new Author();
        Chapters = new List<Chapter>();
    }

#pragma warning disable IDE1006 // Naming Styles
    public Guid id { get; set; }
#pragma warning restore IDE1006 // Naming Styles

    public string Title { get; set; }

    public Author Author { get; set; }

    public IList<Chapter> Chapters { get; set; }
}