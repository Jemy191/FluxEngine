using System.Runtime.InteropServices;
using Flux.Abstraction;
using JetBrains.Annotations;

namespace Flux.Resources;

[StructLayout(LayoutKind.Auto)]
public readonly record struct Resource<T> where T : IResource
{
    internal readonly Guid Value;
    [PublicAPI]
    internal Resource(Guid value) => Value = value;

    public static Resource<T> New() => new Resource<T>(Guid.NewGuid());
}