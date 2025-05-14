using BenchmarkDotNet.Attributes;
using DotGraphFormatParser;
using GraphProject;
using System.Runtime.CompilerServices;

namespace GraphProjectBenchmark;

[ShortRunJob]
public class GraphTracerBenchmark
{

    private Graph? _graph;
    static string WhereAmI([CallerFilePath] string callerFilePath = "") => callerFilePath;

    [Params("test1.gv", "test2.gv")]
    public string? _fileName;

    [GlobalSetup]
    public void Setup()
    {
        string path = Path.Combine(Path.GetDirectoryName(WhereAmI())!, "assets", _fileName!);
        if (File.Exists(path))
        {
            GraphParser.LoadGraph(path, out _graph);
        }
        else
        {
            throw new DirectoryNotFoundException($"Could not locate 'testdata' directory: {path}");
        }
    }

    [Benchmark]
    public void BenchmarkGraph()
    {
        GraphTracer.GetTheLongestPath(_graph!, out var result, 1);
    }
}
