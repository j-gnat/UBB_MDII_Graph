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
        searchTimeLimitSeconds *= 1000;
        bestPath = [];

        if (graph.Nodes is null 
            || graph.Nodes.Count < 2
            || graph.Directions is null
            || graph.Directions.Count < 2)
        {
            return false;
        }

        var firstNode = graph.Nodes.First();
        var lastNode = graph.Nodes.Last();

        var visitedElements = new HashSet<string>();
        var elementsToVisit = new Stack<HashSet<string>>();

        visitedElements.Add(firstNode);

        if(graph.Directions.TryGetValue(firstNode, out var directions))
        {
            elementsToVisit.Push(new HashSet<string>(directions));

            while(elementsToVisit.Count > 0)
            {
                if (bestPath.Count > 0 & stopwatch.ElapsedMilliseconds > searchTimeLimitSeconds)
                {
                    break;
                }

                if (elementsToVisit.First().Count > 0)
                {
                    string newElementToVisit = elementsToVisit.First().First();
                    visitedElements.Add(newElementToVisit);
                    elementsToVisit.First().Remove(elementsToVisit.First().First());

                    if (visitedElements.Contains(lastNode))
                    {
                        if (visitedElements.Count > bestPath.Count)
                        {
                            bestPath.Clear();
                            bestPath.AddRange(visitedElements);
                        }
                        visitedElements.Remove(visitedElements.Last());
                        continue;
                    }

                    if (!graph.Directions.TryGetValue(newElementToVisit, out var newDirections))
                    {
                        visitedElements.Remove(visitedElements.Last());
                        continue;
                    }

                    elementsToVisit.Push([.. newDirections.Where(e => !visitedElements.Contains(e) 
                        && e != firstNode) 
                        ]);
                }
                else
                {
                    elementsToVisit.Pop();
                    visitedElements.Remove(visitedElements.First());
                }
            }
        }
        return bestPath.Count > 0;
    }
}