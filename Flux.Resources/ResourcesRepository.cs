using Flux.Abstraction;

namespace Flux.Resources;

public class ResourcesRepository
{
    readonly Dictionary<Guid, (IResource? resource, object creationInfo)> registeredResources = [];
    
    // This is a temporary solution to simplify the registration of resources
    // Todo: Remove this.
    readonly Dictionary<object, Guid> guidByResources = [];

    public TResource Get<TResource>(Resource<TResource> id) where TResource : IResource
    {
        if (!registeredResources.TryGetValue(id.Value, out var registeredResource))
            throw new ResourceNotRegisteredException<TResource>(id);

        if (registeredResource.resource is null)
            throw new ResourceNotFoundException<TResource>(id);
        
        return (TResource)registeredResource.resource;
    }

    public Resource<TResource> Register<TResource>(object creationInfo) where TResource : IResource
    {
        if(guidByResources.TryGetValue(creationInfo, out var guid))
            return new Resource<TResource>(guid);
        
        var id = Resource<TResource>.New();
        
        registeredResources.Add(id.Value, (null, creationInfo));
        guidByResources.Add(creationInfo, id.Value);
        return id;
    }
    
    internal void Load<TInfo, TResource>(Resource<TResource> id, FluxResourceManager<TInfo, TResource> resourceManager) where TResource : IResource
    {
        if (!registeredResources.TryGetValue(id.Value, out var registeredResource))
            throw new ResourceNotRegisteredException<TResource>(id);

        if (registeredResource.resource is not null)
            return;
        
        var resource = resourceManager.Load((TInfo)registeredResource.creationInfo, this);
        registeredResources[id.Value] = registeredResource with { resource = resource };
    }

    internal void Unload<TResource>(Resource<TResource> id) where TResource : IResource
    {
        if (!registeredResources.TryGetValue(id.Value, out var registeredResource))
            throw new ResourceNotRegisteredException<TResource>(id);

        if (registeredResource.resource is null)
            throw new ResourceNotFoundException<TResource>(id);
            
        registeredResource.resource.Dispose();
        registeredResources[id.Value] = registeredResource with { resource = null };
    }
}