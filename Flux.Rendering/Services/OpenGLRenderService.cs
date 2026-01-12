using System.Drawing;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using Flux.Ecs;
using Flux.Rendering.GLPrimitives;
using Flux.Rendering.ResourceManagers;
using Flux.Resources;
using Flux.Resources.ResourceHandles;
using Shader = Flux.Rendering.GLPrimitives.Shader;
using Texture = Flux.Rendering.GLPrimitives.Textures.Texture;


namespace Flux.Rendering.Services;

public partial class OpenGLRenderService : IDisposable
{
    readonly GL gl;
    readonly IWindow window;

    readonly Mesh<SimpleVertex> screenQuadMesh;
    readonly ResourceHandle<Shader> screenPassShader;
    readonly ResourceHandle<Shader> compositePassShader;

    readonly FramebufferObject opaqueFbo;
    readonly FramebufferObject transparentFbo;

    readonly Texture opaqueTexture;
    readonly Texture depthTexture;
    readonly Texture accumulationTexture;
    readonly Texture revealageTexture;

    public OpenGLRenderService(
        GL gl,
        IWindow window,
        ResourcesRepository resourcesRepository,
        IEcsWorldService ecsService)
    {
        this.gl = gl;
        this.window = window;
        window.FramebufferResize += OnFramebufferResize;

        gl.Enable(EnableCap.CullFace);
        gl.CullFace(TriangleFace.Back);
        gl.FrontFace(FrontFaceDirection.CW);

        gl.Enable(EnableCap.Multisample);

        var world = ecsService.World;

        var entity = world.CreateEntity();

        entity.Set("ScreenQuad");
        entity.Set(resourcesRepository);

        // We could directly load the shader with the LoaderService but we could load multiple time shader like onlyPositionVertexShader.
        // LoaderService will probably be removed at a later point anyway
        var screenShaderId = resourcesRepository.Register<Shader, ShaderCreationInfo>(new ShaderCreationInfo(screenVertexShader, screenFragmentShader));
        var compositeShaderId = resourcesRepository.Register<Shader, ShaderCreationInfo>(new ShaderCreationInfo(onlyPositionVertexShader, compositeFragmentShader));
        entity.AddResource(screenShaderId);
        entity.AddResource(compositeShaderId);

        screenPassShader = resourcesRepository.Get(screenShaderId);
        compositePassShader = resourcesRepository.Get(compositeShaderId);

        screenQuadMesh = new Mesh<SimpleVertex>(gl, vertices, indices);

        // Need to regen this on viewport size changeRegister
        var textureSize = window.Size.As<uint>();
        opaqueTexture = new Texture(gl, textureSetting, InternalFormat.Rgba16f, textureSize, PixelFormat.Rgba, PixelType.HalfFloat);
        depthTexture = new Texture(gl, textureSetting, InternalFormat.DepthComponent, textureSize, PixelFormat.DepthComponent, PixelType.Float);

        accumulationTexture = new Texture(gl, textureSetting, InternalFormat.Rgba16f, textureSize, PixelFormat.Rgba, PixelType.HalfFloat);
        revealageTexture = new Texture(gl, textureSetting, InternalFormat.R8, textureSize, PixelFormat.Red, PixelType.Float);

        FramebufferAttachmentSetting[] opaqueAttachements =
        [
            new FramebufferAttachmentSetting(FramebufferAttachment.ColorAttachment0, opaqueTexture),
            new FramebufferAttachmentSetting(FramebufferAttachment.DepthAttachment, depthTexture)
        ];
        FramebufferAttachmentSetting[] transparentAttachements =
        [
            new FramebufferAttachmentSetting(FramebufferAttachment.ColorAttachment0, accumulationTexture),
            new FramebufferAttachmentSetting(FramebufferAttachment.ColorAttachment1, revealageTexture),
            new FramebufferAttachmentSetting(FramebufferAttachment.DepthAttachment, depthTexture)
        ];

        opaqueFbo = new FramebufferObject(gl, opaqueAttachements);
        transparentFbo = new FramebufferObject(gl, transparentAttachements);

        OnFramebufferResize(window.Size);
    }

    public void StartRendering()
    {
        gl.Enable(EnableCap.DepthTest);
        gl.DepthFunc(DepthFunction.Less);
        gl.DepthMask(true);
        gl.Disable(EnableCap.Blend);

        opaqueFbo.Bind();

        gl.ClearColor(Color.CornflowerBlue);
        gl.Clear((uint)(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit));
    }

    public void StartTransparentRendering()
    {
        gl.DepthMask(false);
        gl.Enable(EnableCap.Blend);
        gl.BlendFunc(0, BlendingFactor.One, BlendingFactor.One);
        gl.BlendFunc(1, BlendingFactor.Zero, BlendingFactor.OneMinusSrcColor);
        gl.BlendEquation(BlendEquationModeEXT.FuncAdd);

        transparentFbo.Bind();
        gl.ClearBuffer(BufferKind.Color, 0, 0);
        gl.ClearBuffer(BufferKind.Color, 1, 1);
    }

    public void EndRendering()
    {
        CombineOpaqueWithAlphaPass();

        DrawAllPass();
    }

    void CombineOpaqueWithAlphaPass()
    {
        gl.DepthFunc(DepthFunction.Always);
        gl.Enable(EnableCap.Blend);
        gl.BlendFunc(BlendingFactor.DstAlpha, BlendingFactor.OneMinusSrcAlpha);

        opaqueFbo.Bind();
        compositePassShader.Resource.Use();

        accumulationTexture.Bind();
        revealageTexture.Bind(TextureUnit.Texture1);

        screenQuadMesh.Bind();
        screenQuadMesh.Draw();
    }

    void DrawAllPass()
    {
        gl.Disable(EnableCap.DepthTest);
        gl.DepthMask(true);
        gl.Disable(EnableCap.Blend);

        // Bind the back buffer(0)
        gl.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

        gl.ClearColor(Color.Black);
        gl.Clear((uint)(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit));

        // Final result
        screenPassShader.Resource.Use();
        opaqueTexture.Bind();
        screenQuadMesh.Bind();
        screenQuadMesh.Draw();
    }

    void OnFramebufferResize(Vector2D<int> size) => gl.Viewport(size);

    public void Dispose()
    {
        window.FramebufferResize -= OnFramebufferResize;
        screenQuadMesh.Dispose();

        opaqueTexture.Dispose();
        depthTexture.Dispose();
        accumulationTexture.Dispose();
        revealageTexture.Dispose();

        opaqueFbo.Dispose();
        transparentFbo.Dispose();
    }
}