using Microsoft.Xna.Framework;
using MonoGame.Core.Scripts.Components.Drawables;

namespace MonoGame.Core.Scripts.Entities;

public sealed class Score : Entity
{
    private TextDrawer _textDrawer;
    
    public int PlayerPoints { get; set; }
    public int EnemyPoints { get; set; }

    private string Text => $"{PlayerPoints} - {EnemyPoints}";
    private Vector2 Size => _textDrawer.Size;
    public override Rectangle BoundingBox => new(
        (int)(Transform.Position.X - Size.X / 2),
        (int)(Transform.Position.Y - Size.Y / 2),
        (int)Size.X,
        (int)Size.Y / 2);

    public override void Initialise(Game1 game)
    {
        Transform.Position = new Vector2(game.GraphicsDevice.Viewport.Width / 2f, 20f);
        _textDrawer = new TextDrawer
        {
            Text = Text,
            FontName = "Fonts/ArialRegular72",
            AnchorPoint = new Vector2(.5f, 0)
        }; 

        AddComponent(_textDrawer);
        base.Initialise(game);
    }

    public override void Update(GameTime gameTime)
    {
        _textDrawer.Text = Text;
    }
}