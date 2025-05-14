using System.Diagnostics;
using System.Numerics;
using DotGraphFormatParser;

namespace GraphProject;

public static class GraphTracer
{
    /// <summary>
    /// Searches for the longest path in the graph between the first and last nodes.
    /// </summary>
    /// <param name="graph">The graph to search within.</param>
    /// <param name="bestPath">The longest path found during the search.</param>
    /// <param name="searchTimeLimitSeconds">The time limit for the search, applied only if at least one path is found.</param>
    /// <returns>True if at least one path is found; otherwise, false.</returns>
    public static bool GetTheLongestPath(Graph graph, out List<string> bestPath, int searchTimeLimitSeconds = 28)
    {
        var stopwatch = Stopwatch.StartNew();
        bestPath = [];

        if (graph.Nodes == null
            || graph.Nodes.Count < 2
            || graph.Directions == null
            || graph.Directions.Count < 2)
        {
            return false;
        }

        var firstNode = graph.Nodes.First();
        var lastNode = graph.Nodes.Last();
        var visited = new HashSet<string>();
        var currentPath = new Stack<string>();
        var localBestPath = new List<string>();

        //This function is inside function only to keep it all as static.
        //It was easier to rework the previous version of the code this way...
        void DFS(string node)
        {
            if (stopwatch.Elapsed.TotalSeconds > searchTimeLimitSeconds)
                return;

            visited.Add(node);
            currentPath.Push(node);

            if (node == lastNode)
            {
                var path = currentPath.Reverse().ToList();

                if (path.Count > localBestPath.Count)
                {
                    localBestPath = new List<string>(path);
                }
            }
            else if (graph.Directions.TryGetValue(node, out var neighbors))
            {
                foreach (var neighbor in neighbors)
                {
                    if (!visited.Contains(neighbor))
                    {
                        DFS(neighbor);
                    }
                }
            }

            currentPath.Pop();
            visited.Remove(node);
        }

        DFS(firstNode);
        bestPath = localBestPath;
        return bestPath.Count > 0;
    }
}