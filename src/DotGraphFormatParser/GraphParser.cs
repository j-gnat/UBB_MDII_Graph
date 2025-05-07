namespace DotGraphFormatParser;

public class GraphParser
{
    //^\s*(digraph|graph)(\s*\w*\s*)(\{([\s\S]*)\}\s*$)
    public static bool LoadGraph(string path, out Graph graph)
    {
        graph = new();
        return true;
    }
}
