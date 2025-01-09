using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace MonoGame.Data.Collision;

public class AabbCollider : Component
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    
    [JsonIgnore]
    public Rectangle Rectangle => new(X, Y, Width, Height);

    public bool Intersects(Rectangle rectangle) => Rectangle.Intersects(rectangle);
    public bool Intersects(AabbCollider other) => Rectangle.Intersects(other.Rectangle);
    public bool Intersects(CircleCollider circle) => circle.Intersects(Rectangle);
}