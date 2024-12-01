using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Mono.Geometry;

public class VectorGenerator
{
    public static IEnumerable<Vector3> Circle(int segments, float thetaOffset = 0) =>
        Iterator
            .Range((0, 2 * MathHelper.Pi), segments)
            .Select(theta => new Vector3(
                (float)Math.Cos(-theta + thetaOffset),
                (float)Math.Sin(-theta + thetaOffset),
                0));
}