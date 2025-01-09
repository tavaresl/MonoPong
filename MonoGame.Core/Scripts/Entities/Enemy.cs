using Microsoft.Xna.Framework;
using MonoGame.Data;
using MonoGame.Data.Components.Drawables.Textures.Shapes;

namespace MonoGame.Core.Scripts.Entities;

public sealed class Enemy : Entity
{
    private Rectangle _bounds;
    private bool _previouslyIntersectedBall;
    private readonly Vector2 _size = new (24, 96);
    public Ball Ball { get; init; }
    
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
        Transform.Position = new Vector2(game.GraphicsDevice.Viewport.Width - 128 - _size.X, game.GraphicsDevice.Viewport.Height / 2f);
        
        base.Initialise(game);
    }
}