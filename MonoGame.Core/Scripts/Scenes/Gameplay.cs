using Microsoft.Xna.Framework;
using MonoGame.Core.Scripts.Entities;

namespace MonoGame.Core.Scripts.Scenes;

public class Gameplay(Game1 game1) : Scene(game1)
{
    private Player _player;
    private Enemy _enemy;
    private Ball _ball;
    private Score _score;

    public override void Initialise(Game game)
    {
        _ball = new Ball();
        _player = new Player { Ball = _ball };
        _enemy = new Enemy { Ball = _ball };
        _score = new Score();
        
        Entities.Add(_ball);       
        Entities.Add(_player);
        Entities.Add(_enemy);
        Entities.Add(_score);

        base.Initialise(game);
    }

    public override void Update(GameTime gameTime)
    {
        if (_ball.BoundingBox.X + _ball.BoundingBox.Width > Game.GraphicsDevice.Viewport.Width - 80)
        {
            _score.PlayerPoints++;
            _ball.Reset();
        }

        if (_ball.BoundingBox.X < 80)
        {
            _score.EnemyPoints++;
            _ball.Reset();
        }   
        
        base.Update(gameTime);
    }
}