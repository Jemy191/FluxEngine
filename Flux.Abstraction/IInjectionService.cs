namespace Flux.Abstraction;
public interface IInjectionService: IAsyncDisposable
{
    T Instanciate<T>();
}
