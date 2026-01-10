using Flux.Abstraction;

namespace Flux.Engine;

public class GameEngineService
{
    public IGameEngine GameEngine { get; internal set; }
}