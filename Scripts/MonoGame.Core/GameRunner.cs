using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Entities;
using MonoGame.Entities.Monogame.Entities.Player;

namespace MonoGame.Core;

public class GameRunner : Game
{
    private GraphicsDeviceManager _graphics;
    private IEntity _player;
    public GameRunner()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        _player = new Player();
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        _player.Initialise(this);
        base.Initialize();
    }

    protected override void LoadContent()
    {
        // TODO: use this.Content to load your game content here
        _player.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        _player.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        _player.Draw();
        base.Draw(gameTime);
    }
}