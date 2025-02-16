using System.Numerics;

namespace Flux.MathAddon
{
    public readonly struct Frustum(Matrix4x4 projectionMatrix)
    {
        public readonly Plane[] Planes =
        [
            // Left plane
            Plane.Normalize(new Plane(
                projectionMatrix.M14 + projectionMatrix.M11,
                projectionMatrix.M24 + projectionMatrix.M21,
                projectionMatrix.M34 + projectionMatrix.M31,
                projectionMatrix.M44 + projectionMatrix.M41)),

            // Right plane
            Plane.Normalize(new Plane(
                projectionMatrix.M14 - projectionMatrix.M11,
                projectionMatrix.M24 - projectionMatrix.M21,
                projectionMatrix.M34 - projectionMatrix.M31,
                projectionMatrix.M44 - projectionMatrix.M41)),

            // Bottom plane
            Plane.Normalize(new Plane(
                projectionMatrix.M14 + projectionMatrix.M12,
                projectionMatrix.M24 + projectionMatrix.M22,
                projectionMatrix.M34 + projectionMatrix.M32,
                projectionMatrix.M44 + projectionMatrix.M42)),

            // Top plane
            Plane.Normalize(new Plane(
                projectionMatrix.M14 - projectionMatrix.M12,
                projectionMatrix.M24 - projectionMatrix.M22,
                projectionMatrix.M34 - projectionMatrix.M32,
                projectionMatrix.M44 - projectionMatrix.M42)),

            // Near plane
            Plane.Normalize(new Plane(
                projectionMatrix.M13,
                projectionMatrix.M23,
                projectionMatrix.M33,
                projectionMatrix.M43)),

            // Far plane
            Plane.Normalize(new Plane(
                projectionMatrix.M14 - projectionMatrix.M13,
                projectionMatrix.M24 - projectionMatrix.M23,
                projectionMatrix.M34 - projectionMatrix.M33,
                projectionMatrix.M44 - projectionMatrix.M43)),
        ];

        public bool IsCubeInFrustum(Vector3 center, float halfSize, Vector3? offset = null)
        {
             center += (offset ?? Vector3.Zero);
            Vector3[] vertices =
            [
                new Vector3(center.X - halfSize, center.Y - halfSize, center.Z - halfSize),
                new Vector3(center.X + halfSize, center.Y - halfSize, center.Z - halfSize),
                new Vector3(center.X - halfSize, center.Y + halfSize, center.Z - halfSize),
                new Vector3(center.X + halfSize, center.Y + halfSize, center.Z - halfSize),
                new Vector3(center.X - halfSize, center.Y - halfSize, center.Z + halfSize),
                new Vector3(center.X + halfSize, center.Y - halfSize, center.Z + halfSize),
                new Vector3(center.X - halfSize, center.Y + halfSize, center.Z + halfSize),
                new Vector3(center.X + halfSize, center.Y + halfSize, center.Z + halfSize),
            ];

            return Planes
                .Select(plane => vertices.All(vertex => !(Plane.DotCoordinate(plane, vertex) >= 0)))
                .All(allOutside => !allOutside);
        }
    }
}