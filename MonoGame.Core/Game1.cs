using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Core.Scripts.Systems;
using MonoGame.Data;
using MonoGame.Data.Drawing;
using MonoGame.Data.Utils.Extensions;
using MonoGame.Persistence.Scenes;

namespace MonoGame.Core;

public class Game1 : Game
{
    public Scene ActiveScene { get; private set; }
    public GraphicsSettings GraphicsSettings { get; private set; }

    public Game1()
    {
        GraphicsSettings = new GraphicsSettings(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        
        LoadSystems();
        ActiveScene = SceneManager.Load("Content/Scenes/Gameplay.json");

        if (ActiveScene == null)
        {
            Exit();
            return;
        }

        ActiveScene.Game = this;
        ActiveScene.Start();
        base.Initialize();
    }

    protected override void LoadContent()
    {
        // TODO: use this.Content to load your game content here
        // SceneManager.Save(ActiveScene, "Content/out.json");
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        
        // TODO: Add your update logic here
        
        var stack = new Stack<IEntity>();

        stack.Push(ActiveScene);

        while (stack.TryPop(out var entity))
        {
            if (!entity.Enabled)
            {
                foreach (var component in entity.Components) GameExtensions.Components.Remove(component);
                continue;
            }

            foreach (var component in entity.Components)
            {
                if (!component.Enabled)
                {
                    GameExtensions.Components.Remove(component);
                    continue;
                }

                if (component.Initialised) GameExtensions.Components.Add(component);
                else
                {
                    component.Initialise();
                    component.Initialised = true;
                }
            }
            
            foreach (var child in entity.Children)
                stack.Push(child);
        }
        
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        base.Draw(gameTime);
    }

    public void OpenScene(Scene scene)
    {
        ActiveScene.Stop();
        ActiveScene = scene;
        scene.Start();
    }

    private void LoadSystems()
    {
        Components.Add(new MatchManagement(this));
        Components.Add(new BallMovement(this));
        Components.Add(new PaddleMovement(this));
        Components.Add(new DrawingSystem(this));
    }
}