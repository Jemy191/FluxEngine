using Prowl.Slang;
using static System.Text.Encoding;

namespace Flux.Slang;

using static CompilationResult;

public class SlangCompiler
{
    readonly SessionDescription sessionDescription;

    /// <param name="searchPaths">Where the compiler will search for dependencies files.</param>
    public SlangCompiler(string[] searchPaths)
    {
        var glslTargetDescription = new TargetDescription
        {
            Format = CompileTarget.Glsl,
            Profile = GlobalSession.FindProfile("glsl_450"),
        };

        sessionDescription = new SessionDescription
        {
            Targets = [glslTargetDescription],
            SearchPaths = searchPaths,
            DefaultMatrixLayoutMode = MatrixLayoutMode.ColumnMajor
        };
    }

    /// <remarks>
    /// The shader should only have one of each stage.
    /// </remarks>
    /// <exception cref="CompilationException"/>
    public CompilationResult Compile(FileInfo file)
    {
        if(!file.Exists)
            throw new FileNotFoundException(file.FullName);
        try
        {
            var session = GlobalSession.CreateSession(sessionDescription);
            var module = session.LoadModule(file.FullName, out _);

            var entryPointsByStage = module
                .EnumerateEntryPoints()
                .ToDictionary(
                    e => e.GetLayout().EntryPoints.Single().Stage,
                    e => e
                );

            var vertex = entryPointsByStage[ShaderStage.Vertex];
            var fragment = entryPointsByStage[ShaderStage.Fragment];

            // There should be a better way to do this.
            var program = session.CreateCompositeComponentType([module, vertex, fragment], out _).Link(out _);

            var compileVertex = program.GetEntryPointCode(0, 0, out _);
            var compileFragment = program.GetEntryPointCode(1, 0, out _);

            var vertexSource = UTF8.GetString(compileVertex.Span);
            var fragmentSource = UTF8.GetString(compileFragment.Span);

            return new Success(vertexSource, fragmentSource);
        }
        catch (CompilationException e)
        {
            return new Fail(e.Diagnostics);
        }
    }
}