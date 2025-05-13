using DotGraphFormatParser;
using System.Diagnostics;

namespace GraphProject;
public static class Program
{
    public static int Main()
    {
        int returnCode = 0;
        try
        {
            string path = Path.Combine(Settings.DotFileDirectory, Settings.DotFileNames[0]);
            if (!GraphParser.LoadGraph(path, out var graph))
            {
                returnCode = 1;
                return returnCode;
            }

            if (GraphTracer.GetTheLongestPath(graph, out var foundPathsList, out var _result))
            {
                _result.ForEach(e => Console.Write(_result.Last() == e ? $"{e}" : $"{e} -> "));
            }
            returnCode = 0;
        }
        catch(Exception ex)
        {
            Debug.Print(ex.Message);
            returnCode = 1;
        }

        return returnCode;
        
    }
}



