using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Data.Components.Drawables.Textures;
using MonoGame.Data.Utils.Extensions;

namespace MonoGame.Data.Drawing;

[GameSystem(typeof(IDrawableComponent))]
public class DrawingSystem(Game game) : DrawableGameComponent(game)
{
    private readonly SpriteBatch _spriteBatch = new(game.GraphicsDevice);
    
    public override void Draw(GameTime gameTime)
    {
        var drawables = Game.Query<IDrawableComponent>();
        
        foreach (var drawable in drawables)
        {
            switch (drawable)
            {
                case DrawableTexture texture: 
                    DrawTexture(texture);
                    break;
                case DrawableText text:
                    DrawText(text);
                    break;
            }
        }
    }

    private void DrawTexture(DrawableTexture component)
    {
        if (component.Texture == null) component.CreateTexture();

        _spriteBatch.Begin();
        _spriteBatch.Draw(component.Texture,
            component.Entity.Transform.Position, 
            null,
            component.Mask,
            component.Entity.Transform.Rotation,
            component.Origin,
            component.Entity.Transform.Scale,
            component.Effect,
            component.Layer);
        _spriteBatch.End();
    }

    private void DrawText(DrawableText component)
    {
        _spriteBatch.Begin();
        _spriteBatch.DrawString(
            component.Font,
            component.Text,
            component.Entity.Transform.Position,
            component.Mask, 
            component.Entity.Transform.Rotation, 
            component.Origin, 
            component.Entity.Transform.Scale,
            component.Effect,
            component.Layer);
        _spriteBatch.End();
    }
}