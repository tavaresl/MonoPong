namespace MonoGame.Data;

public interface IScene : IEntity
{
    bool Started { get; set; }
    bool Paused { get; set; }
    
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