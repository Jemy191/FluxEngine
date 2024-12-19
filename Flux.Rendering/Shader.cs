using System.Numerics;
using Silk.NET.OpenGL;

namespace Flux.Rendering;

public readonly struct Shader : IDisposable
{
    readonly uint handle;
    readonly GL gl;
    readonly Dictionary<string, int> uniformLocations = [];

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

    bool TryGetUniformLocation(string name, out int location)
    {
        if (uniformLocations.TryGetValue(name, out location))
            return true;
        
        location = gl.GetUniformLocation(handle, name);
        uniformLocations[name] = location;
        return location != -1;
    }

    internal void SetUniforms(IEnumerable<Uniform> uniforms)
    {
        foreach (var uniform in uniforms)
        {
            SetUniform(uniform);
        }
    }

    public unsafe void SetUniform<T>(string name, T uniform)
    {
        if (!TryGetUniformLocation(name, out var location))
            return;

        switch (uniform)
        {
            case int intUni: gl.Uniform1(location, intUni); break;
            case float floatUni: gl.Uniform1(location, floatUni); break;
            case Vector2 vector2Uni: gl.Uniform2(location, vector2Uni.X, vector2Uni.Y); break;
            case Vector3 vector3Uni: gl.Uniform3(location, vector3Uni.X, vector3Uni.Y, vector3Uni.Z); break;
            case Matrix4x4 matrix4X4Uni: gl.UniformMatrix4(location, 1, false, (float*)&matrix4X4Uni); break;
        }
    }

    public void SetUniform(Uniform uniform)
    {
        switch (uniform)
        {
            case Uniform<int> intUni: SetUniform(uniform.name, intUni.Value); break;
            case Uniform<float> floatUni: SetUniform(uniform.name, floatUni.Value); break;
            case Uniform<Vector2> vector2Uni: SetUniform(uniform.name, vector2Uni.Value); break;
            case Uniform<Vector3> vector3Uni: SetUniform(uniform.name, vector3Uni.Value); break;
            case Uniform<Matrix4x4> matrix4X4Uni: SetUniform(uniform.name, matrix4X4Uni.Value); break;
        }
    }

    public void Dispose() => gl.DeleteProgram(handle);
}