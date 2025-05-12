using BenchmarkDotNet.Running;

namespace GraphProjectBenchmark;

public class Program
{
    public static void Main()
    {
        BenchmarkRunner.Run<GraphTracerBenchmark>();
    }
}