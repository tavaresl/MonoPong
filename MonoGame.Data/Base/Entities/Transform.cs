using Newtonsoft.Json;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace MonoGame.Data;

public class Transform
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Vector2 Position { get; set; } = Vector2.Zero;

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Vector2 Scale { get; set; } = Vector2.One;

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public float Rotation { get; set; }
}
 