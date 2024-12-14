using DefaultEcs;
using Flux.Abstraction;
using Flux.EntityBehavior.Interfaces;

namespace Flux.EntityBehavior;

public readonly struct BehaviorComponent : IDisposable, IUIRenderComponent
{
    readonly Entity Entity;
    readonly IInjectionService injectionServices;
    readonly Dictionary<Type, Behavior> behaviors = new Dictionary<Type, Behavior>();

    readonly HashSet<IUpdatable> updatables = [];
    readonly HashSet<IUIDrawable> uIDrawables = [];
    readonly HashSet<IDisposable> disposables = [];

    public BehaviorComponent(Entity entity, IInjectionService injectionServices)
    {
        Entity = entity;
        this.injectionServices = injectionServices;
    }
    public async Task<BehaviorComponent> AddBehavior<T>() where T : Behavior
    {
        var behavior = injectionServices.Instantiate<T>();
        behavior.Attach(Entity);
        return await AddBehavior(behavior);
    }
    public async Task<BehaviorComponent> AddBehavior<T>(T behavior) where T : Behavior
    {
        behaviors.Add(typeof(T), behavior);

        if (behavior is IUpdatable updatable)
            updatables.Add(updatable);

        if (behavior is IUIDrawable uIDrawable)
            uIDrawables.Add(uIDrawable);

        if (behavior is IDisposable disposable)
            disposables.Add(disposable);
        
        if (behavior is IInitializable initializable)
            initializable.Initialize();
        
        if (behavior is IAsyncInitializable asyncInitializable)
            await asyncInitializable.InitializeAsync();

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