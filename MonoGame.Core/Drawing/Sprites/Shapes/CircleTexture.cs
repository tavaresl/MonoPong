using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

namespace MonoGame.Core.Drawing.Sprites.Shapes;

public class CircleTexture : DrawableTexture
{
    [JsonProperty(PropertyName = "Radius")]
    private float _radius;

    [JsonIgnore]
    public float Radius
    {
        get => _radius;
        set
        {
            _radius = value;
            CreateTexture();
        }
    }

    
    [JsonProperty(PropertyName = "Color")]
    private Color _color = Color.White;

    [JsonProperty(PropertyName = "BorderColor")]
    private Color _borderColor = Color.White;

    [JsonProperty(PropertyName = "BorderWidth")]
    private int _borderWidth;

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

    [JsonIgnore]
    public Color BorderColor
    {
        get => _borderColor;
        set
        {
            _borderColor = value;
            CreateTexture();
        }
    }

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

    public override void CreateTexture()
    {
        if (Game == null) return;
        
        var diameter = (int)(2 * Radius);
        var pixels = new List<Color>();
        
        Texture?.Dispose();
        Texture = new Texture2D(Game.GraphicsDevice, diameter, diameter);

        for (int x = 0; x < diameter; x++)
        for (int y = 0; y < diameter; y++)
        {
            var d = Vector2.Distance(new Vector2(Radius), new Vector2(x, y));
            if (d > Radius) pixels.Add(Color.Transparent);
            else if (d <= Radius - BorderWidth) pixels.Add(Color);
            else pixels.Add(BorderColor);
        }    
        
        Texture.SetData(pixels.ToArray());
    }
}