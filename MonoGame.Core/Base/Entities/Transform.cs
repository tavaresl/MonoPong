using Newtonsoft.Json;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace MonoGame.Core;

public class Transform
{
    public Entity Entity { get; init; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Vector2 Position { get; set; } = Vector2.Zero;

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Vector2 Scale { get; set; } = Vector2.One;

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public float Rotation { get; set; }

    [JsonIgnore]
    public Vector2 AbsolutePosition =>
        Vector2.Add(Entity.Parent?.Transform.AbsolutePosition ?? Vector2.Zero, Position);
    
    [JsonIgnore]
    public Vector2 AbsoluteScale =>
        Vector2.Multiply(Entity.Parent?.Transform.AbsoluteScale ?? Vector2.One, Scale);

    [JsonIgnore]
    public float AbsoluteRotation =>
        (Entity.Parent?.Transform.AbsoluteRotation ?? 0f) + Rotation;
}
 