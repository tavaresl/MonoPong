using Microsoft.Xna.Framework;
using MonoGame.Core.Scripts.Components;
using MonoGame.Data;
using MonoGame.Data.Utils.Extensions;

namespace MonoGame.Core.Scripts.Systems;

public class AiMovementController(Game game) : GameSystem<AiControl>(game)
{
    public override void Initialize()
    {
        EventBus.Subscribe("MatchEnded", OnMatchEnded);
    }

    private void OnMatchEnded(GameSystemEvent obj)
    {
        Paused = true;
    }

    public override void Update(AiControl component, GameTime gameTime)
    {
        var dir = Vector2.Zero;
        var paddle = component.Entity.GetComponent<Paddle>();
        var halfHeight = paddle.Size.Y / 2f;
        
        if (paddle.Ball.Transform.Position.Y <= paddle.Transform.Position.Y - halfHeight) dir -= Vector2.UnitY;
        if (paddle.Ball.Transform.Position.Y > paddle.Transform.Position.Y + halfHeight) dir += Vector2.UnitY;
    
        if (dir != Vector2.Zero)
            paddle.Transform.Position += dir.Normalised() * paddle.MovementSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
    }
}