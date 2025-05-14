using DotGraphFormatParser;

namespace DotGraphFormatParserTest;

public class Tests
{
    private static readonly Dictionary<string, List<string>> _exprectedDirections = new()
    {
        { "0", new List<string> { "1", "2" } },
        { "1", new List<string> { "2" } },
        { "2", new List<string> { "3" } },
        { "3", new List<string> { "4", "1" } },
        { "4", new List<string> { "5", "2" } },
        { "5", new List<string> { "3" } }
    };

    private static readonly string[] testDelegate = ["0", "1", "2", "3", "4", "5"];

    [TestCase("GraphCase1.gv")]
    [TestCase("GraphCase2.gv")]
    [TestCase("GraphCase3.gv")]
    public void IfTheParserCreatesGraphWithCorrectData(string fileName)
    {
        var path = Path.Combine(TestContext.CurrentContext.TestDirectory, "assets", fileName);
        bool result = GraphParser.LoadGraph(path, out var graph);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.True, "Expected to return true");
            Assert.That(graph.Nodes, Has.Count.EqualTo(6), "Expected count is 6");
            var expectedNodes = testDelegate;
            Assert.That(graph.Nodes, Is.EqualTo(expectedNodes), "Expected to have the same nodes with the same order");
            Assert.That(graph.Directions, Has.Count.EqualTo(6), "Expected count is 6");
            foreach(var dir in graph.Directions)
            {
                bool test = _exprectedDirections.TryGetValue(dir.Key, out var directions);
                Assert.That(test, Is.True, $"Direction key {dir.Key} not found in expected directions");
                Assert.That(dir.Value, Is.EqualTo(directions), $"Directions for key {dir.Key} do not match");
            }
        });
    }
}
