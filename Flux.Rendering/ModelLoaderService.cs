using Silk.NET.Assimp;

namespace Flux.Rendering;

public class ModelLoaderService : IDisposable
{
    readonly GL gl;
    readonly Assimp assimp;

    public ModelLoaderService(GL gl)
    {
        this.gl = gl;
        assimp = Assimp.GetApi();
    }

    public unsafe Mesh[] LoadMeshes(FileInfo file, bool loadColor = false)
    {
        var scene = assimp.ImportFile(file.FullName, (uint)PostProcessSteps.Triangulate);

        if (scene == null || scene->MFlags == Assimp.SceneFlagsIncomplete || scene->MRootNode == null)
        {
            var error = assimp.GetErrorStringS();
            throw new Exception(error);
        }

        var meshes = ProcessNode(scene->MRootNode, scene, loadColor);

        return meshes.ToArray();
    }

    unsafe List<Mesh> ProcessNode(Node* node, Scene* scene, bool loadColor)
    {
        var meshes = new List<Mesh>((int)node->MNumMeshes);

        for (var i = 0; i < node->MNumMeshes; i++)
        {
            var mesh = scene->MMeshes[node->MMeshes[i]];
            meshes.Add(ProcessMesh(mesh, loadColor));

        }

        for (var i = 0; i < node->MNumChildren; i++)
        {
            meshes.AddRange(ProcessNode(node->MChildren[i], scene, loadColor));
        }

        return meshes;
    }

    unsafe Mesh ProcessMesh(Silk.NET.Assimp.Mesh* mesh, bool loadColor)
    {
        var verticeSize = mesh->MNumVertices * Mesh.VertexSize;
        if (loadColor)
            verticeSize += 3;
        var vertices = new float[verticeSize];
        var indices = new uint[mesh->MNumFaces * 3]; // We assume that the mesh is triangulated

        for (uint i = 0; i < mesh->MNumVertices; i++)
        {
            var t = i * Mesh.VertexSize;
            vertices[t] = mesh->MVertices[i].X;
            vertices[t + 1] = mesh->MVertices[i].Y;
            vertices[t + 2] = mesh->MVertices[i].Z;

            vertices[t + 3] = mesh->MNormals[i].X;
            vertices[t + 4] = mesh->MNormals[i].Y;
            vertices[t + 5] = mesh->MNormals[i].Z;

            vertices[t + 6] = mesh->MTangents[i].X;
            vertices[t + 7] = mesh->MTangents[i].Y;
            vertices[t + 8] = mesh->MTangents[i].Z;

            vertices[t + 9] = mesh->MBitangents[i].X;
            vertices[t + 10] = mesh->MBitangents[i].Y;
            vertices[t + 11] = mesh->MBitangents[i].Z;

            vertices[t + 12] = mesh->MTextureCoords[0][i].X;
            vertices[t + 13] = mesh->MTextureCoords[0][i].Y;

            if (loadColor)
            {
                vertices[t + 4] = mesh->MColors[0][i].X;
                vertices[t + 15] = mesh->MColors[0][i].Y;
                vertices[t + 16] = mesh->MColors[0][i].Z;
            }
        }

        for (uint i = 0; i < mesh->MNumFaces; i++)
        {
            var face = mesh->MFaces[i];

            for (uint j = 0; j < face.MNumIndices; j++)
                indices[i + j] = face.MIndices[j];
        }

        return new Mesh(gl, vertices, indices, loadColor);
    }

    public void Dispose() => assimp.Dispose();
}