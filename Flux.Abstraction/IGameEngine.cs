using DefaultEcs.System;

namespace Flux.Abstraction;

public interface IGameEngine
{
    IGameEngine AddRenderSystem<T>() where T : ISystem<float>;
    IGameEngine AddUpdateSystem<T>() where T : ISystem<float>;
    T Instantiate<T>();
    void Run();
    void RunWith<T>() where T: IGame;
}
