namespace Flux.Rendering;

class RendererException : Exception
{
    public RendererException() { }

    public RendererException(string message)
        : base(message)
    {
    }

    public RendererException(string message, Exception inner)
        : base(message, inner)
    {
    }
}