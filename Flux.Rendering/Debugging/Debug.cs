using System.Drawing;
using System.Numerics;
using Flux.Rendering.GLPrimitives;
using Silk.NET.OpenGL;

namespace Flux.Rendering.Debugging;

public class Debug
{
    readonly GL gl;
    readonly List<Mesh<LineVertex>> lines = [];
    readonly List<Mesh<LineVertex>> points = [];

    public Color Color { get; set; } = Color.Yellow;
    public float PointSize { get; set; } = 10;
    
    public Debug(GL gl)
    {
        this.gl = gl;
    }

    public void DrawClosedLine(params IEnumerable<Vector3> points) => lines.Add(MeshFromPoints(points, true));
    public void DrawLine(params IEnumerable<Vector3> points) => lines.Add(MeshFromPoints(points, false));
    public void DrawPoints(params IEnumerable<Vector3> points) => this.points.Add(MeshFromPoints(points, false));

    Mesh<LineVertex> MeshFromPoints(IEnumerable<Vector3> points, bool closed)
    {
        var vertices = points.Select(p => new LineVertex { Position = p }).ToArray();

        var numberOfIndices = vertices.Length;

        var indices = Enumerable.Range(0, numberOfIndices).Select(i => (uint)i).ToList();
        if(closed)
            indices.Add(0);
        return new Mesh<LineVertex>(gl, vertices, indices.ToArray());
    }
    
    public void DrawLineBox(Vector3 position, float size = 1, float padding = 0)
    {
        var unitX = Vector3.UnitX * size * (1f + padding);
        var unitY = Vector3.UnitY * size * (1f + padding);
        var unitZ = Vector3.UnitZ * size * (1f + padding);

        DrawLine(
            [
                position,
                position + unitX,
                position + unitX - unitY,
                position - unitY,
                position - unitY + unitZ,
                position - unitY + unitX + unitZ,
                position + unitX + unitZ,
                position + unitZ,
                position - unitY + unitZ,
                position - unitY,
                position,
                position + unitZ,
                position + unitX + unitZ,
                position + unitX,
                position + unitX - unitY,
                position + unitX + unitZ - unitY,
            ]
        );
    }

    internal void Render()
    {
        gl.PointSize(PointSize);
        foreach (var line in lines)
        {
            line.Bind();
            line.Draw(PrimitiveType.LineStrip);
            line.Dispose();
        }
        foreach (var point in points)
        {
            point.Bind();
            point.Draw(PrimitiveType.Points);
            point.Dispose();
        }
        lines.Clear();
        points.Clear();
    }
}