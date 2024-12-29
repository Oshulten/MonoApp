using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoApp.Geometry;

/// <summary>
/// A collection of entities on which you can apply functional transformations
/// Caches transformed entities as transformations are added.
/// </summary>
public class TransformableCollection<TEntity>(IEnumerable<TEntity> entities) : ITransformableCollection<TEntity>
{
    private IEnumerable<TEntity> _baseEntities = entities;
    private IEnumerable<TEntity> _transformedEntities = entities;
    private List<Func<TEntity, TEntity>> _transformations = [];

    public TransformableCollection() : this([]) { }

    /// <summary>
    /// Returns the most up-to-date transformed version of the entities.
    /// </summary>
    public IEnumerable<TEntity> Entities
    {
        get => _transformedEntities; set
        {
            _baseEntities = value;
            _transformedEntities = RecalculateTransformedEntities();
        }
    }

    /// <summary>
    /// Adds a transformation to the list and updates _transformedEntities by applying
    /// the given transformation.
    /// </summary>
    /// <param name="transformation"></param>
    /// <returns></returns>
    public ITransformableCollection<TEntity> AddTransformation(Func<TEntity, TEntity> transformation)
    {
        _transformations.Add(transformation);

        _transformedEntities = from entity in _transformedEntities
                               select transformation(entity);

        return this;
    }

    /// <summary>
    /// Clears all transformations from the list and synchronizes
    /// _transformedEntities with _baseEntities.
    /// </summary>
    public ITransformableCollection<TEntity> ClearTransformations()
    {
        _transformations.Clear();

        _transformedEntities = _baseEntities;

        return this;
    }

    /// <summary>
    /// Applies all stored transformations on _baseEntities to produce
    /// an up-to-date _transformedEntities.
    /// 
    /// Leaves _baseEntities unchanged.
    /// </summary>
    private IEnumerable<TEntity> RecalculateTransformedEntities()
    {
        return _transformedEntities =
            from entity in _baseEntities
            from transformation in _transformations
            select transformation(entity);
    }

    public int Count => Entities.Count();

    /// <summary>
    /// Applies all stored transformations to _baseEntities in order,
    /// and clears the list of transformations.
    /// Clears _transformedEntities.
    /// </summary>
    public ITransformableCollection<TEntity> ApplyTransformations()
    {
        _baseEntities = _transformedEntities;
        _transformations.Clear();
        return this;
    }

    /// <summary>
    /// Applies a given transformation directly on _baseEntities.
    /// _transformedEntities are recalculated.
    /// </summary>
    /// <param name="transformation"></param>
    /// <returns></returns>
    public ITransformableCollection<TEntity> ApplyTransformation(Func<TEntity, TEntity> transformation)
    {
        _baseEntities = from entity in _baseEntities
                        select transformation(entity);

        _transformedEntities = RecalculateTransformedEntities();

        return this;
    }
}