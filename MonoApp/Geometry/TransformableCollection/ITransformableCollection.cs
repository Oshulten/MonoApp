using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoApp.Geometry;

public interface ITransformableCollection<TEntity>
{
    public IEnumerable<TEntity> Entities { get; set; }
    public int Count => Entities.Count();

    public ITransformableCollection<TEntity> AddTransformation(Func<TEntity, TEntity> transformation);
    public ITransformableCollection<TEntity> ClearTransformations();
    public ITransformableCollection<TEntity> ApplyTransformations();
    public ITransformableCollection<TEntity> ApplyTransformation(Func<TEntity, TEntity> transformation);
}