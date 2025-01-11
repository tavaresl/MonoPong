using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

namespace MonoGame.Data.Drawing;

public class DrawableText : DrawableComponent
{
    public Vector2 Size => Font?.MeasureString(Text) ?? Vector2.Zero;
    
    public override Vector2 Origin => new (Size.X * AnchorPoint.X, Size.Y * AnchorPoint.Y);
    
    [JsonIgnore]
    public SpriteFont Font { get; set; }
    public string Text { get; set; } = string.Empty;
    public string FontName { get; set; } = string.Empty;
    
    public override void Initialise()
    {
        LoadFont();
    }

    public void LoadFont()
    {
        Font = Game?.Content.Load<SpriteFont>(FontName);
    }
}