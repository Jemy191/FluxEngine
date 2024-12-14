using DefaultEcs;
using Flux.Abstraction;
using Flux.Ecs;

namespace Flux.EntityBehavior;

public class BehaviorService : IDisposable
{
    readonly IInjectionService injectionService;
    private readonly IEcsWorldService ecsService;
    readonly IDisposable behaviorComponentRemovedSub;
    public BehaviorService(IInjectionService injectionService, IEcsWorldService ecsService)
    {
        this.injectionService = injectionService;
        this.ecsService = ecsService;
        behaviorComponentRemovedSub = ecsService.World.SubscribeComponentRemoved<BehaviorComponent>(OnBehaviorComponentRemoved);
    }

    void OnBehaviorComponentRemoved(in Entity _, in BehaviorComponent value) => value.Dispose();
    public BehaviorComponent AddBehaviorToEntity(Entity entity)
    {
        return entity.AddBehavior(injectionService);
    }

    public (Entity entity, BehaviorComponent behavior) CreateBehaviorEntity()
    {
        var entity = ecsService.World.CreateEntity();
        var behavior = entity.AddBehavior(injectionService);
        return (entity, behavior);
    }

    public async Task<Entity> CreateSingleBehaviorEntity<T>() where T : Behavior
    {
        var (entity, behavior) = CreateBehaviorEntity();
        await behavior.AddBehavior<T>();
        return entity;
    }

    public async Task<Entity> CreateSingleBehaviorEntity<T>(T behavior) where T : Behavior
    {
        var (entity, behaviorComponent) = CreateBehaviorEntity();
        await behaviorComponent.AddBehavior(behavior);
        return entity;
    }

    public void Dispose()
    {
        behaviorComponentRemovedSub.Dispose();

        var entities = ecsService.World.GetEntities()
            .With<BehaviorComponent>()
            .AsEnumerable();

        foreach (var entity in entities)
        {
            entity.Get<BehaviorComponent>().Dispose();
        }
    }
}
