using System.Numerics;
using Flux.Engine;
using Silk.NET.OpenGL;

namespace Flux.Rendering;

public readonly struct Shader : IDisposable
{
    readonly uint handle;
    readonly GL gl;

    public Shader(GL gl, IReadOnlyDictionary<ShaderStage, string> stageCodes)
    {
        this.gl = gl;

        if (!stageCodes.ContainsKey(ShaderStage.Vertex))
            throw new Exception("Can't create a shader without a vertex shader");
        if (!stageCodes.ContainsKey(ShaderStage.Fragment))
            throw new Exception("Can't create a shader without a fragment shader");

        if (stageCodes.ContainsKey(ShaderStage.TessellationControl) && !stageCodes.ContainsKey(ShaderStage.TessellationEvaluation))
            Console.WriteLine("Warning: tessellation control shader need an evaluation shader to work.");

        var shaderHandles = new List<uint>();

        foreach (var (stage, code) in stageCodes)
        {
            var shaderType = stage switch
            {

                ShaderStage.Vertex => ShaderType.VertexShader,
                ShaderStage.TessellationControl => ShaderType.TessControlShader,
                ShaderStage.TessellationEvaluation => ShaderType.TessEvaluationShader,
                ShaderStage.Geometry => ShaderType.GeometryShader,
                ShaderStage.Fragment => ShaderType.FragmentShader,
            };

            shaderHandles.Add(SendToGPU(shaderType, code));
        }

        handle = this.gl.CreateProgram();

        foreach (var shaderHandle in shaderHandles)
        {
            this.gl.AttachShader(handle, shaderHandle);
        }
        this.gl.LinkProgram(handle);

        this.gl.GetProgram(handle, GLEnum.LinkStatus, out var status);
        if (status == 0)
            throw new GlException($"Program failed to link with error: {this.gl.GetProgramInfoLog(handle)}");

        foreach (var shaderHandle in shaderHandles)
        {
            this.gl.DetachShader(handle, shaderHandle);
            this.gl.DeleteShader(shaderHandle);
        }
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

    int GetUniformLocation(string name)
    {
        if (TryGetUniformLocation(name, out var location))
            return location;

        throw new GlException($"{name} uniform not found on shader.");
    }

    bool TryGetUniformLocation(string name, out int location)
    {
        location = gl.GetUniformLocation(handle, name);
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

        if (uniform is int intUni)
        {
            gl.Uniform1(location, intUni);
        }
        else if (uniform is float floatUni)
        {
            gl.Uniform1(location, floatUni);
        }
        else if (uniform is Vector2 vector2Uni)
        {
            gl.Uniform2(location, vector2Uni.X, vector2Uni.Y);
        }
        else if (uniform is Vector3 vector3Uni)
        {
            gl.Uniform3(location, vector3Uni.X, vector3Uni.Y, vector3Uni.Z);
        }
        else if (uniform is Matrix4x4 matrix4x4Uni)
        {
            var v = matrix4x4Uni;
            gl.UniformMatrix4(location, 1, false, (float*)&v);
        }
    }

    public void SetUniform(Uniform uniform)
    {
        if (!TryGetUniformLocation(uniform.name, out _))
            return;

        if (uniform is Uniform<int> intUni)
        {
            SetUniform(uniform.name, intUni.value);
        }
        else if (uniform is Uniform<float> floatUni)
        {
            SetUniform(uniform.name, floatUni.value);
        }
        else if (uniform is Uniform<Vector2> vector2Uni)
        {
            SetUniform(uniform.name, vector2Uni.value);
        }
        else if (uniform is Uniform<Vector3> vector3Uni)
        {
            SetUniform(uniform.name, vector3Uni.value);
        }
        else if (uniform is Uniform<Matrix4x4> matrix4x4Uni)
        {
            SetUniform(uniform.name, matrix4x4Uni.value);
        }
    }

    public void Dispose() => gl.DeleteProgram(handle);
}