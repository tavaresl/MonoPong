using Microsoft.Xna.Framework;
using MonoGame.Core.Scripts.Components.Drawables;
using MonoGame.Core.Utils.Extensions;

namespace MonoGame.Core.Scripts.Entities;

public sealed class Enemy : Entity
{
    private const float MovementSpeed = 300f;

    private Rectangle _bounds;
    private bool _previouslyIntersectedBall;
    private readonly Vector2 _size = new (20, 80);
    public Ball Ball { get; init; }
    
    public override Rectangle BoundingBox => new((int)(Transform.Position.X - _size.X / 2), 
        (int)(Transform.Position.Y - _size.Y / 2), (int)_size.X, (int)_size.Y);

    public override void Initialise(Game1 game)
    {
        Transform.Position = new Vector2(game.GraphicsDevice.Viewport.Width - 80 - _size.X, game.GraphicsDevice.Viewport.Height / 2f);
        _bounds = new Rectangle(0, 0, game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height);
        
        AddComponent(new RectangleDrawer { Size = _size, Color = Color.White, AnchorPoint = new Vector2(.5f, .5f)});
        base.Initialise(game);
    }

    public override void Update(GameTime gameTime)
    {
        var dir = Vector2.Zero;
        var halfHeight = _size.Y / 2f;
        
        if (Ball.Transform.Position.Y <= Transform.Position.Y - halfHeight) dir -= Vector2.UnitY;
        if (Ball.Transform.Position.Y >= Transform.Position.Y + halfHeight) dir += Vector2.UnitY;

        if (dir != Vector2.Zero)
            Transform.Position += dir.Normalised() * MovementSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (Transform.Position.Y - halfHeight <= 0)
            Transform.Position = new Vector2(Transform.Position.X, halfHeight);
        else if (Transform.Position.Y + halfHeight >= _bounds.Height)
            Transform.Position = new Vector2(Transform.Position.X, _bounds.Height - halfHeight);
        
        if (Ball.BoundingBox.Intersects(BoundingBox))
        {
            if (!_previouslyIntersectedBall) Ball.GetHitBy(this, -Vector2.UnitX);    
            _previouslyIntersectedBall = true;
        }
        else
        {
            _previouslyIntersectedBall = false;
        }
    }
}