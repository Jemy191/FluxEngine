namespace Flux.Resources.FileChangeWatchers;

public class NullFileChangeWatcher : IFileChangeWatcher
{
    public void StartWatchingFile(FileInfo file, Action onChanged) { }
    public void StopWatchingFile(FileInfo file) { }
    public void Flush() { }
    public void Dispose() { }
}