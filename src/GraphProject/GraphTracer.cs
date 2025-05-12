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
    /// <param name="foundPathsList">A list of all paths discovered during the search.</param>
    /// <param name="bestPath">The longest path found during the search.</param>
    /// <param name="searchTimeLimitSeconds">The time limit for the search, applied only if at least one path is found.</param>
    /// <returns>True if at least one path is found; otherwise, false.</returns>
    public static bool GetTheLongestPath(Graph graph, out List<List<string>> foundPathsList, out List<string> bestPath, int searchTimeLimitSeconds = 28)
    {
        var stopwatch = Stopwatch.StartNew();
        searchTimeLimitSeconds *= 1000;
        foundPathsList = [];
        bestPath = [];

        if (graph.Nodes is null 
            || graph.Nodes.Count < 2
            || graph.Directions is null
            || graph.Directions.Count < 2)
        {
            return false;
        }

        var lastNode = graph.Nodes.Last();
        var visitedElements = new Stack<string>();

        visitedElements.Push(Settings.StartElementName);
        var elementsToVisit = new Stack<List<string>>();

        BigInteger counter = 0;

        if(graph.Directions.TryGetValue(Settings.StartElementName, out var directions))
        {
            elementsToVisit.Push(directions);
            while(elementsToVisit.Count > 0)
            {
                counter++;
                if(foundPathsList.Count > 0 & stopwatch.ElapsedMilliseconds > searchTimeLimitSeconds)
                {
                    break;
                }

                if (elementsToVisit.First().Count > 0)
                {
                    string newElementToVisit = elementsToVisit.First().Last();
                    visitedElements.Push(newElementToVisit);
                    elementsToVisit.First().RemoveAt(elementsToVisit.First().Count -1);

                    if (!graph.Directions.TryGetValue(newElementToVisit, out var newDirections))
                    {
                        visitedElements.Pop();
                        continue;
                    }

                    if (newDirections.Contains(lastNode))
                    {
                        foundPathsList.Add([ .. visitedElements.Reverse()]);
                        if (visitedElements.Count > bestPath.Count)
                        {
                            bestPath.Clear();
                            bestPath.AddRange(visitedElements.Reverse());
                        }
                    }

                    elementsToVisit.Push([.. newDirections.Where(e => !visitedElements.Contains(e) 
                        && e != lastNode
                        && e != Settings.StartElementName) 
                        ]);
                }
                else
                {
                    elementsToVisit.Pop();
                    visitedElements.Pop();
                }
            }
            bestPath.Add(lastNode);
        }
        Debug.Print(counter.ToString());
        return foundPathsList.Count > 0;
    }
}