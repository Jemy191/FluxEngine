using System.Drawing;
using DefaultEcs.System;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;

namespace Flux.Rendering.Systems;

public class OpenGlRenderSystem : ISystem<float>
{
    readonly GL gl;
    readonly IWindow window;

    public bool IsEnabled { get; set; }

    public OpenGlRenderSystem(GL gl, IWindow window)
    {
        this.gl = gl;
        this.window = window;
        window.FramebufferResize += OnFramebufferResize;

        gl.ClearColor(Color.CornflowerBlue);
        gl.Enable(EnableCap.DepthTest);

        gl.Enable(EnableCap.CullFace);
        gl.CullFace(TriangleFace.Back);
        gl.FrontFace(FrontFaceDirection.CW);

        gl.Enable(EnableCap.Multisample);

        OnFramebufferResize(window.Size);
    }

    void OnFramebufferResize(Vector2D<int> size) => gl.Viewport(size);

    public void Update(float state)
    {
        gl.Clear((uint)(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit));
    }

    public void Dispose() => window.FramebufferResize -= OnFramebufferResize;
}