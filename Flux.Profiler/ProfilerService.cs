using Flux.Core;
using StackExchange.Profiling;

namespace Flux.Profiler;

public class ProfilerService : IDisposable
{
    public readonly MiniProfiler Profiler = MiniProfiler.StartNew("Profiler") ?? throw new Exception("Profiler did not start");
    
    public void Dispose()
    {
        Profiler.Stop();
        
        var now = DateTime.Now;
        var file = "ProfilerResult".ToDirectory().ToFile($"Profiling-{now.Day}-{now.Hour}-{now.Minute}-{now.Second}.svg");
        
        file.Directory!.Create();

        Profiler.RenderFlameGraph(file);
    }
}