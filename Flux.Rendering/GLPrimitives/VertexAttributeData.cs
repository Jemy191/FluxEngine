using System.Runtime.InteropServices;
using Silk.NET.OpenGL;

namespace Flux.Rendering.GLPrimitives
{
    [StructLayout(LayoutKind.Auto)]
    public readonly struct VertexAttributeData
    {
        public readonly uint Index;
        public readonly int Count;
        public readonly VertexAttribPointerType Type;
        public readonly uint OffSet;
        public readonly uint ConsecutiveElementCount;
        
        public VertexAttributeData(uint index, int count, VertexAttribPointerType type, uint offSet, uint consecutiveElementCount = 1)
        {
            ArgumentOutOfRangeException.ThrowIfZero(consecutiveElementCount);
            
            Index = index;
            Count = count;
            Type = type;
            OffSet = offSet;
            ConsecutiveElementCount = consecutiveElementCount;
        }
    }
}