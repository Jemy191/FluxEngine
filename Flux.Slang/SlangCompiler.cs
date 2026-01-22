using Prowl.Slang;
using static System.Text.Encoding;

namespace Flux.Slang;

public class SlangCompiler
{
    readonly SessionDescription sessionDescription;
    /// <param name="searchPaths">Where the compiler will search for dependencies files.</param>
    public SlangCompiler(string[] searchPaths)
    {
        var glslTargetDescription = new TargetDescription
        {
            Format = CompileTarget.Glsl,
            Profile = GlobalSession.FindProfile("glsl_450")
        };

        sessionDescription = new SessionDescription
        {
            Targets = [glslTargetDescription],
            SearchPaths = searchPaths
        };
    }

    /// <remarks>
    /// The shader should only have one of each stage.
    /// </remarks>
    /// <exception cref="CompilationException"/>
    public (string vertex, string fragment) Compile(FileInfo file)
    {
        var session = GlobalSession.CreateSession(sessionDescription);

        var module = session.LoadModule(file.FullName, out var diagnosticInfo);

        diagnosticInfo.ThrowIfMessage();

        var entryPointsByStage = module.EnumerateEntryPoints()
            .ToDictionary(
                e => e.GetLayout().EntryPoints.Single().Stage,
                e => e
            );

        var vertex = entryPointsByStage[ShaderStage.Vertex];
        var fragment = entryPointsByStage[ShaderStage.Fragment];


        var program = session.CreateCompositeComponentType([module, vertex, fragment], out diagnosticInfo);
        diagnosticInfo.ThrowIfMessage();

        var compileVertex = program.GetEntryPointCode(0, 0, out diagnosticInfo);
        diagnosticInfo.ThrowIfMessage();

        var compileFragment = program.GetEntryPointCode(1, 0, out diagnosticInfo);

        var vertexSource = UTF8.GetString(compileVertex.Span);
        var fragmentSource = UTF8.GetString(compileFragment.Span);

        return (vertexSource, fragmentSource);
    }
}