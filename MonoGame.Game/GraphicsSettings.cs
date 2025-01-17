using Microsoft.Xna.Framework;
namespace MonoGame.Game;

public class GraphicsSettings
{
    public GraphicsDeviceManager GraphicsDevice { get; }

    public int Width
    {
        get => GraphicsDevice.PreferredBackBufferWidth;
        set => GraphicsDevice.PreferredBackBufferWidth = value;
    }

    public int Height
    {
        get => GraphicsDevice.PreferredBackBufferHeight;
        set => GraphicsDevice.PreferredBackBufferHeight = value;
    }

    public GraphicsSettings(Microsoft.Xna.Framework.Game game)
    {
        GraphicsDevice = new GraphicsDeviceManager(game);
        Width = 1280;
        Height = 720;
        Save();
    }
    

    public void Save()
    {
        GraphicsDevice.ApplyChanges();
    }
}   