using System.Drawing;
using DefaultEcs.System;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;

namespace Flux.Rendering;

public class OpenGlRenderSystem : ISystem<float>
{
    readonly GL gl;
    readonly IWindow window;

    public bool IsEnabled { get; set; }

    public OpenGlRenderSystem(GL Gl, IWindow window)
    {
        gl = Gl;
        this.window = window;
        window.FramebufferResize += OnFramebufferResize;

        Gl.ClearColor(Color.CornflowerBlue);
        Gl.Enable(EnableCap.DepthTest);

        Gl.Enable(EnableCap.CullFace);
        Gl.CullFace(TriangleFace.Back);
        Gl.FrontFace(FrontFaceDirection.CW);

        Gl.Enable(EnableCap.Multisample);

        OnFramebufferResize(window.Size);
    }

    void OnFramebufferResize(Vector2D<int> size) => gl.Viewport(size);

    public void Update(float state)
    {
        gl.Clear((uint)(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit));
    }

    public void Dispose() => window.FramebufferResize -= OnFramebufferResize;
}