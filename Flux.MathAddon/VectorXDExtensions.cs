using System.Numerics;
using Silk.NET.Maths;

namespace Flux.MathAddon
{
    // ReSharper disable once InconsistentNaming
    public static class VectorXDExtensions
    {
        public static Vector2 AsNumerics(this Vector2D<uint> vector) => new Vector2
        (
            vector.X,
            vector.Y
        );

        public static Vector2 AsNumerics(this Vector2D<int> vector) => new Vector2
        (
            vector.X,
            vector.Y
        );
        
        public static Vector2 AsNumerics(this Vector2D<float> vector) => new Vector2
        (
            vector.X,
            vector.Y
        );
        
        public static Vector3 AsNumerics(this Vector3D<uint> vector) => new Vector3
        (
            vector.X,
            vector.Y,
            vector.Z
        );
        
        public static Vector3 AsNumerics(this Vector3D<int> vector) => new Vector3
        (
            vector.X,
            vector.Y,
            vector.Z
        );
        
        public static Vector3 AsNumerics(this Vector3D<float> vector) => new Vector3
        (
            vector.X,
            vector.Y,
            vector.Z
        );
    }
}