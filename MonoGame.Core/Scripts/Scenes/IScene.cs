using System;
using Microsoft.Xna.Framework;

namespace MonoGame.Core.Scripts.Scenes;

public interface IScene : IDisposable
{
    bool HasStarted { get; }
    
    void Initialise(Game game);
    void LoadContent(Game game);
    void Start(Game game);
    void SwitchTo(IScene scene);
}