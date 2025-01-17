using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

namespace MonoGame.Core.Drawing.Sprites.Shapes;

public sealed class RectangleTexture : DrawableTexture
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

    [JsonProperty(PropertyName = "BorderColor", NullValueHandling = NullValueHandling.Ignore)]
    private Color? _borderColor;

    [JsonIgnore]
    public Color BorderColor
    {
        get => _borderColor ?? _color;
        set
        {
            _borderColor = value;
            CreateTexture();
        }
    }

    [JsonProperty(PropertyName = "BorderWidth")]
    private int _borderWidth;

    [JsonIgnore]
    public int BorderWidth
    {
        get => _borderWidth;
        set
        {
            _borderWidth = value;
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
        Texture = new Texture2D(Game.GraphicsDevice, (int)_size.X, (int)_size.Y);
        
        Color[] data = new Color[Texture.Width * Texture.Height];

        for (int i = 0; i < data.Length; i++)
        {
            int remainder = i % Texture.Width;

            if (i < Texture.Width * _borderWidth) data[i] = BorderColor;
            else if (i >= data.Length - Texture.Width * _borderWidth - 1) data[i] = BorderColor;
            else if (remainder < _borderWidth || remainder > Texture.Width - _borderWidth - 1) data[i] = BorderColor;
            else data[i] = _color;
        }
        
        Texture.SetData(data);
    }
}