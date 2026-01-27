using System.Numerics;
using DefaultEcs;
using DefaultEcs.System;
using Flux.Ecs;
using Flux.MathAddon;
using Flux.Rendering.Extensions;
using Flux.Rendering.GLPrimitives;
using Flux.Resources;
using Flux.Resources.ResourceHandles;

namespace Flux.Rendering.Debugging;

public class DebugDrawRenderSystem : ISystem<float>
{
    readonly Debug debug;
    
    readonly ResourceHandle<Material> debugLineMaterialHandle;
    readonly EntitySet cameraSet;
    
    readonly ModelViewProjectionBuffer mvp;
    readonly UniformBufferObject<Vector4> lineColorUbo;

    public bool IsEnabled { get; set; } = true;

    public DebugDrawRenderSystem(Debug debug,
                                 IEcsWorldService ecsService,
                                 ResourcesRepository resourcesRepository,
                                 DebuggingOptions options,
                                 GL gl)
    {
        this.debug = debug;
        
        var world = ecsService.World;
        var entity = world.CreateEntity();
        
        entity.Set("DebugDrawRenderData");
        
        entity.AddResource(options.DebugLineShaderId);
        entity.AddResource(options.DebugLineMaterialId);
        
        debugLineMaterialHandle = resourcesRepository.Get(options.DebugLineMaterialId);

        cameraSet = world
            .GetEntities()
            .With<Camera>()
            .With<Transform>()
            .AsSet();

        mvp = new ModelViewProjectionBuffer(gl);
        lineColorUbo = new UniformBufferObject<Vector4>(gl, 2);
    }

    public void Update(float state)
    {
        if (cameraSet.Count == 0)
            throw new InvalidOperationException("No camera in the scene.");
        
        var cameraEntity = cameraSet.GetEntities()[0];
        var cameraTransform = cameraEntity.Get<Transform>();
        var camera = cameraEntity.Get<Camera>();

        mvp.SetViewProjection(camera.ComputeViewProjection(cameraTransform));
        mvp.SetModel(new Transform().ModelMatrix);

        var color = debug.Color;

        lineColorUbo.SendData(new Vector4(color.R, color.G, color.B, color.A));

        var debugLineMaterial = debugLineMaterialHandle.Resource;

        debugLineMaterial.Use();
        mvp.Bind();
        lineColorUbo.Bind();
        
        debug.Render();
    }

    public void Dispose()
    {
        mvp.Dispose();
        lineColorUbo.Dispose();
    }
}