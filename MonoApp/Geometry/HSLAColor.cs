using Microsoft.Xna.Framework;
using ColorHelper;

namespace Mono.Geometry;

public class HSLAColor(Vector4 hsla)
{
    private Vector4 _hsla = hsla;

    public HSLAColor(float hue, float saturation, float luminance, float alpha)
     : this(new(hue, saturation, luminance, alpha)) { }

    public static implicit operator Color(HSLAColor color)
    {
        var rgb = ColorConverter.HslToRgb(new HSL(
            (int)(color._hsla.X * 360),
            (byte)(color._hsla.Y * 100),
            (byte)(color._hsla.Z * 100)
        ));

        return new(rgb.R, rgb.G, rgb.B, color._hsla.W);
    }
}