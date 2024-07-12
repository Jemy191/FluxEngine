namespace Flux.Abstraction;
public interface IInjectionService
{
    T Instantiate<T>();
}
