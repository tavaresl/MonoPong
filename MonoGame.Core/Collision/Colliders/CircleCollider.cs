using System;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace MonoGame.Core.Collision;

public class CircleCollider : Collider
{
    public float Radius { get; set; }
    public Vector2 Offset { get; set; }

    public float Diameter => Radius + Radius; 
    public override Vector2 RelativePosition => Transform.AbsolutePosition + Offset;

    [JsonIgnore]
    public override Rectangle BoundingBox => new(
        (int)(RelativePosition.X - Radius),
        (int)(RelativePosition.Y - Radius),
        (int)Diameter, 
        (int)Diameter);

    public bool Intersects(CircleCollider other)
    {
        if (other == this) return false;
        var distance = Vector2.Distance(RelativePosition, other.RelativePosition); 
        return distance <= Radius + other.Radius;
    }

    public bool Intersects(AabbCollider aabb) => Intersects(aabb.BoundingBox);

    public bool Intersects(Rectangle rectangle)
    {
        // temporary variables to set edges for testing
        float testX = RelativePosition.X;
        float testY = RelativePosition.Y;

        // which edge is closest?
        if (RelativePosition.X < rectangle.Left) testX = rectangle.X;      // test left edge
        else if (RelativePosition.X > rectangle.Right) testX = rectangle.Right;   // right edge
        if (RelativePosition.Y < rectangle.Top) testY = rectangle.Top;      // top edge
        else if (RelativePosition.Y > rectangle.Bottom) testY = rectangle.Bottom;   // bottom edge

        // get distance from closest edges
        float distX = RelativePosition.X - testX;
        float distY = RelativePosition.Y - testY;
        float distance = MathF.Sqrt((distX * distX) + (distY * distY));

        // if the distance is less than the radius, collision!
        if (distance <= Radius) {
            return true;
        }
        return false;
    }
    
    
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