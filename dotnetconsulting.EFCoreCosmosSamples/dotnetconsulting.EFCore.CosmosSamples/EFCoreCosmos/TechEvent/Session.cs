// Disclaimer
// Dieser Quellcode ist als Vorlage oder als Ideengeber gedacht. Er kann frei und ohne 
// Auflagen oder Einschränkungen verwendet oder verändert werden.
// Jedoch wird keine Garantie übernommen, dass eine Funktionsfähigkeit mit aktuellen und 
// zukünftigen API-Versionen besteht. Der Autor übernimmt daher keine direkte oder indirekte 
// Verantwortung, wenn dieser Code gar nicht oder nur fehlerhaft ausgeführt wird.
// Für Anregungen und Fragen stehe ich jedoch gerne zur Verfügung.

// Thorsten Kansy, www.dotnetconsulting.eu
#nullable disable
using Newtonsoft.Json;

namespace dotnetconsulting.EFCore.CosmosSamples;

public class Session
{
    public Guid SessionId { get; set; }

    public string Title { get; set; }

    public string Abstract { get; set; }

    public DifficultyLevel Difficulty { get; set; }

    public int Duration { get; set; }

    public int? EventId { get; set; }

    public Speaker Speaker { get; set; }

    public Guid SpeakerId { get; set; }

    public DateTime Begin { get; set; }

    public DateTime End { get; set; }

    public DateTime Created { get; set; }

    public DateTime? Updated { get; set; }

    public bool IsDeleted { get; set; }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this, Formatting.Indented);
    }
}

public enum DifficultyLevel
{
    Level1 = 1,
    Level2 = 2,
    Level3 = 3,
    Level4 = 4
}