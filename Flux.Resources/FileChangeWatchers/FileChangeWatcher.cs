using System.Collections.Concurrent;
using Flux.Core;

namespace Flux.Resources.FileChangeWatchers;

public class FileChangeWatcher : IFileChangeWatcher
{
    // This exists because of windows.
    static readonly StringComparer stringComparer = OperatingSystem.IsWindows() ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;

    readonly FileSystemWatcher watcher;
    readonly MainThreadThrower mainThreadThrower;

    readonly ConcurrentDictionary<string, Action> callbacks;
    readonly ConcurrentDictionary<string, byte> changedFilesBuffer;

    public FileChangeWatcher(DirectoryInfo rootDirectory, MainThreadThrower mainThreadThrower)
    {
        this.mainThreadThrower = mainThreadThrower;

        if (!rootDirectory.Exists)
            throw new DirectoryNotFoundException($"Root directory not found: {rootDirectory.FullName}");

        callbacks = new ConcurrentDictionary<string, Action>(stringComparer);
        changedFilesBuffer = new ConcurrentDictionary<string, byte>(stringComparer);

        watcher = new FileSystemWatcher(rootDirectory.FullName)
        {
            IncludeSubdirectories = true,
            NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size,
            EnableRaisingEvents = true,
        };

        watcher.Changed += OnFsChanged;
        watcher.Created += OnFsChanged;
        watcher.Renamed += OnFsRenamed;
    }

    public void RegisterFile(FileInfo file, Action onChanged) => callbacks[file.FullName] = onChanged;

    public void UnregisterFile(FileInfo file)
    {
        var fullPath = file.FullName;

        callbacks.TryRemove(fullPath, out _);
        changedFilesBuffer.TryRemove(fullPath, out _);
    }

    /// <inheritdoc/>
    public void Flush()
    {
        if (changedFilesBuffer.IsEmpty)
            return;

        foreach (var fullPath in changedFilesBuffer.Keys)
        {
            // Remove first so a change during callback re-queues for the next frame.
            changedFilesBuffer.TryRemove(fullPath, out _);

            if (!callbacks.TryGetValue(fullPath, out var callback))
                continue;

            if (!File.Exists(fullPath))
                continue;

            try
            {
                callback.Invoke();
            }
            catch(Exception e)
            {
                mainThreadThrower.EnqueueException(e);
            }
        }
    }

    void OnFsChanged(object sender, FileSystemEventArgs e) => MarkChangedIfWatched(e.FullPath);

    void OnFsRenamed(object sender, RenamedEventArgs e) => MarkChangedIfWatched(e.FullPath);

    void MarkChangedIfWatched(string fullPath)
    {
        if (string.IsNullOrWhiteSpace(fullPath))
            return;

        fullPath = Path.GetFullPath(fullPath);

        if (!callbacks.ContainsKey(fullPath))
            return;

        changedFilesBuffer.TryAdd(fullPath, 0);
    }

    public void Dispose()
    {
        watcher.Changed -= OnFsChanged;
        watcher.Created -= OnFsChanged;
        watcher.Renamed -= OnFsRenamed;

        watcher.Dispose();

        callbacks.Clear();
        changedFilesBuffer.Clear();
    }
}