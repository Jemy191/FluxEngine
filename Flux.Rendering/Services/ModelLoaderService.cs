using System.Numerics;
using Silk.NET.Assimp;
using Mesh = Flux.Rendering.GLPrimitives.Mesh;

namespace Flux.Rendering.Services;

public class ModelLoaderService : IDisposable
{
    readonly GL gl;
    readonly Assimp assimp;

    public ModelLoaderService(GL gl)
    {
        this.gl = gl;
        assimp = Assimp.GetApi();
    }

    public unsafe Mesh[] LoadMeshes(FileInfo file)
    {
        var scene = assimp.ImportFile(file.FullName, (uint)PostProcessSteps.Triangulate);

        if (scene == null || scene->MFlags == Assimp.SceneFlagsIncomplete || scene->MRootNode == null)
        {
            var error = assimp.GetErrorStringS();
            throw new Exception(error);
        }

        var meshes = ProcessNode(scene->MRootNode, scene);

        return meshes.ToArray();
    }

    unsafe List<Mesh> ProcessNode(Node* node, Scene* scene)
    {
        var meshes = new List<Mesh>((int)node->MNumMeshes);

        for (var i = 0; i < node->MNumMeshes; i++)
        {
            var mesh = scene->MMeshes[node->MMeshes[i]];
            meshes.Add(ProcessMesh(mesh));

        }

        for (var i = 0; i < node->MNumChildren; i++)
        {
            meshes.AddRange(ProcessNode(node->MChildren[i], scene));
        }

        return meshes;
    }

    unsafe Mesh ProcessMesh(Silk.NET.Assimp.Mesh* mesh)
    {
        var vertices = new Vertex[14];
        var indices = new uint[mesh->MNumFaces * 3]; // We assume that the mesh is triangulated

        for (uint i = 0; i < mesh->MNumVertices; i++)
        {
            var t = i * 14;

            var texCoords = new Vector2
            {
                X = mesh->MTextureCoords[0][i].X,
                Y = mesh->MTextureCoords[0][i].Y
            };
            
            var vertex = new Vertex
            {
                Position = mesh->MVertices[i],
                Normal = mesh->MNormals[i],
                Tangent = mesh->MTangents[i],
                Bitangent = mesh->MBitangents[i],
                TexCoords = texCoords
            };
            vertices[t] = vertex;
        }

        for (uint i = 0; i < mesh->MNumFaces; i++)
        {
            var face = mesh->MFaces[i];

            for (uint j = 0; j < face.MNumIndices; j++)
                indices[i + j] = face.MIndices[j];
        }

        return new Mesh(gl, vertices, indices);
    }

    public void Dispose() => assimp.Dispose();
}