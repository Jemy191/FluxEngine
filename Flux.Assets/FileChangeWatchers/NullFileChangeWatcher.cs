namespace Flux.Assets.FileChangeWatchers;

public class NullFileChangeWatcher : IFileChangeWatcher
{
    public void RegisterFile(FileInfo file, Action onChanged) { }
    public void UnregisterFile(FileInfo file) { }
    public void Flush() { }
    public void Dispose() { }
}