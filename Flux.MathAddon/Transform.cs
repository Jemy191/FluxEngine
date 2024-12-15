using System.Numerics;
using System.Runtime.InteropServices;

namespace Flux.MathAddon;

[StructLayout(LayoutKind.Sequential)]
public struct Transform
{
    public Vector3 Position;
    public Vector3 Scale = Vector3.One;
    public Quaternion Rotation = Quaternion.Identity;

    public readonly Vector3 Forward => Rotation.Forward();
    public readonly Vector3 Up => Rotation.Up();
    public readonly Vector3 Right => Rotation.Right();

    public readonly Matrix4x4 ModelMatrix => Matrix4x4.Identity
                                   * Matrix4x4.CreateFromQuaternion(Rotation)
                                   * Matrix4x4.CreateScale(Scale)
                                   * Matrix4x4.CreateTranslation(Position);
    
    public Transform()
    {
    }
    
    public override readonly string ToString() => $"Pos: {Position}\nScale: {Scale}\nRotation: {Rotation.QuaternionToEuler()}";
}