using System.Numerics;
using DefaultEcs;
using Flux.Asset;
using Flux.Engine.Assets;
using Flux.EntityBehavior;
using Flux.EntityBehavior.Interfaces;
using Flux.MathAddon;
using Flux.Rendering;
using Silk.NET.Input;

namespace BlockGame;

public class BlockControl : Behavior, IUpdatable, IUIDrawable, IAsyncInitializable
{
    readonly ModelEntityBuilderService modelBuilder;
    readonly AssetsService assetsService;

    ShaderAsset blockShader = null!;
    Entity? blockEntity;

    public BlockControl(IInputContext input, ModelEntityBuilderService modelBuilder, AssetsService assetsService)
    {
        this.modelBuilder = modelBuilder;
        this.assetsService = assetsService;

        foreach (var keyboard in input.Keyboards)
        {
            keyboard.KeyDown += KeyDown;
        }
    }

    public async Task InitializeAsync()
    {
        blockShader = (await assetsService.Load<ShaderAsset>(Guid.Parse("5b173006-ab2b-421c-a5f8-9df095e762a6")))!;
    }

    public void Update(float deltatime)
    {
    }
    public void DrawUI(float deltatime)
    {
    }

    void KeyDown(IKeyboard keyboard, Key key, int arg)
    {
        switch (key)
        {
            case Key.F:
                RefreshBlock();
                break;
        }
    }

    void RefreshBlock()
    {
        blockEntity?.Dispose();
        var mesh = CreateBlockMesh();
        blockEntity = modelBuilder
            .Name("Suzane")
            .Shader(blockShader)
            .Mesh(mesh)
            .Position(new Vector3(0, 5, 0))
            .Create();
    }
    static MeshAsset CreateBlockMesh()
    {
        var southNormal = new Vector3(0, 0, -1);
        var southTangent = new Vector3(1, 0, 0);
        var southBinormal = new Vector3(0, 1, 0);

        IReadOnlyList<Vertex> southFace =
        [
            new Vertex
            {
                Position = new Vector3(-1,
                    1,
                    1),
                Normal = southNormal,
                TexCoords = new Vector2(0,
                    1),
                Tangent = southTangent,
                Bitangent = southBinormal,
                Colors = default,
            },
            new Vertex
            {
                Position = new Vector3(1,
                    1,
                    1),
                Normal = southNormal,
                TexCoords = new Vector2(1,
                    1),
                Tangent = southTangent,
                Bitangent = southBinormal,
                Colors = default,
            },
            new Vertex
            {
                Position = new Vector3(1,
                    -1,
                    1),
                Normal = southNormal,
                TexCoords = new Vector2(1,
                    0),
                Tangent = southTangent,
                Bitangent = southBinormal,
                Colors = default,
            },
            new Vertex
            {
                Position = new Vector3(-1,
                    -1,
                    1),
                Normal = southNormal,
                TexCoords = new Vector2(0,
                    0),
                Tangent = southTangent,
                Bitangent = southBinormal,
                Colors = default,
            },
        ];

        List<uint> indices = [];
        List<Vertex> vertices = [];

        foreach (var vertex in southFace)
        {
        }
        
        return new MeshAsset
        {
            Indices = indices,
            Vertices = vertices
        };
    }
}