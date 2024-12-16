using FlameGraphNet.Core;
using StackExchange.Profiling;

namespace Flux.Profiler;

public static class MiniProfilerExtensions
{
    public static void RenderFlameGraph(this MiniProfiler profiler, FileInfo file)
    {
        ArgumentNullException.ThrowIfNull(profiler);

        var root = new SimpleNode
        {
            Content = "Game",
            Metric = (double)profiler.DurationMilliseconds
        };

        AppendChildren(profiler.Root, root);

        new FlameGraph(
                new FlameGraphOptions
                {
                    Title = "Flame Graph",
                    Width = 1600,
                    AutoHeight = true,
                })
            .BuildTo(root, file.FullName);
    }

    private static void AppendChildren(Timing timing, SimpleNode currentNode)
    {
        if (timing.Children is null)
            return;

        foreach (var child in timing.Children)
        {
            var childNode = new SimpleNode
            {
                Content = child.Name,
                Metric = (double)child.DurationMilliseconds.Value
            };

            currentNode.Children.Add(childNode);
            AppendChildren(child, childNode);
        }
    }

    private class SimpleNode : IFlameGraphNode
    {
        public string Content { get; set; }
        public double Metric { get; set; }
        public List<IFlameGraphNode> Children { get; } = [];
    }
}