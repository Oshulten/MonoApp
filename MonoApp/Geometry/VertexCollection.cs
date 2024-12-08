using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mono.Geometry;

public class TransformableCollection<TEntity>
{
    public IEnumerable<TEntity> Entities { get; set; } = [];
    public List<Func<TEntity, TEntity>> Transformations { get; set; } = [];

    public IEnumerable<TEntity> TransformedEntities
    {
        get
        {
            if (!Transformations.Any())
                return Entities;

            return from entity in Entities
                   from transformation in Transformations
                   select transformation(entity);
        }
    }

    public int Count => Entities.Count();

    public TransformableCollection<TEntity> ApplyTransformations()
    {
        Entities = TransformedEntities;
        Transformations.Clear();
        return this;
    }

    public TransformableCollection<TEntity> ApplyTransformation(Func<TEntity, TEntity> transformation)
    {
        Entities =
            from entity in Entities
            select transformation(entity);

        return this;
    }
}

public class VertexCollection : TransformableCollection<Vector3>
{
    public IEnumerable<Vector3> Vertices { get => Entities; set => Entities = value; }
    public IEnumerable<Vector3> TransformedVertices => TransformedEntities;

    public T ApplyTransformation<T>(Matrix matrix)
    where T : VertexCollection =>
        (T)ApplyTransformation(vertex => Vector3.Transform(vertex, matrix));

    public T ApplyTransformation<T>(Func<Vector3, Vector3> transformation)
    where T : VertexCollection =>
        (T)ApplyTransformation(vertex => transformation(vertex));

    public VertexCollection TransformedClone(Func<Vector3, Vector3> transformation) =>
        (VertexCollection)((VertexCollection)MemberwiseClone())
            .ApplyTransformation(transformation);

    public IEnumerable<VertexCollection> LinearArray(Vector3 step, int resultCount)
    {
        return
            from i in Enumerable.Range(0, resultCount)
            select TransformedClone(vertex =>
                Vector3.Transform(vertex, Matrix.CreateTranslation(step * i)));
    }

    public IEnumerable<T> RectangularArray<T>(Vector3 uStep, Vector3 vStep, int uResultCount, int vResultCount)
    where T : VertexCollection
    {
        return
            from u in Enumerable.Range(0, uResultCount)
            from v in Enumerable.Range(0, vResultCount)
            select (T)TransformedClone(vertex =>
                Vector3.Transform(vertex, Matrix.CreateTranslation(uStep * u + vStep * v)));
    }

    public IEnumerable<VertexCollection> PolarArray(int steps)
    {
        return
            from theta in Iterators.Range((0, (float)Math.PI * 2), steps)
            select TransformedClone(vertex => Vector3.Transform(vertex, Matrix.CreateRotationZ(theta)));
    }

    public virtual void Draw(GraphicsDevice device) { }
}