using DotGraphFormatParser;

namespace GraphProject;
public static class Program
{
    private static List<string> _result = [];
    private static List<List<string>> _possibleResults = [];
    private static string _lastNode = string.Empty;
    public static int Main()
    {
        var _graph = new Graph();
        _graph.Nodes.Add("0"); // first element 
        _graph.Nodes.Add("1");
        _graph.Nodes.Add("2");
        _graph.Nodes.Add("3");
        _graph.Nodes.Add("5");
        _graph.Nodes.Add("4"); // last element in the path
        _graph.Directions.Add("0", ["1", "2"]);
        _graph.Directions.Add("1", ["2"]);
        _graph.Directions.Add("2", ["3", "5"]);
        _graph.Directions.Add("3", ["4", "1"]);
        _graph.Directions.Add("4", ["5", "2"]);
        _graph.Directions.Add("5", ["3", "4"]);

        _lastNode = _graph.Nodes.Last(); // last element

        if (_graph.Directions.TryGetValue(Settings.StartElementName, out var directions))
        {
            List<string> currentPath = [Settings.StartElementName];

            foreach (var dir in directions)
            {
                currentPath.Add(dir);
                VisitNode(currentPath, _graph.Directions);
                currentPath.Remove(dir);

            }

            _result.ForEach(e => Console.Write(_result.Last() == e ? $"{e}" : $"{e} -> "));
            return 0;
        }
        return -1;
    }

    private static void VisitNode(List<string> currentPath, Dictionary<string, List<string>> directionsDict)
    {
        if(directionsDict.TryGetValue(currentPath.Last(), out var directions))
        {
            var filter = new List<string>();
            filter.AddRange(currentPath);

            foreach(var dir in directions.Where(d => !filter.Contains(d)))
            {
                currentPath.Add(dir);
                if (dir == _lastNode && currentPath.Count > _result.Count){
                    var newResult = new List<string>();
                    newResult.AddRange(currentPath);
                    _result = newResult;
                    
                    _possibleResults.Clear();
                    _possibleResults.Add(newResult);
                    currentPath.Remove(dir);
                    continue;
                }

                if (dir == _lastNode && currentPath.Count == _result.Count){
                    var newResult = new List<string>();
                    newResult.AddRange(currentPath);
                    _possibleResults.Add(newResult);
                    currentPath.Remove(dir);
                    continue;
                }

                VisitNode(currentPath, directionsDict);
                currentPath.Remove(dir);
            }
        }
    }
}



