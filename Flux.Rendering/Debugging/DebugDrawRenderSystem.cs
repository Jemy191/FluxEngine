using System.Numerics;
using DefaultEcs;
using DefaultEcs.System;
using Flux.Ecs;
using Flux.MathAddon;
using Flux.Rendering.GLPrimitives;
using Flux.Resources;
using Flux.Resources.ResourceHandles;

namespace Flux.Rendering.Debugging;

public class DebugDrawRenderSystem : ISystem<float>
{
    readonly Debug debug;
    
    readonly ResourceHandle<Material> debugLineMaterialHandle;
    readonly EntitySet cameraSet;
    
    
    readonly Uniform<Matrix4x4> viewUniform;
    readonly Uniform<Matrix4x4> projectionUniform;
    
    readonly Uniform<Vector4> lineColorUniform;
    readonly Uniform<Matrix4x4> lineModelUniform;
    readonly IEnumerable<Uniform> uniforms;

    public bool IsEnabled { get; set; } = true;

    public DebugDrawRenderSystem(Debug debug,
                                 IEcsWorldService ecsService,
                                 ResourcesRepository resourcesRepository,
                                 DebuggingOptions options)
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
        
        viewUniform = new Uniform<Matrix4x4>("uView");
        projectionUniform = new Uniform<Matrix4x4>("uProjection");
        lineModelUniform = new Uniform<Matrix4x4>("uModel");
        

        lineColorUniform = new Uniform<Vector4>("uColor");

        uniforms =
        [
            viewUniform,
            projectionUniform,
            lineModelUniform,
            lineColorUniform,
        ];
    }

    public void Update(float state)
    {
        if (cameraSet.Count == 0)
            throw new InvalidOperationException("No camera in the scene.");
        
        var cameraEntity = cameraSet.GetEntities()[0];
        var cameraTransform = cameraEntity.Get<Transform>();
        var camera = cameraEntity.Get<Camera>();

        viewUniform.Value = camera.ComputeViewMatrix(cameraTransform);
        projectionUniform.Value = camera.ComputeProjectionMatrix();
        lineModelUniform.Value = new Transform().ModelMatrix;
        
        var color = debug.Color;

        lineColorUniform.Value = new Vector4(color.R, color.G, color.B, color.A);

        var debugLineMaterial = debugLineMaterialHandle.Resource;

        debugLineMaterial.Use();
        debugLineMaterial.SetUniforms(uniforms);
        
        debug.Render();
    }

    public void Dispose()
    {
    }
}