using Flux.Assets.Interfaces;

namespace Flux.Assets;

readonly record struct NoTypeAssetId(Guid Id) : IAssetId;