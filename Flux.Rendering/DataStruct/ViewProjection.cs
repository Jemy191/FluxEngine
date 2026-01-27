
using System.Numerics;

namespace Flux.Rendering.DataStruct;

public readonly record struct ViewProjection(Matrix4x4 View, Matrix4x4 Projection);