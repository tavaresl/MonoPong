using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace MonoGame.Data;

public class Scene : Entity, IScene
{
    [JsonIgnore]
    public InitialisationState State { get; private set; } = InitialisationState.Ready;

    public override Rectangle BoundingBox => new(0, 0, Game.GraphicsDevice.Viewport.Width,
        Game.GraphicsDevice.Viewport.Height);
    

    public void Start()
    {
        if (State != InitialisationState.Ready)
            throw new InvalidOperationException("cannot start a not-ready scene");

        State = InitialisationState.Starting;
        
        // Startup Logic
        State = InitialisationState.Initialising;
        var initialisationStack = new Stack<IEntity>([this]);

        Initialised = true;

        while (initialisationStack.TryPop(out var entity))
        {
            foreach (var component in entity.Components)
            {
                component.Entity = entity;
                component.Game = Game;
                
                if (!component.Enabled || component.Initialised || !entity.Enabled) continue;

                component.Initialise();
                component.Initialised = true;
            }

            foreach (var child in entity.Children)
            {
                child.Parent = this;
                child.Game = Game;
                initialisationStack.Push(child);
            }
        }
        
        State = InitialisationState.Started;
    }

    public void Stop()
    {
        if (State != InitialisationState.Started)
            throw new InvalidOperationException("cannot stop a non-started scene");

        State = InitialisationState.Stopping;
        
        // Shutdown logic

        State = InitialisationState.NotRunning;
    }

    public override void Dispose()
    {
        Parallel.ForEach(Children, e => e.Dispose());
        GC.SuppressFinalize(this);
    }
}