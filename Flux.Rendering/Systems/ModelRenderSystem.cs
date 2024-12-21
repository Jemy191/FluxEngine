using System.Numerics;
using DefaultEcs;
using DefaultEcs.Resource;
using DefaultEcs.System;
using Flux.Ecs;
using Flux.MathAddon;
using Flux.Rendering.GLPrimitives;
using Flux.Rendering.RenderGraph;
using Flux.Resources;
using Silk.NET.Windowing;

namespace Flux.Rendering.Systems;

using ManagedTexture = ManagedResource<Resource<Texture>[], Resource<Texture>>;
using ManagedShader = ManagedResource<Resource<Shader>[], Resource<Shader>>;
using ManagedMaterial = ManagedResource<Resource<Material>[], Resource<Material>>;

public class ModelRenderSystem : ISystem<float>
{
    readonly EntitySet cameraSet;

    readonly Uniform<Matrix4x4> viewUniform;
    readonly Uniform<Matrix4x4> projectionUniform;
    readonly Uniform<Vector3> lightDirectionUniform;
    readonly Uniform<float> timeUniform;
    readonly IWindow window;

    readonly EntitySet modelSet;
    readonly EntityMultiMap<ManagedMaterial> materialMap;
    readonly EntityMultiMap<ManagedShader> shaderMap;
    readonly EntityMultiMap<ManagedTexture> textureMap;

    Angle lightYaw = Angle.FromDegrees(90);

    public bool IsEnabled { get; set; }

    public ModelRenderSystem(IEcsWorldService ecsService, IWindow window)
    {
        this.window = window;

        cameraSet = ecsService.World
            .GetEntities()
            .With<Camera>()
            .With<Transform>()
            .AsSet();

        var modelQuery = ecsService.World.GetEntities()
            .With<Transform>()
            .With<Model>();

        modelSet = modelQuery.AsSet();
        
        var materialQuery = modelQuery.With<ManagedMaterial>();
        var materialComparison = EqualityComparer<ManagedMaterial>.Create((m1, m2) => m1.Info == m2.Info);
        materialMap = materialQuery.AsMultiMap(materialComparison);

        var shaderQuery = materialQuery.With<ManagedShader>();
        var shaderComparison = EqualityComparer<ManagedShader>.Create((m1, m2) => m1.Info == m2.Info);
        shaderMap = shaderQuery.AsMultiMap(shaderComparison);

        var textureQuery = materialQuery.With<ManagedTexture>();
        var textureComparison = EqualityComparer<ManagedTexture>.Create((m1, m2) => m1.Info == m2.Info);
        textureMap = textureQuery.AsMultiMap(textureComparison);

        viewUniform = new Uniform<Matrix4x4>("uView");
        projectionUniform = new Uniform<Matrix4x4>("uProjection");
        //viewPosUniform = new("viewPos");
        lightDirectionUniform = new Uniform<Vector3>("lightDirection");
        timeUniform = new Uniform<float>("uTime");
    }

    void PreUpdate(float deltatime)
    {
        if (cameraSet.Count == 0)
        {
            Console.WriteLine("No camera in the scene.");
            return;
        }

        var cameraEntity = cameraSet.GetEntities()[0];
        var camera = cameraEntity.Get<Camera>();
        var cameraTransform = cameraEntity.Get<Transform>();

        //lightYaw += Angle.FromDegrees(20 * deltatime);

        viewUniform.Value = camera.ComputeViewMatrix(cameraTransform);
        projectionUniform.Value = camera.ComputeProjectionMatrix();
        //viewPosUniform.value = cameraTransform.Position;
        lightDirectionUniform.Value = Quaternion.CreateFromYawPitchRoll(lightYaw.Radians, Angle.FromDegrees(-45).Radians, 0).Forward();
        timeUniform.Value = (float)window.Time;
    }

    public void Update(float deltatime)
    {
        if (!IsEnabled)
            return;

        PreUpdate(deltatime);

        BuildRenderGraph();


        var modelTransform = modelEntity.Get<Transform>();
        var model = modelEntity.Get<Model>();

        var uniforms = new Uniform[]
        {
            new Uniform<Matrix4x4>("uModel", modelTransform.ModelMatrix),
            viewUniform,
            projectionUniform,
            //viewPosUniform,
            lightDirectionUniform,
            timeUniform,
        };

        model.Draw(uniforms);
    }

    void BuildRenderGraph()
    {
        var graph = new Graph();

        foreach (var materials in materialMap.Keys)
        {
            foreach (var material in materials.Info)
            {
                foreach (var entity in shaderMap[materials])
                {

                    var shaderGraph = graph.AddShader(material);
                    //shaderGraph
                    //var material = entity.Get<ManagedMaterial>();
                    //var textures = entity.Get<ManagedTexture>();
                    //var model = entity.Get<Model>();
                }
            }

        }
    }

    //void BuildRenderGraph()
    //{
    //    var graph = new Graph();
    //    foreach (var entity in shaderSet.GetEntities())
    //    {
    //        var shaders = entity.Get<ManagedShader>().Info.ToHashSet();
    //        var textures = entity.Get<ManagedTexture>().Info.ToHashSet();
    //        var materials = entity.Get<ManagedMaterial>().Info.ToHashSet();
    //        var model = entity.Get<Model>();
    //        
    //        foreach (var shader in shaders)
    //        {
    //            var shaderGraph = graph.AddShader(shader);
    //            
    //        }
    //    }
    //}

    public void Dispose()
    {
        cameraSet.Dispose();
        modelSet.Dispose();
        materialMap.Dispose();
        shaderMap.Dispose();
        textureMap.Dispose();
    }
}