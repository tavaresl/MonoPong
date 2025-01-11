using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

namespace MonoGame.Data.Drawing.Textures.Shapes;

public class DashedLineTexture : DrawableTexture
{
    [JsonProperty(PropertyName = "Pattern")] private int[] _pattern = [16, 16];
    [JsonProperty(PropertyName = "StrokeWidth")] private int _strokeWidth = 1;
    [JsonProperty(PropertyName = "Length")] private int _length = 1;
    [JsonProperty(PropertyName = "Color")] private Color _color = Color.White;
    
    [JsonIgnore] public int[] Pattern
    {
        get => _pattern;
        set 
        {
            _pattern = value;
            CreateTexture();
        }
    }
    
    [JsonIgnore] public int StrokeWidth
    {
        get => _strokeWidth;
        set 
        {
            _strokeWidth = value;
            CreateTexture();
        }
    }
    
    [JsonIgnore] public int Length
    {
        get => _length;
        set 
        {
            _length = value;
            CreateTexture();
        }
    }
    
    [JsonIgnore] public Color Color
    {
        get => _color;
        set 
        {
            _color = value;
            CreateTexture();
        }
    }

    public override void CreateTexture()
    {
        if (Game == null) return;

        var maxPixels = StrokeWidth * Length;
        var pixels = new List<Color>();
        var transparent = false;

        Texture?.Dispose();
        Texture = new Texture2D(Game.GraphicsDevice, StrokeWidth, Length);
        
        while (true)
        {
            foreach (var patternSlice in Pattern)
            {
                for (var i = 0; i < StrokeWidth * patternSlice; i++)
                {
                    pixels.Add(transparent ? Color.Transparent : Color);
                    if (pixels.Count == maxPixels)  goto Fill;
                }

                transparent = !transparent;
            }
        }
        
        Fill:
        Texture.SetData(pixels.ToArray());
    }
}