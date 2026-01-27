using Prowl.Slang;

namespace Flux.Slang.Test;

// The compiler does not like to run in parallel
[NotInParallel]
public class Tests
{
    readonly SlangCompiler compiler = new SlangCompiler(["TestData"]);
    readonly EntryPoint[] entryPoints = [
        new EntryPoint(ShaderStage.Vertex, "vertexMain"),
        new EntryPoint(ShaderStage.Fragment, "fragmentMain")
    ];

    [Test]
    public Task Basic()
    {

        var result = compiler.Compile(new FileInfo("TestData/Simple.slang"), entryPoints);

        return Verify(result);
    }

    [Test]
    public Task CompilationError()
    {
        var result = compiler.Compile(new FileInfo("TestData/SimpleError.slang"), entryPoints);

        return Verify(result);
    }
}