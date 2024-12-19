namespace Flux.Rendering.GLPrimitives;

public class Uniform<T> : Uniform
{
    public T? Value { get; set; }

    public Uniform(string name, T value) : base(name)
    {
        Value = value;
    }

    public Uniform(string name) : base(name)
    {
        Value = default;
    }
}

public abstract class Uniform
{
    public readonly string name;

    protected Uniform(string name)
    {
        this.name = name;
    }
}