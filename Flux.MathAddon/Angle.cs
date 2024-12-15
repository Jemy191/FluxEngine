namespace Flux.MathAddon;

public readonly record struct Angle
{
    public readonly double DoubleRadians;
    public double DoubleDegrees => DoubleRadians * 180 / Math.PI;
    public float Radians => (float)DoubleRadians;
    public float Degrees => (float)DoubleDegrees;

    public Angle(double radians) => DoubleRadians = radians;

    public static Angle FromDegrees(double degrees) => new Angle(Math.PI / 180 * degrees);
    public static Angle operator +(Angle a, Angle b) => new Angle(a.DoubleRadians + b.DoubleRadians);
    public static Angle operator -(Angle a, Angle b) => new Angle(a.DoubleRadians - b.DoubleRadians);
    public static Angle operator *(Angle a, double scalar) => new Angle(a.DoubleRadians * scalar);
    public static Angle operator /(Angle a, double scalar) => new Angle(a.DoubleRadians / scalar);
    public override string ToString() => $"{DoubleDegrees:F2}°";
}