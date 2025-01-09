using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Core.Scripts.Scenes;
using MonoGame.Data;
using MonoGame.Data.Utils.Extensions;
using MonoGame.Persistence.Scenes;

namespace MonoGame.Core;

public class Game1 : Game
{
    public IScene ActiveScene { get; private set; }
    public GraphicsSettings GraphicsSettings { get; private set; }

    public Game1()
    {
        GraphicsSettings = new GraphicsSettings(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        ActiveScene = new Gameplay { Game = this };
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        
        LoadSystems();
        ActiveScene.Initialise(this);
        
        base.Initialize();
    }

    protected override void LoadContent()
    {
        // TODO: use this.Content to load your game content here
        
        ActiveScene.LoadContent(this);
        SceneManager.Save(ActiveScene, "Content/out.json");
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

                GameExtensions.Components.Add(component);
            }
            
            foreach (var child in entity.Children)
                stack.Push(child);
        }
        
        
        ActiveScene.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        base.Draw(gameTime);
    }

    public void OpenScene(IScene scene)
    {
        ActiveScene.Stop();
        ActiveScene = scene;
        scene.Start();
    }

    private void LoadSystems()
    {
        var thisAssembly = typeof(Game1).Assembly;
        var dataAssembly = typeof(GameSystemAttribute).Assembly;

        foreach (var type in thisAssembly.GetTypes().Union(dataAssembly.GetTypes()).AsParallel())
        {
            var attributes = type.GetCustomAttributes(typeof(GameSystemAttribute), true);
            if (attributes.Length != 0)
            {
                var instance = Activator.CreateInstance(type, [this]);
                if (instance == null) continue;
                
                if (instance is GameComponent gameComponent)
                    Components.Add(gameComponent);
            }
        }
    }
}