namespace MonoGame.Core.Scripts.Events;

public class GameEvents
{
    #region Game Events
    
    public const string GamePaused = "Paused";
    public const string GameResumed = "Resumed";

    #endregion
    
    #region Match Events

    public const string MatchStarted = "MatchStarted";
    public const string MatchRestarted = "MatchRestarted";
    public const string MatchEnded = "MatchEnded";

    #endregion

    #region Score Events

    public const string Scored = "Scored";
    
    #endregion
}