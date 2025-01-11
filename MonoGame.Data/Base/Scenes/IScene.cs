using Microsoft.Xna.Framework;

namespace MonoGame.Data;

public interface IScene : IEntity
{
    InitialisationState State { get; }
    
    void Start();
    void Stop();
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