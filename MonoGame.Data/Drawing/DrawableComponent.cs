using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

namespace MonoGame.Data.Drawing;

public abstract class DrawableComponent : Component, IDrawableComponent
{
    
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Vector2 AnchorPoint { get; set; } = Vector2.Zero;
    
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public SpriteEffects Effect { get; set; } = SpriteEffects.None;
    
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public float Layer { get; set; } = 0f;
    
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Color Mask { get; set; } = Color.White;
    
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public virtual Rectangle Bounds { get; set; }
    
    [JsonIgnore]
    public abstract Vector2 Origin { get; }
}
