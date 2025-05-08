using System.Diagnostics;
using DotGraphFormatParser;

namespace GraphProject;
public static class Program
{
    private static Graph _graph = new();
    private static List<string> _result = [];
    private static string _lastNode = string.Empty;
    private static long _counter = 0;
    static Program()
    {
        _graph.Nodes.Add("0"); // first element 
        _graph.Nodes.Add("1");
        _graph.Nodes.Add("2");
        _graph.Nodes.Add("3");
        _graph.Nodes.Add("5");
        _graph.Nodes.Add("6");
        _graph.Nodes.Add("7");
        _graph.Nodes.Add("8");
        _graph.Nodes.Add("9");
        _graph.Nodes.Add("10");
        _graph.Nodes.Add("11");
        _graph.Nodes.Add("12");
        _graph.Nodes.Add("13");
        _graph.Nodes.Add("14");
        _graph.Nodes.Add("15");
        _graph.Nodes.Add("16");
        _graph.Nodes.Add("17");
        _graph.Nodes.Add("18");
        _graph.Nodes.Add("19");
        _graph.Nodes.Add("4"); // last element in the path
        _graph.Directions.Add("0", ["1", "2"]);
        _graph.Directions.Add("1", ["2", "3", "5", "6", "7", "8", "9"]);
        _graph.Directions.Add("2", ["3", "4", "5", "6", "7", "8", "9", "10"]);
        _graph.Directions.Add("3", ["4", "5", "6", "7", "8", "9", "10"]);
        _graph.Directions.Add("4", ["5", "6", "7", "8", "9", "10", "11"]);
        _graph.Directions.Add("5", ["6", "7", "8", "9", "10", "11", "12"]);
        _graph.Directions.Add("6", ["7", "8", "9", "10", "11", "12", "13"]);
        _graph.Directions.Add("7", ["8", "9", "10", "11", "12", "13", "14"]);
        _graph.Directions.Add("8", ["9", "10", "11", "12", "13", "14", "15"]);
        _graph.Directions.Add("9", ["10", "11", "12", "13", "14", "15", "4"]);
        _graph.Directions.Add("10", ["11", "12", "13", "14", "15", "4", "5"]);
        _graph.Directions.Add("11", ["12", "13", "14", "15", "4", "5", "6"]);
        _graph.Directions.Add("12", ["13", "14", "15", "4", "5", "6", "7"]);
        _graph.Directions.Add("13", ["14", "15", "4", "5", "6", "7", "8"]);
        _graph.Directions.Add("14", ["15", "4", "5", "6", "7", "8", "9"]);
        _graph.Directions.Add("15", ["4", "5", "6", "7", "8", "9", "16"]);
        _graph.Directions.Add("16", ["14", "15", "6", "7", "8", "9", "17"]);
        _graph.Directions.Add("17", ["4", "5", "6", "7", "8", "9", "18"]);
        _graph.Directions.Add("18", ["14", "15", "6", "7", "8", "9", "19"]);
        _graph.Directions.Add("19", ["4", "5", "6", "17", "8", "9", "10"]);
    }

    public static int Main()
    {
        var stopwatch = Stopwatch.StartNew();

        _lastNode = _graph.Nodes.Last(); // last element

        int returnCode = 1;

        try
        {
            GetTheLongestPathIterative();

            _result.ForEach(e => Console.Write(_result.Last() == e ? $"{e}" : $"{e} -> "));
            returnCode = 0;
        }
        catch(Exception ex)
        {
            Debug.Print(ex.Message);
            returnCode = -1;
        }
        finally
        {
            stopwatch.Stop();
            Console.WriteLine($"\nExecution Time: {stopwatch.ElapsedMilliseconds} ms");
        }
        return returnCode;
    }

    private static void GetTheLongestPathIterative() 
    {
        var visitedElements = new Stack<string>();
        visitedElements.Push(Settings.StartElementName);
        var elementsToVisit = new Stack<List<string>>();
        long counter = 0;
        if(_graph.Directions.TryGetValue(Settings.StartElementName, out var directions))
        {
            elementsToVisit.Push(directions);
            while(elementsToVisit.Count > 0)
            {
                counter++;
                if (elementsToVisit.First().Count > 0)
                {
                    string newElementToVisit = elementsToVisit.First().Last();
                    visitedElements.Push(newElementToVisit);
                    elementsToVisit.First().RemoveAt(elementsToVisit.First().Count -1);

                    if (!_graph.Directions.TryGetValue(newElementToVisit, out var newDirections))
                    {
                        visitedElements.Pop();
                        continue;
                    }

                    if (newDirections.Contains(_lastNode) & visitedElements.Count > _result.Count)
                    {
                        _result.Clear();
                        _result.AddRange(visitedElements.Reverse());
                    }

                    elementsToVisit.Push([.. newDirections.Where(e => !visitedElements.Contains(e) 
                        && e != _lastNode
                        && e != Settings.StartElementName) 
                        ]);
                }
                else
                {
                    elementsToVisit.Pop();
                    visitedElements.Pop();
                }
            }
            _result.Add(_lastNode);
        }
        Console.WriteLine($"{counter}");
    } 
}



