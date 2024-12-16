using System.Numerics;
using Flux.MathAddon;

namespace Flux.Rendering;

public struct Camera
{
    public Angle fov;
    public float aspectRatio;

    public float nearPlane = 0.01f;
    public float farPlane = 100_000.0f;

    public Camera()
    {
    }

    public readonly Matrix4x4 ComputeViewMatrix(Transform transform) =>
    Matrix4x4.CreateLookAt(transform.Position, transform.Position + transform.Forward, transform.Up);

    public readonly Matrix4x4 ComputeProjectionMatrix() =>
        Matrix4x4.CreatePerspectiveFieldOfView(fov.Radians, -aspectRatio, nearPlane, farPlane);
}