using System.Numerics;
using System.Runtime.InteropServices;
using Flux.MathAddon;
using Flux.Rendering.DataStruct;
using JetBrains.Annotations;

namespace Flux.Rendering;

[PublicAPI]
[StructLayout(LayoutKind.Auto)]
public struct Camera
{
    public required float NearPlane { get; set; }
    public required float FarPlane { get; set; }
    public required Angle Fov { get; set; }
    public required float AspectRatio { get; set; }

    public readonly Matrix4x4 ComputeViewMatrix(Transform transform) =>
        Matrix4x4.CreateLookAt(transform.Position, transform.Position + transform.Forward, transform.Up);

    public readonly Matrix4x4 ComputeProjectionMatrix() =>
        Matrix4x4.CreatePerspectiveFieldOfView(Fov.Radians, -AspectRatio, NearPlane, FarPlane);

    public readonly ViewProjection ComputeViewProjection(Transform transform) =>
        new ViewProjection(ComputeViewMatrix(transform), ComputeProjectionMatrix());

    public Frustum ComputeFrustrum(Transform transform) =>
        new Frustum(ComputeViewMatrix(transform) * ComputeProjectionMatrix());
}