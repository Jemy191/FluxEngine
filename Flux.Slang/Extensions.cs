using System.Diagnostics.CodeAnalysis;
using Prowl.Slang;

namespace Flux.Slang;

public static class Extensions
{
    extension(DiagnosticInfo info)
    {
        public void ThrowIfMessage()
        {
            if (info.GetException() is { } ex)
                throw ex;
        }
    }

    extension(Module module)
    {
        public IEnumerable<EntryPoint> EnumerateEntryPoints()
        {
            for (var i = 0; i < module.GetDefinedEntryPointCount(); i++)
            {
                yield return module.GetDefinedEntryPoint(i);
            }
        }
    }
}