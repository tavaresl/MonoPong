using Microsoft.Xna.Framework;
using MonoGame.Core.Scripts.Entities;

namespace MonoGame.Core.Scripts.Scenes;

public class Gameplay : IScene
{
    public Player Player { get; private set; }
    public Ball Ball { get; private set; }
    
    public bool HasStarted { get; private set; }
    
    public void Dispose()
    {
        throw new System.NotImplementedException();
    }
    
    public void Initialise(Game game)
    {
        throw new System.NotImplementedException();
    }

    public void LoadContent(Game game)
    {
        throw new System.NotImplementedException();
    }

    public void Start(Game game)
    {
        throw new System.NotImplementedException();
    }

    public void SwitchTo(IScene gamme)
    {
        throw new System.NotImplementedException();
    }
}