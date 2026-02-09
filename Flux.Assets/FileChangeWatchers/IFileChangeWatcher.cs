namespace Flux.Assets.FileChangeWatchers;

public interface IFileChangeWatcher : IDisposable
{
    void RegisterFile(FileInfo file, Action onChanged);
    void UnregisterFile(FileInfo file);
    /// <summary>
    /// Executes callbacks for files that have changed since the previous Flush.
    /// </summary>
    /// <remarks>
    /// Call once per frame (on the main thread).
    /// If you plan to refresh a GraphicEngine object, ensure you do it before of after starting rendering.
    /// </remarks>
    void Flush();
}