namespace GraphProjectTest;

public class Tests
{
    [Test]
    public void TestHandlesEmptyNodesInGraph()
    {
        DotGraphFormatParser.Graph graph = new();

        bool result = GraphProject.GraphTracer.GetTheLongestPath(graph, out var longestPath, out var pathLength);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.False, "Expected GetTheLongestPath to return false for an empty graph.");
            Assert.That(pathLength, Is.Empty, "Expected pathLength to be 0 for an empty graph.");
        });
    }
}
