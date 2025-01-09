using Microsoft.Xna.Framework;
using MonoGame.Data;
using MonoGame.Data.Drawing;

namespace MonoGame.Core.Scripts.Entities;

public sealed class Score : Entity
{
    private DrawableText _drawableText;
    
    public int PlayerPoints { get; set; }
    public int EnemyPoints { get; set; }

    private string Text => GetComponent<Components.Score>().Text;
    private Vector2 Size => _drawableText.Size;
    public override Rectangle BoundingBox => new(
        (int)(Transform.Position.X - Size.X / 2),
        (int)(Transform.Position.Y - Size.Y / 2),
        (int)Size.X,
        (int)Size.Y);

    public override void Initialise(Game game)
    {
        Transform.Position = new Vector2(game.GraphicsDevice.Viewport.Width / 2f, 20f);
        _drawableText = new DrawableText
        {
            Text = Text,
            FontName = "Fonts/ArialRegular72",
            AnchorPoint = new Vector2(.5f, 0)
        }; 

        AddComponent(_drawableText);
        base.Initialise(game);
    }

    public override void Update(GameTime gameTime)
    {
        _drawableText.Text = Text;
    }
}