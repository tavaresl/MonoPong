using System;
using Microsoft.Xna.Framework;

namespace MonoGame.Data.Events;

internal static class GameSystemEventExtension
{
    private static readonly GameSystemEventsManager Events = new();

    public static void Publish(this GameComponent gameSystem, string topicName)
    {
        Events.Publish(topicName, gameSystem);
    }
    
    
    public static void Publish(this GameComponent gameSystem, string topicName, object data)
    {
        Events.Publish(topicName, gameSystem, data);
    }

    public static void Subscribe(this GameComponent _, string topicName, Action<GameSystemEvent> handler)
    {
        Events.Subscribe(topicName, handler);
    }
    
    public static void Unsubscribe(this GameComponent _, string topicName, Action<GameSystemEvent> handler)
    {
        Events.Unsubscribe(topicName, handler);
    }
}