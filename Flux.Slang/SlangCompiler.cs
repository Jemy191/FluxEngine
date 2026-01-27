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
    public CompilationResult Compile(FileInfo file, EntryPoint[] entryPoints)
    {
        if (!file.Exists)
            throw new FileNotFoundException(file.FullName);
        try
        {
            var session = GlobalSession.CreateSession(sessionDescription);
            var module = session.LoadModule(file.FullName, out _);

            List<ComponentType> component = [module];
            Dictionary<ShaderStage, int> stagesIndices = [];

            for (var index = 0; index < entryPoints.Length; index++)
            {
                var (stage, name) = entryPoints[index];

                if (stage is not (ShaderStage.Vertex or ShaderStage.Fragment))
                    throw new NotSupportedException($"The stage {stage} is not yet supported.");

                Prowl.Slang.EntryPoint foundEntryPoint;
                try
                {
                    foundEntryPoint = module.FindEntryPointByName(name);
                }
                catch (Exception e)
                {
                    return new EntryPointNotFound(entryPoints[index]);
                }

                if (foundEntryPoint.GetLayout().EntryPoints.Single().Stage != stage)
                    throw new Exception($"The entry point {name} is not a {stage} entry point.");

                component.Add(foundEntryPoint);
                stagesIndices.Add(stage, index);
            }

            // There should be a better way to do this.
            var program = session.CreateCompositeComponentType(component.ToArray(), out _).Link(out _);

            // The target index will always be 0 because we will always compile for one shader type. (Ex: only spirv or only glsl)
            const int targetIndex = 0;
            var compileVertex = program.GetEntryPointCode(stagesIndices[ShaderStage.Vertex], targetIndex, out _);
            var compileFragment = program.GetEntryPointCode(stagesIndices[ShaderStage.Fragment], targetIndex, out _);

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