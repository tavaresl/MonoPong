using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Core.Scripts.Components;
using MonoGame.Core.Scripts.Events;
using MonoGame.Data;
using MonoGame.Data.Collision;
using MonoGame.Data.Drawing;
using MonoGame.Data.Drawing.GUI;
using MonoGame.Data.Events;
using Score = MonoGame.Core.Scripts.Components.Score;

namespace MonoGame.Core.Scripts.Systems;

public class MatchManagement(Game game) : GameSystem<Gameplay>(game)
{
    private bool _finished;
    private int _enemyPoints;
    private int _playerPoints;

    public override void OnInitialise()
    {
        Start();
        On(GameEvents.MatchStarted, OnRestart);
    }

    public override void Initialise(Gameplay component)
    {
        if (!component.Entity.TryGetChild("Pause", out var pauseScene))
            return;
        
        if (pauseScene.TryGetComponent<Button>("ResumeButton", out var resumeButton))
            resumeButton.Click += OnResume;
    }

    private void OnResume(object sender, MouseState e)
    {
        Paused = false;
    }

    public override void Update(Gameplay gameplay, GameTime gameTime)
    {
        if (_finished) return;

        var score = gameplay.Score;
        var ballCollider = gameplay.Ball.GetComponent<CircleCollider>();
        var playerCollider = gameplay.Player.GetComponent<AabbCollider>();
        var enemyCollider = gameplay.Enemy.GetComponent<AabbCollider>();
        
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

        if (gameplay.Entity.TryGetChild("Pause", out var pauseScene))
            pauseScene.Enabled = true;

        Notify(GameEvents.MatchEnded);
        _finished = true;
    }

    private void Start()
    {
        _playerPoints = 0;
        _enemyPoints = 0;
    }

    private void OnRestart(GameSystemEvent @event)
    {
        Start();
        _finished = false;
        Notify(GameEvents.MatchRestarted, this);
    }
}