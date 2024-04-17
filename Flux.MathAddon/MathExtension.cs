using System.Numerics;

namespace Flux.MathAddon;

public static class MathExtension
{
    public static Vector3 QuaternionToEuler(this Quaternion q)
    {
        // Extract the pitch (x), yaw (y), and roll (z) angles from the quaternion

        return new Vector3
        {
            X = (float)Math.Atan2(2.0f * (q.W * q.Y + q.X * q.Z), 1.0f - 2.0f * (q.Y * q.Y + q.Z * q.Z)),
            Y = (float)Math.Asin(2.0f * (q.W * q.X - q.Y * q.Z)),
            Z = (float)Math.Atan2(2.0f * (q.W * q.Z + q.X * q.Y), 1.0f - 2.0f * (q.Z * q.Z + q.Y * q.Y))
        };
    }


    public static Vector3 Forward(this Quaternion rotation) => Vector3.Transform(Vector3.UnitZ, rotation);
    public static Vector3 Up(this Quaternion rotation) => Vector3.Transform(Vector3.UnitY, rotation);
    public static Vector3 Right(this Quaternion rotation) => Vector3.Transform(Vector3.UnitX, rotation);

    public static (Vector3 Tangent, Vector3 Bitangent) CalculateTangentBitangent(this Vector3 normal)
    {
        normal = Vector3.Normalize(normal);

        Vector3 tangent;
        if (Math.Abs(normal.Z) > Math.Abs(normal.X))
        {
            var invLength = 1.0f / (float)Math.Sqrt(normal.Z * normal.Z + normal.Y * normal.Y);
            tangent = new Vector3(0.0f, normal.Z * invLength, -normal.Y * invLength);
        }
        else
        {
            var invLength = 1.0f / (float)Math.Sqrt(normal.X * normal.X + normal.Y * normal.Y);
            tangent = new Vector3(-normal.Y * invLength, normal.X * invLength, 0.0f);
        }

        var bitangent = Vector3.Cross(normal, tangent);

        return (tangent, bitangent);
    }
}
