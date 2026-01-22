namespace Flux.Slang.Test;

public class Tests
{
    [Test]
    public Task Basic()
    {
        var compiler = new SlangCompiler(["TestData"]);

        var shaders = compiler.Compile(new FileInfo("TestData/Simple.slang"));

        return Verify(shaders);
    }
}