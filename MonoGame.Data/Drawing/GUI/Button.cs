using System.Collections.Generic;
using System.Text.Json.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Data.Drawing.GUI;

public class Button : InteractiveGuiComponent
{
    [JsonIgnore] internal Texture2D Texture => GetCurrentTexture();
    [JsonIgnore] internal ButtonStyle Style => GetCurrentStyle();

    [JsonIgnore] internal SpriteFont Font { get; set; }

    private Dictionary<InteractionState, Texture2D> _textures = new();

    public override Rectangle Bounds => new((int)(Transform.AbsolutePosition.X - AnchorPoint.X * Texture.Width),
        (int)(Transform.AbsolutePosition.Y - AnchorPoint.Y * Texture.Height), Texture.Width, Texture.Height);
    public override Vector2 Origin => Vector2.Multiply(new Vector2(Texture.Width, Texture.Height), AnchorPoint);

    public Dictionary<InteractionState, ButtonStyle> Styles { get; set; } = new([
        new KeyValuePair<InteractionState, ButtonStyle>(InteractionState.Idle, new ButtonStyle())
    ]);
    
    public string Label { get; set; } = string.Empty;
    public string FontName { get; set; } = string.Empty;

    public override void Initialise()
    {
        LoadFont();
        CreateTexture();
    }

    public void CreateTexture()
    {
        if (Game == null) return;
        Point labelSize = Font?.MeasureString(Label).ToPoint() ?? Point.Zero;

        foreach (var (state, style) in Styles)
        {
            int width = 2 * style.Padding[0] + labelSize.X + 2 * style.BorderWidth;
            int height =  2 * style.Padding[1] + labelSize.Y + 2 * style.BorderWidth;

            Texture2D texture = new Texture2D(Game.GraphicsDevice, width, height);
            
            Color[] data = new Color[width * height];
            int currentLine = 0;

            for (int i = 0; i < data.Length; i++)
            {
                int remainder = i % width;

                if (remainder <= style.BorderRadius)
                {
                    if (currentLine <= style.BorderRadius)
                    {
                        var origin = new Vector2(style.BorderRadius);
                        data[i] = GetCornerPixelColor(remainder, currentLine, origin, style);
                    }
                    else if (currentLine >= height - style.BorderRadius)
                    {
                        var origin = new Vector2(style.BorderRadius, height - style.BorderRadius);
                        data[i] = GetCornerPixelColor(remainder, currentLine, origin, style);
                    }
                    else
                    {
                        data[i] = remainder <= style.BorderWidth ? style.BorderColor : style.BackgroundColor;
                    }
                }
                else if (remainder >= width - style.BorderRadius)
                {
                    if (currentLine <= style.BorderRadius)
                    {
                        var origin = new Vector2(width - style.BorderRadius, style.BorderRadius);
                        data[i] = GetCornerPixelColor(remainder, currentLine, origin, style);
                    }
                    else if (currentLine >= height - style.BorderRadius)
                    {
                        var origin = new Vector2(width - style.BorderRadius, height - style.BorderRadius);
                        data[i] = GetCornerPixelColor(remainder, currentLine, origin, style);
                    }
                    else
                    {
                        data[i] = remainder >= width - style.BorderWidth ? style.BorderColor : style.BackgroundColor;
                    }
                }
                else
                {
                    if (i < width * style.BorderWidth) data[i] = style.BorderColor;
                    else if (i >= data.Length - width * style.BorderWidth - 1) data[i] = style.BorderColor;
                    else if (remainder < style.BorderWidth || remainder > width - style.BorderWidth - 1) data[i] = style.BorderColor;
                    else data[i] = style.BackgroundColor;
                }

                if (remainder == width - 1)
                    currentLine++;
            }
            
            if (_textures.TryGetValue(state, out var currentTexture))
                currentTexture?.Dispose();
            
            texture.SetData(data);
            _textures[state] = texture;
        }
    }

    private Color GetCornerPixelColor(int x, int y, Vector2 origin, ButtonStyle style)
    {
        var d = Vector2.Distance(origin, new Vector2(x, y));
        if (d > style.BorderRadius) return Color.Transparent;
        if (d <= style.BorderRadius - style.BorderWidth) return style.BackgroundColor;
        return style.BorderColor;
    }

    public void LoadFont()
    {
        if (Game == null) return;
        Font = Game.Content.Load<SpriteFont>(FontName);
    }

    private ButtonStyle GetCurrentStyle()
    {
        if (Pressed && Styles.TryGetValue(InteractionState.Pressed, out var clickedStyle))
            return clickedStyle;
        if ((Pressed || Hovered) && Styles.TryGetValue(InteractionState.Hovered, out var hoveredStyle))
            return hoveredStyle;
        if (Styles.TryGetValue(InteractionState.Idle, out var idleStyle))
            return idleStyle;

        return new ButtonStyle();
    }

    private Texture2D GetCurrentTexture()
    {
        if (Pressed && _textures.TryGetValue(InteractionState.Pressed, out var clickedTexture))
            return clickedTexture;
        if ((Pressed || Hovered) && _textures.TryGetValue(InteractionState.Hovered, out var hoveredTexture))
            return hoveredTexture;
        if (_textures.TryGetValue(InteractionState.Idle, out var idleTexture))
            return idleTexture;

        return new Texture2D(Game.GraphicsDevice, 0, 0);
    }
}

public class ButtonStyle
{
    public int[] Padding { get; set; } = [0, 0];
    public Color BorderColor { get; set; } = Color.White;
    public int BorderWidth { get; set; } = 0;
    public int BorderRadius { get; set; } = 0;
    public Color BackgroundColor { get; set; } = Color.Black;

    public Color TextColor { get; set; } = Color.White;
}

public enum InteractionState
{
    Idle,
    Hovered,
    Pressed,
}