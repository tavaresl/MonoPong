using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

namespace MonoGame.Data.Drawing.GUI;

public class TextLabel : GuiComponent
{
    public Vector2 Size => Font?.MeasureString(Text) ?? Vector2.Zero;
    
    public override Vector2 Origin => new (Size.X * AnchorPoint.X, Size.Y * AnchorPoint.Y);
    
    [JsonIgnore]
    public SpriteFont Font { get; set; }
    public string Text { get; set; } = string.Empty;

    [JsonProperty(PropertyName = "FontName")]
    private string _fontName = string.Empty;
    
    [JsonIgnore]
    public string FontName
    {
        get => _fontName;
        set
        {
            _fontName = value;
            LoadFont();
        }
    }

    public override void Initialise()
    {
        LoadFont();
    }

    public void LoadFont()
    {
        Font = Game?.Content.Load<SpriteFont>(FontName);
    }
}