using Flux.Abstraction;
using Flux.Rendering.ResourceManagers;
using Silk.NET.OpenGL;

namespace Flux.Rendering.GLPrimitives;

public readonly struct Shader : IResource<ShaderCreationInfo>
{
    readonly uint handle;
    readonly GL gl;

    public Shader(GL gl, string vertexSource, string fragmentSource)
    {
        this.gl = gl;

        var vertex = SendToGPU(ShaderType.VertexShader, vertexSource);
        var fragment = SendToGPU(ShaderType.FragmentShader, fragmentSource);

        handle = this.gl.CreateProgram();

        this.gl.AttachShader(handle, vertex);
        this.gl.AttachShader(handle, fragment);
        this.gl.LinkProgram(handle);

        this.gl.GetProgram(handle, GLEnum.LinkStatus, out var status);
        if (status == 0)
            throw new GlException($"Program failed to link with error: {this.gl.GetProgramInfoLog(handle)}");

        this.gl.DetachShader(handle, vertex);
        this.gl.DetachShader(handle, fragment);
        this.gl.DeleteShader(vertex);
        this.gl.DeleteShader(fragment);
    }

    uint SendToGPU(ShaderType type, string src)
    {
        var shaderHandle = gl.CreateShader(type);
        gl.ShaderSource(shaderHandle, src);
        gl.CompileShader(shaderHandle);

        var infoLog = gl.GetShaderInfoLog(shaderHandle);
        if (!string.IsNullOrWhiteSpace(infoLog))
            throw new GlException($"Error compiling shader of type {type}, failed with error {infoLog}");

        return shaderHandle;
    }

    public void Use() => gl.UseProgram(handle);

    public void Dispose() => gl.DeleteProgram(handle);
}