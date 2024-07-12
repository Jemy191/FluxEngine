using DefaultEcs;
using Flux.Abstraction;

namespace Flux.EntityBehavior;

public readonly struct BehaviorComponent : IDisposable, IUIRenderComponent
{
    readonly Entity Entity;
    readonly IInjectionService injectionServices;
    readonly Dictionary<Type, Behavior> behaviors = new();

    readonly HashSet<IUpdatable> updatables = new();
    readonly HashSet<IUIDrawable> uIDrawables = new();
    readonly HashSet<IDisposable> disposables = new();

    public BehaviorComponent(Entity entity, IInjectionService injectionServices)
    {
        Entity = entity;
        this.injectionServices = injectionServices;
    }
    public BehaviorComponent AddBehavior<T>() where T : Behavior
    {
        var behavior = injectionServices.Instantiate<T>();
        behavior.Attach(Entity);
        return AddBehavior(behavior);
    }
    public BehaviorComponent AddBehavior<T>(T behavior) where T : Behavior
    {
        behaviors.Add(typeof(T), behavior);

        if (behavior is IUpdatable updatable)
            updatables.Add(updatable);

        if (behavior is IUIDrawable uIDrawable)
            uIDrawables.Add(uIDrawable);

        if (behavior is IDisposable disposable)
            disposables.Add(disposable);

        return this;
    }

    public T? GetBehavior<T>() where T : Behavior
    {
        behaviors.TryGetValue(typeof(T), out var behavior);
        return behavior as T;
    }


    public void Update(float deltatime)
    {
        foreach (var updatable in updatables)
        {
            updatable.Update(deltatime);
        }
    }

    public void RenderUI(float deltatime)
    {
        foreach (var uIDrawable in uIDrawables)
        {
            uIDrawable.DrawUI(deltatime);
        }
    }

    public void Dispose()
    {
        foreach (var disposable in disposables)
        {
            disposable.Dispose();
        }
        behaviors.Clear();
        disposables.Clear();
        updatables.Clear();
    }
}