namespace Flux.Abstraction;

/// <typeparam name="TIngo">
/// The CreationInfo of the resource.
/// It the same type that go in the <see cref="IFluxResourceManager"/>.
/// </typeparam>
public interface IResource<TIngo> : IResource;

/// <summary>
/// Use <see cref="IResource{TIngo}"/> instead.
/// </summary>
/// <remarks> This is required because you C# can't do
///     <code>
///         ResourceId&lt;T&gt; where T : IResource&lt;T'1&gt;
///     </code>
/// </remarks>
public interface IResource : IDisposable;