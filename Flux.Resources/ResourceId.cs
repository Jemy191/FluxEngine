using System.Runtime.InteropServices;
using Flux.Abstraction;
using JetBrains.Annotations;

namespace Flux.Resources;

[StructLayout(LayoutKind.Auto)]
public readonly record struct ResourceId<T> where T : IResource
{
    internal readonly Guid Value;
    [PublicAPI]
    internal ResourceId(Guid value) => Value = value;

    public static ResourceId<T> New() => new ResourceId<T>(Guid.NewGuid());
}