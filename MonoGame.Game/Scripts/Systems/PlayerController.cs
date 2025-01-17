using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Core;
using MonoGame.Core.Events;
using MonoGame.Core.Utils.Extensions;
using MonoGame.Game.Scripts.Components;
using MonoGame.Game.Scripts.Events;

namespace MonoGame.Game.Scripts.Systems;

public class PlayerController(Microsoft.Xna.Framework.Game game) : GameSystem<Player>(game)
{
    public override void OnInitialise()
    {
        On(GameEvents.MatchStarted, OnMatchStarted);
        On(GameEvents.MatchResumed, OnMatchStarted);
        On(GameEvents.MatchPaused, OnMatchEnded);
        On(GameEvents.MatchEnded, OnMatchEnded);
    }

    private void OnMatchStarted(GameSystemEvent obj) => Paused = false;
    private void OnMatchEnded(GameSystemEvent obj) => Paused = true;

    public override void Update(Player component, GameTime gameTime)
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