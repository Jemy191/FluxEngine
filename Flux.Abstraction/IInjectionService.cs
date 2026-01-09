namespace Flux.Abstraction;
public interface IInjectionService: IAsyncDisposable
{
    T Instantiate<T>();
}