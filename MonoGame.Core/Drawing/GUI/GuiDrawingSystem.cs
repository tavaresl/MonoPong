using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace MonoGame.Core.Drawing.GUI;

public class GuiDrawingSystem(Game game) : DrawingGameSystem<GuiComponent>(game)
{
    private readonly SpriteBatch _spriteBatch = new (game.GraphicsDevice);

    public override void Draw(GuiComponent component, GameTime gameTime)
    {
        _spriteBatch.Begin();
        if (component is Button button) DrawButton(button);
        else if (component is TextLabel text) DrawText(text);
        _spriteBatch.End();
    }

    private void DrawButton(Button button)
    {
        if (button.Texture == null) button.CreateTexture();
        _spriteBatch.Draw(button.Texture,
            button.Transform.Position, 
            null,
            button.Mask,
            button.Transform.Rotation,
            button.Origin,
            button.Transform.Scale,
            button.Effect,
            0f);
        
        if (button.Font == null) button.LoadFont();
        Vector2 displacement = new(button.Style.Padding[0] + button.Style.BorderWidth, button.Style.Padding[1] + button.Style.BorderWidth);
        
        _spriteBatch.DrawString(button.Font,
            button.Label,
            button.Transform.Position + displacement,
            button.Style.TextColor,
            button.Transform.Rotation,
            button.Origin,
            button.Transform.Scale,
            button.Effect,
            0f);
    }

    private void DrawText(TextLabel text)
    {
        if (text.Font == null) text.LoadFont();
        _spriteBatch.DrawString(
            text.Font,
            text.Text,
            text.Transform.Position,
            text.Mask, 
            text.Transform.Rotation, 
            text.Origin, 
            text.Transform.Scale,
            text.Effect,
            0f);
    }
}