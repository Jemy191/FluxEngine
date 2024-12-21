using System.Runtime.InteropServices;
using Flux.Abstraction;

namespace Flux.Resources;

[StructLayout(LayoutKind.Auto)]
public class ResourceCreationInfo<TInfo, TResource> where TResource : IResource
{
    public readonly Resource<TResource> Id;
    public readonly TInfo Info;
        
    public ResourceCreationInfo(Resource<TResource> id, TInfo info)
    {
        Id = id;
        Info = info;
    }
}