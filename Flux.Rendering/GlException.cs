namespace Flux.Rendering;

class GlException : RendererException
{
    public GlException() { }
    
    public GlException(string message)
        : base(message)
    {
    }

    public GlException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
