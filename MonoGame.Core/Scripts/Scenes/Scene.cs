using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MonoGame.Core.Scripts.Entities;
using MonoGame.Core.Scripts.Systems;

namespace MonoGame.Core.Scripts.Scenes;

public abstract class Scene(Game1 game) : Entity, IScene
{
    public Game1 Game { get; private set; } = game;
    public InitialisationState State { get; private set; } = InitialisationState.NotRunning;
    public IList<IEntity> Entities { get; protected set; } = [];
    public IList<ISystem> Systems { get; protected set; } = [];
    public override Rectangle BoundingBox => new(0, 0, Game.GraphicsDevice.Viewport.Width,
        Game.GraphicsDevice.Viewport.Height);
    
    public override void Initialise(Game game)
    {
        State = InitialisationState.Initialising;
        
        foreach (var entity in Entities)
        {
            entity.Initialise(game);
        }

        State = InitialisationState.Initialised;
    }

    public override void LoadContent(Game game)
    {
        State = InitialisationState.Loading;
            
        foreach (var entity in Entities)
        {
            entity.LoadContent(game);
        }

        State = InitialisationState.Ready;
    }

    public void Start()
    {
        if (State != InitialisationState.Ready)
            throw new InvalidOperationException("cannot start a not-ready scene");

        State = InitialisationState.Starting;
        
        // Startup Logic
        Initialise(Game);
        LoadContent(Game);

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

    public override void Update(GameTime gameTime)
    {
        foreach (var entity in Entities)
        {
            entity.Update(gameTime);
        }
    }

    public override void Draw()
    {
        foreach (var entity in Entities)
        {
            entity.Draw();
        }
    }

    public void Open(IScene scene)
    {
        Stop();
        Game.ActiveScene = scene;
        scene.Start();
    }

    public override void Dispose()
    {
        GC.SuppressFinalize(this);
        Parallel.ForEach(Entities, e => e.Dispose());
    }
}