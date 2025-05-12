using BenchmarkDotNet.Attributes;
using DotGraphFormatParser;
using GraphProject;

namespace GraphProjectBenchmark;

[ShortRunJob]
public class GraphTracerBenchmark
{
    private Graph? _graph;
    [Params(10, 15, 20)]
    public int _nodes;

    [Params(90, 210, 380)]
    public int _edges;

    [GlobalSetup]
    public void Setup()
    {
        _graph = GenerateGraph(_nodes, _edges);
    }

    [Benchmark]
    public void BenchmarkGraph()
    {
        GraphTracer.GetTheLongestPath(_graph!, out _, out _);
    }

    private static Graph GenerateGraph(int nodeCount, int edgeCount)
    {
        var graph = new Graph();
        var maxEdgeCount = nodeCount * (nodeCount - 1);
        edgeCount = maxEdgeCount < edgeCount ? maxEdgeCount : edgeCount;

        for (int c = 0; c < nodeCount; c++)
        {
            graph.Nodes.Add($"{c}");
        }

        var random = new Random();
        int i = 0;
        while (i < edgeCount)
        {
            var fromNode = graph.Nodes[random.Next(graph.Nodes.Count)];
            var toNode = graph.Nodes[random.Next(graph.Nodes.Count)];

            if (!graph.Directions.TryGetValue(fromNode, out _))
            {
                graph.Directions[fromNode] = [];
            }

            if (fromNode != toNode 
                && !graph.Directions[fromNode].Contains(toNode))
            {
                graph.Directions[fromNode].Add(toNode);
                i++;
            }
        }

        return graph;
    }
}