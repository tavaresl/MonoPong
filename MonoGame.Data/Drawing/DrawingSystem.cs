using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Data.Drawing.Textures;

namespace MonoGame.Data.Drawing;

public class DrawingSystem(Game game) : DrawingGameSystem<IDrawableComponent>(game)
{
    private readonly SpriteBatch _spriteBatch = new(game.GraphicsDevice);
    
    public override void Draw(IDrawableComponent component, GameTime gameTime)
    {
        switch (component)
        {
            case DrawableTexture texture: 
                DrawTexture(texture);
                break;
            case DrawableText text:
                DrawText(text);
                break;
        }
    }

    private void DrawTexture(DrawableTexture component)
    {
        if (component.Texture == null) component.CreateTexture();
        _spriteBatch.Begin();
        _spriteBatch.Draw(component.Texture,
            component.Transform.Position, 
            null,
            component.Mask,
            component.Transform.Rotation,
            component.Origin,
            component.Transform.Scale,
            component.Effect,
            component.Layer);
        _spriteBatch.End();
    }

    private void DrawText(DrawableText component)
    {   
        if (component.Font == null) component.LoadFont();
        _spriteBatch.Begin();
        _spriteBatch.DrawString(
            component.Font,
            component.Text,
            component.Transform.Position,
            component.Mask, 
            component.Transform.Rotation, 
            component.Origin, 
            component.Transform.Scale,
            component.Effect,
            component.Layer);
        _spriteBatch.End();
    }
}