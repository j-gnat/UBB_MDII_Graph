namespace DotGraphFormatParser;

public class Graph
{
    public Dictionary<string, List<string>> Directions {get; init;} = [];
    public List<string> Nodes { get; init; } = [];
}