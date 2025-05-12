using System.Diagnostics;
using DotGraphFormatParser;

namespace GraphProject;
public static class Program
{
    private static Graph _graph = new();

    public static int Main()
    {
        int returnCode;

        try
        {
            if (GraphTracer.GetTheLongestPath(_graph, out var foundPathsList, out var _result))
            {
                _result.ForEach(e => Console.Write(_result.Last() == e ? $"{e}" : $"{e} -> "));
            }
            returnCode = 0;
        }
        catch(Exception ex)
        {
            Debug.Print(ex.Message);
            returnCode = -1;
        }
        return returnCode;
    }
}



