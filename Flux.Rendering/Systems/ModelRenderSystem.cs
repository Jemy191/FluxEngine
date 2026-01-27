using System.Numerics;
using DefaultEcs;
using DefaultEcs.System;
using Flux.Ecs;
using Flux.MathAddon;
using Flux.Rendering.Extensions;
using Flux.Rendering.GLPrimitives;
using Silk.NET.Windowing;

namespace Flux.Rendering.Systems;

public class ModelRenderSystem : AEntitySetSystem<float>
{
    readonly EntitySet cameraSet;

    Angle lightYaw = Angle.FromDegrees(90);

    readonly ModelViewProjectionBuffer mvp;
    readonly UniformBufferObject<Vector3> lightDirectionUbo;
    readonly IWindow window;

    public ModelRenderSystem(IEcsWorldService ecsService, IWindow window, GL gl)
        : base(ecsService.World.GetEntities()
            .With<Transform>()
            .With<Model>()
            .AsSet())
    {
        this.window = window;

        cameraSet = ecsService.World
            .GetEntities()
            .With<Camera>()
            .With<Transform>()
            .AsSet();

        mvp = new ModelViewProjectionBuffer(gl);
        lightDirectionUbo = new UniformBufferObject<Vector3>(gl, 2);
    }

    protected override void PreUpdate(float deltatime)
    {
        if (cameraSet.Count == 0)
            throw new  Exception("No camera in the scene.");

        var cameraEntity = cameraSet.GetEntities()[0];
        var camera = cameraEntity.Get<Camera>();
        var cameraTransform = cameraEntity.Get<Transform>();

        mvp.SetViewProjection(camera.ComputeViewProjection(cameraTransform));
        var lightDirection = Quaternion.CreateFromYawPitchRoll(lightYaw.Radians, Angle.FromDegrees(-45).Radians, 0).Forward();
        lightDirectionUbo.SendData(lightDirection);
        mvp.Bind();
        lightDirectionUbo.Bind();
    }

    protected override void Update(float deltatime, in Entity modelEntity)
    {
        var modelTransform = modelEntity.Get<Transform>();
        var model = modelEntity.Get<Model>();

        mvp.SetModel(modelTransform.ModelMatrix);

        model.Draw();
    }

    public override void Dispose()
    {
        base.Dispose();
        cameraSet.Dispose();
        mvp.Dispose();
        lightDirectionUbo.Dispose();
    }
}