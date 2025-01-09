using Microsoft.Xna.Framework;
using MonoGame.Data;
using MonoGame.Data.Components.Drawables.Textures.Shapes;

namespace MonoGame.Core.Scripts.Entities;

public sealed class Player : Entity
{
    private const float MovementSpeed = 300f;
    
    private Rectangle _bounds;
    private bool _previouslyIntersectedBall;
    public Ball Ball { get; set; }

    public override Rectangle BoundingBox
    {
        get
        {
            var size = GetComponent<RectangleTexture>().Size;
            return new Rectangle((int)(Transform.Position.X - size.X / 2), (int)(Transform.Position.Y - size.Y / 2),
                (int)size.X, (int)size.Y);
        }
    }

    public override void Initialise(Game game)
    {
        _bounds = new Rectangle(0, 0, game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height);
        Transform.Position = new Vector2(128, game.GraphicsDevice.Viewport.Height / 2f);
        base.Initialise(game);
    }
}