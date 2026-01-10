namespace Flux.Resources.FileChangeWatchers;

public interface IFileChangeWatcher : IDisposable
{
    void StartWatchingFile(FileInfo file, Action onChanged);
    void StopWatchingFile(FileInfo file);
    /// <summary>
    /// Call once per frame (on the main thread).
    /// Executes callbacks for files that have changed since the previous Flush.
    /// </summary>
    void Flush();
}