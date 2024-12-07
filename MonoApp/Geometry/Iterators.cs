using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Mono.Geometry;

public static class Iterators
{
    public static IEnumerable<(int, int)> DoubleRange(int uStart, int uCount, int vStart, int vCount)
    {
        for (int u = uStart; u < uCount + uStart; u++)
        {
            for (int v = vStart; v < vCount + vStart; v++)
            {
                yield return (u, v);
            }
        }
    }

    public static IEnumerable<float> Range(int numberOfValues)
    {
        var delta = 1.0f / (numberOfValues - 1.0f);

        for (float i = 0; i < numberOfValues; i++)
        {
            yield return delta * i;
        }
    }

    public static IEnumerable<float> Range((float min, float max) bounds, int numberOfValues)
    {
        var interval = bounds.max - bounds.min;
        var delta = interval / (numberOfValues - 1);

        for (var i = 0; i < numberOfValues; i++)
        {
            yield return bounds.min + delta * i;
        }
    }

    public static IEnumerable<Vector3> VectorGrid(int numberOfUValues, int numberOfVValues)
    {
        foreach (var v in Range(numberOfVValues))
        {
            foreach (var u in Range(numberOfUValues))
            {
                yield return new Vector3(u, v, 0);
            }
        }
    }

    public static IEnumerable<(float, float)> Grid(int uValues, int vValues)
    {
        foreach (var v in Range(vValues))
        {
            foreach (var u in Range(uValues))
            {
                yield return (u, v);
            }
        }
    }

    public static IEnumerable<(float, float)> Grid(
        (float min, float max) uBounds, int uValues,
        (float min, float max) vBounds, int vValues)
    {
        foreach (var v in Range((vBounds.min, vBounds.max), vValues))
        {
            foreach (var u in Range((uBounds.min, uBounds.max), uValues))
            {
                yield return (u, v);
            }
        }
    }

    public static IEnumerable<Vector3> Transform(this IEnumerable<Vector3> vectors, Matrix matrix)
    {
        foreach (var vector in vectors)
        {
            yield return Vector3.Transform(vector, matrix);
        }
    }
}