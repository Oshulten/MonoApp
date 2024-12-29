using ColorHelper;
using Microsoft.Xna.Framework;
using MonoApp.Generic;

namespace MonoApp.Geometry;

public class MonoVector(IEnumerable<double> values) : NumericList(values)
{
    public MonoVector Transform(Matrix transformation)
    {
        switch (Count)
        {
            case 2:
                {
                    var result = Vector2.Transform(this, transformation);
                    this[0] = result.X;
                    this[1] = result.Y;
                    break;
                }

            case 3:
                {
                    var result = Vector3.Transform(this, transformation);
                    this[0] = result.X;
                    this[1] = result.Y;
                    this[2] = result.Z;
                    break;
                }

            case 4:
                {
                    var result = Vector2.Transform(this, transformation);
                    this[0] = result.X;
                    this[1] = result.Y;
                    break;
                }
        }
        return this;
    }

    public static implicit operator Vector4(MonoVector vector) =>
        new()
        {
            X = vector.Count >= 0 ? (float)vector[0] : 0,
            Y = vector.Count >= 1 ? (float)vector[1] : 0,
            Z = vector.Count >= 2 ? (float)vector[2] : 0,
            W = vector.Count >= 3 ? (float)vector[3] : 0,
        };


    public static implicit operator Vector3(MonoVector vector) =>
        new()
        {
            X = vector.Count >= 0 ? (float)vector[0] : 0,
            Y = vector.Count >= 1 ? (float)vector[1] : 0,
            Z = vector.Count >= 2 ? (float)vector[2] : 0,
        };

    public static implicit operator Vector2(MonoVector vector) =>
        new()
        {
            X = vector.Count >= 0 ? (float)vector[0] : 0,
            Y = vector.Count >= 1 ? (float)vector[1] : 0,
        };

    public static implicit operator Color(MonoVector vector)
    {
        var rgb = ColorConverter.HslToRgb(new HSL(
            (int)(vector[0] * 360),
            (byte)(vector[1] * 100),
            (byte)(vector[2] * 100)
        ));

        return new Color(
            r: rgb.R,
            g: rgb.G,
            b: rgb.B,
            alpha: (float)vector[3]
        );
    }
}