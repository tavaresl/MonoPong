using System;
using System.Collections.Generic;
using System.Runtime.Loader;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace MonoGame.Data;

public class GameSystemEventBus
{
    private readonly Dictionary<string, HashSet<Action<GameSystemEvent>>> _topics = new ();

    public void Subscribe(string topic, Action<GameSystemEvent> handler)
    {
        if (!_topics.ContainsKey(topic)) _topics[topic] = [];
        _topics[topic].Add(handler);
    }
    
    public void Unsubscribe(string topic, Action<GameSystemEvent> handler)
    {
        if (_topics.TryGetValue(topic, out var handlers))
            handlers.Remove(handler);
    }

    
    public void Notify(string topic, IGameComponent sender, object data)
    {
        Parallel.ForEach(_topics[topic], handler => handler.Invoke(new GameSystemEvent
        {
            Sender = sender,
            Data = data,
        }));
    }
    
    public void Notify(string topic,  IGameComponent sender)
    {
        Parallel.ForEach(_topics[topic], handler => handler.Invoke(new GameSystemEvent
        {
            Sender = sender,
        }));
    }
}

public record struct GameSystemEvent
{
    public required IGameComponent Sender { get; init; }
    public object Data { get; init; }
}
