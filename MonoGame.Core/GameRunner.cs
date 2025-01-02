using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Core.Scripts.Entities;

namespace MonoGame.Core;

public class GameRunner : Game
{
    private readonly Score _score;
    private readonly Player _player;
    private readonly Ball _ball;
    private readonly Enemy _enemy;
    public GraphicsSettings GraphicsSettings { get; private set; }

    public GameRunner()
    {
        GraphicsSettings = new GraphicsSettings(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        _score = new Score();
        _player = new Player();
        _enemy = new Enemy();
        _ball = new Ball();

        _player.Ball = _ball;
        _enemy.Ball = _ball;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        _player.Initialise(this);
        _enemy.Initialise(this);
        _ball.Initialise(this);
        _score.Initialise(this);
        base.Initialize();
    }

    protected override void LoadContent()
    {
        // TODO: use this.Content to load your game content here
        _player.LoadContent(this);
        _enemy.LoadContent(this);
        _ball.LoadContent(this);
        _score.LoadContent(this);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        _player.Update(gameTime);
        _enemy.Update(gameTime);
        _ball.Update(gameTime);
        _score.Update(gameTime);

        if (_ball.BoundingBox.X + _ball.BoundingBox.Width > GraphicsDevice.Viewport.Width - 80)
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

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        _player.Draw();
        _enemy.Draw();
        _ball.Draw();
        _score.Draw();
        base.Draw(gameTime);
    }
}