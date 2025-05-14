using DotGraphFormatParser;

namespace GraphProject;
public static class Program
{
    private static readonly string s_outputFileName = "output.txt";
    public static int Main(string[] args)
    {
        if (args.Length != 1)
        {
            Console.WriteLine("Expected one argument as input");
            return 1;
        }

        if (args[0] == "-h"
            || args[0] == "-help")
        {
            PrintHelpMessage();
            return 0;
        }

        string path = Path.Combine(args[0]);

        return ProcessTheFile(path) ? 0 : 1;
        
    }

    private static void PrintHelpMessage()
    {
        Console.WriteLine("Usage:");
        Console.WriteLine("  GraphProject <path-to-dot-file>");
        Console.WriteLine();
        Console.WriteLine("Description:");
        Console.WriteLine("  This program expects a single argument: the path to a DOT format file describing a directed graph.");
        Console.WriteLine("  If parsing succeeds, the program generates an output file in the program directory containing the longest path found.");
        Console.WriteLine("  If an error occurs, the program will return exit code 1.");
        Console.WriteLine();
        Console.WriteLine("Press Enter to exit...");
        Console.ReadLine();
    }

    private static bool ProcessTheFile(string path)
    {
        try
        {
            if (!GraphParser.LoadGraph(path, out var graph))
            {
                Console.WriteLine($"Could not parse the file:");
                Console.WriteLine($"{path}");
                Console.ReadLine();
                return false;
            }

            if (!GraphTracer.GetTheLongestPath(graph, out var foundPathsList, out var _result))
            {
                Console.WriteLine("No route found in the graph.");
                Console.ReadLine();
                return false;
            }

            string outputText = string.Empty;
            _result.ForEach(e => outputText += _result.Last() == e ? $"{e}" : $"{e} -> ");
            Console.WriteLine(outputText);
            WriteOutputFile(outputText);
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
        return true;
    }

    private static void WriteOutputFile(string content)
    {
        var outputPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, s_outputFileName);
        using var writer = new StreamWriter(outputPath, false);
        writer.Write(content);
    }
}



