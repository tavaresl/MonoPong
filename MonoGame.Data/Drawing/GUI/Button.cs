using System.Text.Json.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Data.Drawing.GUI;

public class Button : InteractiveGuiComponent
{
    [JsonIgnore] internal Texture2D Texture { get; set; }
    [JsonIgnore] internal SpriteFont Font { get; set; }

    public override Rectangle Bounds => new((int)(Transform.AbsolutePosition.X - AnchorPoint.X * Texture.Width),
        (int)(Transform.AbsolutePosition.Y - AnchorPoint.Y * Texture.Height), Texture.Width, Texture.Height);
    public override Vector2 Origin => Vector2.Multiply(new Vector2(Texture.Width, Texture.Height), AnchorPoint);
    public int[] Padding { get; set; } = [0, 0, 0, 0];
    public Color BorderColor { get; set; } = Color.White;
    public int BorderWidth { get; set; }
    public Color BackgroundColor { get; set; } = Color.White;
    public Color TextColor { get; set; } = Color.Black;
    public string Label { get; set; } = string.Empty;
    public string FontName { get; set; }

    public override void Initialise()
    {
        LoadFont();
        CreateTexture();
    }

    public void CreateTexture()
    {
        if (Game == null) return;
        Vector2 labelSize = Font?.MeasureString(Label) ?? Vector2.Zero;
        int width = Padding[3] + (int)labelSize.X + Padding[1] + 2 * BorderWidth;
        int height =  Padding[2] + (int)labelSize.Y + Padding[0] + 2 * BorderWidth;

        Texture?.Dispose();
        Texture = new Texture2D(Game.GraphicsDevice, width, height);
        
        Color[] data = new Color[Texture.Width * Texture.Height];

        for (int i = 0; i < data.Length; i++)
        {
            int remainder = i % Texture.Width;

            if (i < width * BorderWidth) data[i] = BorderColor;
            else if (i >= data.Length - width * BorderWidth - 1) data[i] = BorderColor;
            else if (remainder < BorderWidth || remainder > width - BorderWidth - 1) data[i] = BorderColor;
            else data[i] = BackgroundColor;
        }
        
        Texture.SetData(data);
    }

    public void LoadFont()
    {
        if (Game == null) return;
        Font = Game.Content.Load<SpriteFont>(FontName);
    }
}