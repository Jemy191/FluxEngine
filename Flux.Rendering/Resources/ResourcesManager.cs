namespace Flux.Rendering.Resources;

public abstract class ResourcesManager<T>
{
    readonly Dictionary<FileInfo, T> resources = new Dictionary<FileInfo, T>();

    public T Get(FileInfo file)
    {
        if (!resources.TryGetValue(file, out var resource))
        {
            resource = Load(file);
            resources[file] = resource;
        }

        return resource;
    }

    public abstract T Load(FileInfo file);

    internal void Decrement(Resource<T> resource)
    {
    }

    internal void Increment(Resource<T> resource)
    {
    }

    protected static string LoadAssetFile(FileInfo file) => ResourcesService.LoadAssetFile(file);
}
