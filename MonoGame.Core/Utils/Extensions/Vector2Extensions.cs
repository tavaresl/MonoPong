using Microsoft.Xna.Framework;

namespace MonoGame.Core.Utils.Extensions;

public static class Vector2Extensions
{
    public static Vector2 Normalised(this Vector2 vector)
    {
        var copy = new Vector2(vector.X, vector.Y);
        copy.Normalize();
        return copy;
    }

    public static Vector2 Reflected(this Vector2 vector, Vector2 normal)
    {
        return vector - 2 * Vector2.Dot(vector, normal.Normalised()) * normal.Normalised();
    }
}   