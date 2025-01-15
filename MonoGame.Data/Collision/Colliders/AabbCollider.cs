using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace MonoGame.Data.Collision;

public class AabbCollider : Collider
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }

    [JsonIgnore]
    public override Vector2 RelativePosition => Entity.Transform.Position + new Point(X, Y).ToVector2();
    
    [JsonIgnore]
    public override Rectangle BoundingBox => new((int)RelativePosition.X, (int)RelativePosition.Y, Width, Height);

    public bool Intersects(Rectangle rectangle) => BoundingBox.Intersects(rectangle);

    public bool Intersects(AabbCollider other)
    {
        return other != this && BoundingBox.Intersects(other.BoundingBox);
    }
    public bool Intersects(CircleCollider circle) => circle.Intersects(BoundingBox);

    public override bool IsCollidingWith(Collider other)
    {
        return other switch
        {
            AabbCollider aabb => Intersects(aabb),
            CircleCollider circle => Intersects(circle),
            _ => false
        };
    }
}