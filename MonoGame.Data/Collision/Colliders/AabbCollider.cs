using Microsoft.Xna.Framework;
using MonoGame.Data.Drawing;
using MonoGame.Data.Drawing.Textures.Shapes;
using Newtonsoft.Json;

namespace MonoGame.Data.Collision;

public class AabbCollider : Component
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }

    [JsonIgnore]
    public Vector2 RelativePosition => Entity.Transform.Position + new Point(X, Y).ToVector2();
    
    [JsonIgnore]
    public Rectangle BoundingBox => new((int)RelativePosition.X, (int)RelativePosition.Y, Width, Height);

    public bool Intersects(Rectangle rectangle) => BoundingBox.Intersects(rectangle);
    public bool Intersects(AabbCollider other) => BoundingBox.Intersects(other.BoundingBox);
    public bool Intersects(CircleCollider circle) => circle.Intersects(BoundingBox);

    
#if DEBUG
    public override void Initialise()
    {
        Entity.AddComponent(new RectangleTexture
        {
            Name = "ColliderShaper",
            AnchorPoint = new Vector2(0.5f, 0.5f),
            Size = new Vector2(Width,Height),
            Color = Color.Transparent,
            BorderColor = Color.Green,
            BorderWidth = 1,
            Layer = 1,
        });
    }
#endif
}