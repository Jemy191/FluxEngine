using System.Numerics;
using Flux.Asset;
using Flux.Asset.Interface;
using Flux.Engine.Assets;
using Flux.MathAddon;
using SharpGLTF.Schema2;

namespace Flux.Engine.AssetImporters;

public class GltfImporter : IAssetImporter
{
    public IEnumerable<string> SupportedFileFormats => ["glb"];

    public Task<SourceAsset?> Import(Stream stream, Guid guid, string name, string format)
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
        
        return Task.FromResult((SourceAsset)new MeshAsset
        {
            Indices = indices,
            Vertices = vertices
        })!;
    }
}