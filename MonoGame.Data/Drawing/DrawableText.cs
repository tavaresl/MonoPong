using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

namespace MonoGame.Data.Drawing;

public class DrawableText : DrawableComponent
{
    public override Game Game
    {
        get => base.Game;
        set
        {
            base.Game = value;
            Font = value.Content.Load<SpriteFont>(FontName);
        }
    }
    
    public Vector2 Size => Font.MeasureString(Text);
    
    public override Vector2 Origin => new (Size.X * AnchorPoint.X, Size.Y * AnchorPoint.Y);
    
    [JsonIgnore]
    public SpriteFont Font { get; set; }
    public string Text { get; set; } = string.Empty;
    public string FontName { get; set; } = string.Empty;
}