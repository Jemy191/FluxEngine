namespace Flux.Core;

public static class ReflexionExtensions
{
    public static bool InheritFrom(this Type derivedType, Type baseType) => baseType.IsAssignableFrom(derivedType);
    public static bool InheritFrom<T>(this Type derivedType) => typeof(T).IsAssignableFrom(derivedType);
}