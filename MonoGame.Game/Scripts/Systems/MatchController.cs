using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Core;
using MonoGame.Core.Collision;
using MonoGame.Core.Drawing.GUI;
using MonoGame.Game.Scripts.Components;
using MonoGame.Game.Scripts.Events;

namespace MonoGame.Game.Scripts.Systems;

public class MatchController(Microsoft.Xna.Framework.Game game) : GameSystem<Match>(game)
{
    private int _enemyPoints;
    private int _playerPoints;
    private bool _restarting;

    public override void OnInitialise()
    {
        Start();
    }

    public override void Initialise(Match component)
    {
        if (component.Entity.TryGetChild("GameOver", out var gameOverScene))
            gameOverScene.EnabledChanged += HandleGameOverSceneEnabledChanged;

        if (component.Entity.TryGetChild("Pause", out var pauseScene))
            pauseScene.EnabledChanged += HandlePauseSceneEnabledChanged;
    }

    public override void Update(Match match, GameTime gameTime)
    {
        if (_restarting)
        {
            _restarting = false;
            return;
        }

        var score = match.Score;
        var ballCollider = match.Ball.GetComponent<CircleCollider>();
        var playerCollider = match.Player.GetComponent<AabbCollider>();
        var enemyCollider = match.Enemy.GetComponent<AabbCollider>();
        var keyboardState = Keyboard.GetState();

        if (keyboardState.IsKeyDown(Keys.P))
        {
            if (match.Entity.TryGetChild("Pause", out var pauseScene))
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

        if (match.Entity.TryGetChild("GameOver", out var gameOverScene))
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