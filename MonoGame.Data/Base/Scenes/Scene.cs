using System;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace MonoGame.Data;

public class Scene : Entity, IScene
{
    [JsonIgnore]
    public Game Game { get; set; }
    
    [JsonIgnore]
    public InitialisationState State { get; private set; } = InitialisationState.NotRunning;

    public override Rectangle BoundingBox => new(0, 0, Game.GraphicsDevice.Viewport.Width,
        Game.GraphicsDevice.Viewport.Height);
    
    public override void Initialise(Game game)
    {
        State = InitialisationState.Initialising;
        
        base.Initialise(game);

        State = InitialisationState.Initialised;
    }

    public override void LoadContent(Game game)
    {
        State = InitialisationState.Loading;
            
        foreach (var entity in Children)
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
        foreach (var entity in Children)
        {
            entity.Update(gameTime);
        }
    }

    public override void Draw()
    {
        foreach (var entity in Children)
        {
            entity.Draw();
        }
    }

    public override void Dispose()
    {
        Parallel.ForEach(Children, e => e.Dispose());
        GC.SuppressFinalize(this);
    }
}