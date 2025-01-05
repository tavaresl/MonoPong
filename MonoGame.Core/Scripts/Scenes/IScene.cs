using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGame.Core.Scripts.Entities;
using MonoGame.Core.Scripts.Systems;

namespace MonoGame.Core.Scripts.Scenes;

public interface IScene : IEntity
{
    Game1 Game { get; }
    InitialisationState State { get; }
    IList<IEntity> Entities { get; }
    IList<ISystem> Systems { get; }
    
    void Start();
    void Stop();
    void Open(IScene scene);
}

public enum InitialisationState
{
    NotRunning,
    Initialising,
    Initialised,
    Loading,
    Ready,
    Starting,
    Started,
    Stopping,
}