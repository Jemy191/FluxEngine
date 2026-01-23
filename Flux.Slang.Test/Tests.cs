using Prowl.Slang;

namespace Flux.Slang.Test;

// The compiler does not like to run in parallel
[NotInParallel]
public class Tests
{
    readonly SlangCompiler compiler = new SlangCompiler(["TestData"]);

    [Test]
    public Task Basic()
    {

        var result = compiler.Compile(new FileInfo("TestData/Simple.slang"));

        return Verify(result);
    }

    [Test]
    public Task CompilationError()
    {
        var result = compiler.Compile(new FileInfo("TestData/SimpleError.slang"));

        return Verify(result);
    }
}