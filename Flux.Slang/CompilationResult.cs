using Dunet;
using Prowl.Slang;

namespace Flux.Slang;

[Union]
public partial record CompilationResult
{
    partial record Success(string VertexSource, string FragmentSource);
    partial record Fail(DiagnosticInfo DiagnosticInfo);
}