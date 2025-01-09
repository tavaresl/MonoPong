using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Data.Drawing;
using Newtonsoft.Json;

namespace MonoGame.Data.Components.Drawables.Textures.Shapes;

public sealed class RectangleTexture : DrawableTexture, IDrawableComponent
{
    [JsonProperty(PropertyName = "Color")]
    private Color _color;
    
    [JsonIgnore]
    public Color Color
    {
        get => _color;
        set
        {
            _color = value;
            CreateTexture();
        }
    }
    
    [JsonProperty(PropertyName = "Size")]
    private Vector2 _size = Vector2.One;
    
    [JsonIgnore]
    public Vector2 Size
    {
        get => _size;
        set
        {
            _size = value;
            CreateTexture();
        }
    }

    public override void CreateTexture()
    {
        if (Game == null) return;
        Texture?.Dispose();
        Texture = new Texture2D(Game.GraphicsDevice, (int)Size.X, (int)Size.Y);
        Texture.SetData(Enumerable.Repeat(Color, Texture.Width * Texture.Height).ToArray());
    }
}