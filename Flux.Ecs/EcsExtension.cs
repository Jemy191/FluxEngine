using DefaultEcs.System;
using Flux.Abstraction;
using Microsoft.Extensions.DependencyInjection;

namespace Flux.Ecs;

public static class EcsExtension
{
    public static S InstanciateSystem<T, S>(this IInjectionService injectionService) where S : ISystem<T> => injectionService.Instantiate<S>();
}
