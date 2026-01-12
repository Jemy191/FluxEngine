using Flux.Abstraction;
using Flux.Resources.ResourceHandles;

namespace Flux.Resources;

public class ResourcesRepository
{
    readonly Dictionary<Guid, (IResourceHandleInternal? resource, object creationInfo)> registeredResources = [];
    
    // This is a temporary solution to simplify the registration of resources
    // Todo: Remove this.
    readonly Dictionary<object, Guid> guidByResourcesCreationInfo = [];

    public ResourceHandle<TResource> Get<TResource>(ResourceId<TResource> id) where TResource : IResource
    {
        if (!registeredResources.TryGetValue(id.Value, out var registeredResource))
            throw new ResourceNotRegisteredException<TResource>(id);

        if (registeredResource.resource is null)
            throw new ResourceNotFoundException<TResource>(id);
        
        return (ResourceHandle<TResource>)registeredResource.resource;
    }

    public ResourceId<TResource> Register<TResource, TInfo>(TInfo creationInfo) where TResource : IResource<TInfo>
    {
        if(guidByResourcesCreationInfo.TryGetValue(creationInfo, out var guid))
            return new ResourceId<TResource>(guid);
        
        var id = ResourceId<TResource>.New();
        
        registeredResources.Add(id.Value, (null, creationInfo));
        guidByResourcesCreationInfo.Add(creationInfo, id.Value);
        return id;
    }
    
    internal void Load<TInfo, TResource>(ResourceId<TResource> id, FluxResourceManager<TInfo, TResource> resourceManager) where TResource : IResource<TInfo>
    {
        if (!registeredResources.TryGetValue(id.Value, out var registeredResource))
            throw new ResourceNotRegisteredException<TResource>(id);

        if (registeredResource.resource is not null)
            return;
        
        var resource = resourceManager.Load((TInfo)registeredResource.creationInfo, this);
        registeredResources[id.Value] = registeredResource with { resource = resource };
    }

    internal void Unload<TInfo, TResource>(ResourceId<TResource> id, FluxResourceManager<TInfo, TResource> resourceManager) where TResource : IResource<TInfo>
    {
        if (!registeredResources.TryGetValue(id.Value, out var registeredResource))
            throw new ResourceNotRegisteredException<TResource>(id);

        if (registeredResource.resource is null)
            throw new ResourceNotFoundException<TResource>(id);

        resourceManager.Unload((TInfo)registeredResource.creationInfo, (ResourceHandle<TResource>)registeredResource.resource);

        registeredResource.resource.Dispose();
        registeredResources[id.Value] = registeredResource with { resource = null };
    }
}