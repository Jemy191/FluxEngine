using System.Numerics;
using Flux.Asset.Assets;
using Flux.Asset.Interface;
using Flux.MathAddon;
using JetBrains.Annotations;
using SharpGLTF.Schema2;

namespace Flux.Asset.AssetImporters;

public class GltfImporter : IAssetImporter<MeshAsset>
{
    [MustDisposeResource]
    public Task<MeshAsset?> Import(Stream stream)
    {
        var gltf = ModelRoot.ReadGLB(stream);

        List<uint> indices = [];
        List<Vertex> vertices = [];

        foreach (var primitive in gltf.LogicalMeshes.SelectMany(m => m.Primitives))
        {
            indices.AddRange(primitive.IndexAccessor.AsIndicesArray());

            var positions = primitive.VertexAccessors["POSITION"].AsVector3Array();
            var normals = primitive.VertexAccessors["NORMAL"].AsVector3Array();
            var texcoords = primitive.VertexAccessors["TEXCOORD_0"].AsVector2Array();
            var tangentAndBitangent = normals.Select(n => n.CalculateTangentBitangent()).ToList();

            vertices.EnsureCapacity(vertices.Count + positions.Count);

            vertices.AddRange(positions.Select((position, index) => new Vertex
            {
                Position = position,
                Normal = normals[index],
                TexCoords =texcoords[index],
                Tangent =tangentAndBitangent[index].Tangent,
                Bitangent =tangentAndBitangent[index].Bitangent,
                Colors =Vector3.Zero
            }));
        }
        
        return Task.FromResult(new MeshAsset(indices, vertices))!;
    }
}