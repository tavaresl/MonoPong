using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Core.Scripts.Components;
using MonoGame.Core.Scripts.Events;
using MonoGame.Data;
using MonoGame.Data.Collision;
using MonoGame.Data.Drawing.GUI;

namespace MonoGame.Core.Scripts.Systems;

public class MatchManagement(Game game) : GameSystem<Gameplay>(game)
{
    private int _enemyPoints;
    private int _playerPoints;
    private bool _restarting;

    public override void OnInitialise()
    {
        Start();
    }

    public override void Initialise(Gameplay component)
    {
        if (component.Entity.TryGetChild("GameOver", out var gameOverScene))
            gameOverScene.EnabledChanged += HandleGameOverSceneEnabledChanged;

        if (component.Entity.TryGetChild("Pause", out var pauseScene))
            pauseScene.EnabledChanged += HandlePauseSceneEnabledChanged;
    }

    public override void Update(Gameplay gameplay, GameTime gameTime)
    {
        if (_restarting)
        {
            _restarting = false;
            return;
        }

        var score = gameplay.Score;
        var ballCollider = gameplay.Ball.GetComponent<CircleCollider>();
        var playerCollider = gameplay.Player.GetComponent<AabbCollider>();
        var enemyCollider = gameplay.Enemy.GetComponent<AabbCollider>();
        var keyboardState = Keyboard.GetState();

        if (keyboardState.IsKeyDown(Keys.P))
        {
            if (gameplay.Entity.TryGetChild("Pause", out var pauseScene))
                pauseScene.Enabled = true;
            
            return;
        }
        
        if (ballCollider.BoundingBox.X + ballCollider.BoundingBox.Width > enemyCollider.BoundingBox.X + enemyCollider.BoundingBox.Width + 20)
        {
            _playerPoints++;
            Notify(GameEvents.Scored, new { Scorer = playerCollider.Entity });
        }

        if (ballCollider.BoundingBox.X < playerCollider.BoundingBox.X - 20)
        {
            _enemyPoints++;
            Notify(GameEvents.Scored, new { Scorer = enemyCollider.Entity });
        }
        
        score.GetComponent<TextLabel>().Text = $"{_playerPoints} - {_enemyPoints}";

        if (_enemyPoints < 11 && _playerPoints < 11)
            return;

        if (gameplay.Entity.TryGetChild("GameOver", out var gameOverScene))
            gameOverScene.Enabled = true;

        Notify(GameEvents.MatchEnded);
    }

    private void Start()
    {
        _playerPoints = 0;
        _enemyPoints = 0;
        Notify(GameEvents.MatchStarted, this);
    }

    private void HandleGameOverSceneEnabledChanged(object _, bool enabled)
    {
        Paused = enabled;

        if (!Paused)
        {
            _restarting = true;
            Start();
        }
    }

    private void HandlePauseSceneEnabledChanged(object _, bool enabled)
    {
        Paused = enabled;

        if (Paused)
            Notify(GameEvents.MatchPaused, true);
        else
            Notify(GameEvents.MatchResumed, true);
    }
}