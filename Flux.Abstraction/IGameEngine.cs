using DefaultEcs.System;

namespace Flux.Abstraction;

public interface IGameEngine
{
    IGameEngine AddRenderSystem<T>() where T : ISystem<float>;
    IGameEngine AddUpdateSystem<T>() where T : ISystem<float>;
    IGameEngine Instantiate<T>();
    void Run();
    void RunWith<T>();
}
