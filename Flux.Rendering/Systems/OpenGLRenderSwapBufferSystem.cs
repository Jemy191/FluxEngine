using DefaultEcs.System;
using Flux.Ecs;
using Flux.Rendering.Extensions;
using Flux.Rendering.GLPrimitives;
using Flux.Rendering.ResourceManagers;
using Flux.Resources;

namespace Flux.Rendering.Systems;

public partial class OpenGLRenderSwapBufferSystem : ISystem<float>
{
    readonly FileInfo quadVertexShader = Path.Join("Rendering", "FistPassScreen.vert").ToEngineInternalAsset();
    readonly FileInfo quadFragmentShader = Path.Join("Rendering", "FistPassScreen.frag").ToEngineInternalAsset();

    readonly Mesh<SimpleVertex> quadMesh;
    readonly Material quadMaterial;

    public bool IsEnabled { get; set; } = true;


    public OpenGLRenderSwapBufferSystem(
        GL gl,
        ResourcesRepository resourcesRepository,
        IEcsWorldService ecsService)
    {
        var world = ecsService.World;

        var entity = world.CreateEntity();

        entity.Set("ScreenQuad");
        entity.Set(resourcesRepository);

        var quadShaderId = resourcesRepository.Register<Shader, ShaderCreationInfo>(new ShaderCreationInfo(quadVertexShader, quadFragmentShader));
        entity.AddResource(quadShaderId);

        quadMaterial = new Material(quadShaderId, [], [], resourcesRepository);
        quadMesh = new Mesh<SimpleVertex>(gl, vertices, indices);
    }

    public void Update(float state)
    {
        quadMaterial.Use();
        quadMesh.Bind();
        quadMesh.Draw();
    }

    public void Dispose()
    {
        quadMesh.Dispose();
        quadMaterial.Dispose();
    }
}