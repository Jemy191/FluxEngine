namespace Flux.Rendering.Extensions;

public static class BindableExtensions
{
    public static BindScope ScopeBind(this IBindable bindable)
    {
        bindable.Bind();
        return new BindScope(bindable);
    }
}