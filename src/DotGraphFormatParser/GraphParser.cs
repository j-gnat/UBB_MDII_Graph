using System.Text.RegularExpressions;

namespace DotGraphFormatParser;

public class GraphParser
{
    public static bool LoadGraph(string path, out Graph graph)
    {
        graph = new();

        if(!File.Exists(path)) { return false; }

        StreamReader streamReader = new(path);
        string content = streamReader.ReadToEnd();

        if (!GetBody(content, out var contentBody)) { return false; }

        if (!GetLines(contentBody, out var lines)) { return false; }

        FillGraph(lines, graph);

        return true;
    }

    private static bool GetBody(string content, out string contentBody)
    {
        contentBody = string.Empty;

        //Looking for 5 groups
        //0 -> whole match
        //1st group "  digraph  "
        //2nd group name of the graph (can be empty)
        //3rd group content which starts with { and ends with }
        //4th all the text between brackets <- the part we are looking for
        string pattern = @"^\s*(digraph)(\s*\w*\s*)(\{([\s\S]*)\}\s*$)";
        Regex regex = new(pattern);
        Match match = regex.Match(content);

        if(!match.Success 
            || match.Groups.Count < 5)
        {
            return false;
        }

        Group bodyGroup = match.Groups[4];
        contentBody = bodyGroup.Value;

        return true;
    }

    private static bool GetLines(string contentBody, out List<string> lines)
    {
        lines = [];

        //dividing into lines which:
        // starts with any other characker than white space or ";"
        // doesn't contains ";"
        // ends with ";"
        string pattern = @"[^\s;][^;]*;";
        Regex regex = new(pattern);
        Match match = regex.Match(contentBody);

        while(match.Success)
        {
            lines.Add(match.Value);
            match = match.NextMatch();
        }

        return lines.Count > 0;
    }

    private static bool FillGraph(List<string> lines, Graph graph)
    {
        //looking for string like:
        //a [ any text or empty];
        //or
        //a;
        //brackets are optional
        string nodePattern = @"^\s*(\w+){1}\s*([\[]\s*[\s\S]*[\]]*\s*)*;";

        //looking for string like:
        //a -> b -> c
        string directionsFirstVariantPattern = @"^\s*(\w+\s*[-]+\s*[>]\s*)+(\w+)";

        //looging for string like:
        //a -> {c b d}
        string directionsSecondVariantPattern = @"^(\w+\s*[-]+[>]\s*){1}([{](\s*(\w+)\s*)+[}]){1}";
        
        Regex nodeRegex = new Regex(nodePattern);
        Regex directionFirstVariantRegex = new Regex(directionsFirstVariantPattern);
        Regex directionSecondVariantRegex = new Regex(directionsSecondVariantPattern);

        for (var i = 0; i < lines.Count; i++)
        {
            Match nodeMatch = nodeRegex.Match(lines[i]);
            if(nodeMatch.Success)
            {
                AddNode(nodeMatch.Value, graph.Nodes);
                continue;
            }

            Match directionFirstVariantMatch = directionFirstVariantRegex.Match(lines[i]);
            if(directionFirstVariantMatch.Success)
            {
                AddDirectionsFirstVariant(directionFirstVariantMatch.Value, graph);
                continue;
            }

            Match directionSecondVariantMatch = directionSecondVariantRegex.Match(lines[i]);
            if(directionSecondVariantMatch.Success)
            {
                AddDirectionsSecondVariant(directionSecondVariantMatch.Value, graph);
            }
        }

        return true;
    }

    private static void AddNode(string node, List<string> nodes)
    {
        string pattern = @"\w+";
        Regex regex = new(pattern);
        Match match = regex.Match(node);

        if(match.Success
            && !nodes.Any(n => n == match.Value))
        {
            nodes.Add(match.Value);
        }
    }

    private static void AddDirectionsFirstVariant(string directions, Graph graph)
    {
        string pattern = @"\w+";
        Regex regex = new (pattern);
        string from = string.Empty;
        string to = string.Empty;
        Match match = regex.Match(directions);
        while(match.Success)
        {
            from = to;
            to = match.Value;

            if (!graph.Nodes.Any(n => n == to)) { 
                AddNode(to, graph.Nodes); 
                }

            EnsureListFromExists(from, graph.Directions);
            AddDirection(from, to, graph.Directions);

            match = match.NextMatch();
        }
    }

    private static void AddDirectionsSecondVariant(string directions, Graph graph)
    {
        string pattern = @"\w+";
        Regex regex = new (pattern);
        string from = string.Empty;
        string to = string.Empty;
        Match match = regex.Match(directions);

        from = match.Value;
        match = match.NextMatch();
        
        while(match.Success)
        {
            to = match.Value;

            if (!graph.Nodes.Any(n => n == to)) { 
                AddNode(to, graph.Nodes); 
                }

            EnsureListFromExists(from, graph.Directions);
            AddDirection(from, to, graph.Directions);
            
            match = match.NextMatch();
        }
    }

    private static void EnsureListFromExists(string from, Dictionary<string, List<string>> dict)
    {
        if (!string.IsNullOrEmpty(from) 
            && !dict.Any(d => d.Key == from))
        {
            dict.Add(from, []);
        }
    }

    private static void AddDirection(string from, string to, Dictionary<string, List<string>> dict)
    {
        if (dict.TryGetValue(from, out var dir)
            && !dir.Any(d => d == to))
        {
            dir.Add(to);
        }
    }
}
