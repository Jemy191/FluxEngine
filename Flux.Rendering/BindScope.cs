namespace Flux.Rendering;

public readonly struct BindScope : IDisposable
{
    readonly IBindable bindable;

    public BindScope(IBindable bindable) => this.bindable = bindable;
    public void Dispose() => bindable.Unbind();
}