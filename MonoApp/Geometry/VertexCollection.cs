using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mono.Geometry;

public interface IVertexCollection
{
    
    public void Draw(GraphicsDevice device);
}

public abstract class VertexCollection
{
    public IEnumerable<Vector3> Vertices { get; set; } = [];
    public IEnumerable<Color> Colors { get; set; } = [];
    public Matrix Transformation { get; set; } = Matrix.Identity;

    public T Transform<T>(Func<Vector3, Vector3> transformation) where T: VertexCollection
    {
        Vertices = Vertices.Select(vertex => transformation(vertex));
        return (T)this;
    }

    public T TransformClone<T>(Func<Vector3, Vector3> transformation) where T: VertexCollection
    {
        var clone = (T)MemberwiseClone();
        clone.Vertices = clone.Vertices.Select(vertex => transformation(vertex));
        return clone;
    }

    public T Transform<T>(Matrix transformation) where T: VertexCollection
    {
        Transformation *= transformation;
        return (T)this;
    }

    public T TransformClone<T>(Matrix transformation) where T: VertexCollection
    {
        var clone = (T)MemberwiseClone();
        clone.Transformation *= transformation;
        return clone;
    }

    public T ApplyTransformation<T>() where T: VertexCollection
    {
        Vertices = Vertices.Select(vertex => Vector3.Transform(vertex, Transformation));
        Transformation = Matrix.Identity;
        return (T)this;
    }

    public IEnumerable<T> LinearArray<T>(Vector3 step, int resultCount) where T: VertexCollection
    {
        return Enumerable
            .Range(0, resultCount)
            .Select(i =>
            {
                var clone = (T)MemberwiseClone();
                clone.Transformation *= Matrix.CreateTranslation(step * i);
                return clone;
            });
    }

    public IEnumerable<T> RectangularArray<T>(Vector3 uStep, Vector3 vStep, int uResultCount, int vResultCount) where T: VertexCollection
    {
        return Enumerable
            .Range(0, uResultCount)
            .SelectMany(u => Enumerable.Range(0, vResultCount)
                .Select(v =>
                {
                    var clone = (T)MemberwiseClone();
                    clone.Transformation *= Matrix.CreateTranslation(uStep * u + vStep * v);
                    return clone;
                }));
    }

    public IEnumerable<T> PolarArray<T>(int steps) where T: VertexCollection
    {
        return Iterator.Range((0, (float)Math.PI * 2), steps)
            .Select(theta =>
            {
                var clone = (T)MemberwiseClone();
                clone.Transformation *= Matrix.CreateRotationZ(theta);
                return clone;
            });
    }

    public virtual void Log(string filePath)
    {
        string[] lines = { "First line", "Second line", "Third line" };

        // Set a variable to the Documents path.
        string docPath =
          Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        // Write the string array to a new file named "WriteLines.txt".
        using var outputFile = new StreamWriter(Path.Combine(docPath, "WriteLines.txt"));
        {
            foreach (string line in lines)
                outputFile.WriteLine(line);
        }
    }

    public abstract void Draw(GraphicsDevice device);
}