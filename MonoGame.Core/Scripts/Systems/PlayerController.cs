using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Core.Scripts.Components;
using MonoGame.Data;
using MonoGame.Data.Utils.Extensions;

namespace MonoGame.Core.Scripts.Systems;

public class PlayerController(Game game) : GameSystem<PlayerControl>(game)
{
    public override void Initialize()
    {
        EventBus.Subscribe("MatchEnded", OnMatchEnded);
    }

    private void OnMatchEnded(GameSystemEvent obj)
    {
        Paused = true;
    }

    public override void Update(PlayerControl component, GameTime gameTime)
    {
        var keyboardState = Keyboard.GetState();
        var dir = Vector2.Zero;
        var paddle = component.Entity.GetComponent<Paddle>();

        if (keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Up)) dir -= Vector2.UnitY;
        if (keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.Down)) dir += Vector2.UnitY;

        if (dir != Vector2.Zero)
            paddle.Transform.Position += dir.Normalised() * paddle.MovementSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
    }
}