using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Core.Scripts.Systems;
using MonoGame.Data;
using MonoGame.Data.Collision;
using MonoGame.Data.Drawing.GUI;
using MonoGame.Data.Drawing.Sprites;
using MonoGame.Data.Events;
using MonoGame.Data.Scripting;
using MonoGame.Data.Utils.Extensions;

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
        ActiveScene = SceneManager.Load("Content/Scenes/Main.json");

        if (ActiveScene == null)
        {
            Exit();
            return;
        }
        
        ActiveScene.Game = this;
        ActiveScene.Start();
        BuildComponentList();
        base.Initialize();
    }
    
    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        
        // TODO: Add your update logic here
        BuildComponentList();
        
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
        var collisionRegions = new CollisionRegions<Collider>();
        
        Components.Add(new CollisionRegionsSystem(this) { Regions = collisionRegions });
        Components.Add(new ScriptableComponentRunner(this));
        Components.Add(new MatchManagement(this));
        Components.Add(new BallMovement(this));
        Components.Add(new AiMovementController(this));
        Components.Add(new PlayerController(this));
        Components.Add(new PaddleCollision(this));
        Components.Add(new PauseManagement(this));
        Components.Add(new GuiInteractionSystem(this));
        Components.Add(new CollisionDetector(this) { Regions = collisionRegions });
        Components.Add(new SpriteDrawingSystem(this));
        Components.Add(new GuiDrawingSystem(this));
    }

    private void BuildComponentList()
    {
        var stack = new Stack<IEntity>();

        stack.Push(ActiveScene);

        while (stack.TryPop(out var entity))
        {
            if (!entity.Enabled)
            {
                RemoveEntityComponents(entity);
                continue;
            }

            foreach (var component in entity.Components)
            {
                if (component.Enabled) GameExtensions.AddComponent(component);
                else GameExtensions.RemoveComponent(component);
            }
            
            foreach (var child in entity.Children)
                stack.Push(child);
        }

        GameExtensions.Flatten();
    }

    private static void RemoveEntityComponents(IEntity rootEntity)
    {
        var removalStack = new Stack<IEntity>();
        
        removalStack.Push(rootEntity);

        while (removalStack.TryPop(out var entity))
        {
            foreach (var component in entity.Components) GameExtensions.RemoveComponent(component);
            foreach (var child in entity.Children) removalStack.Push(child);
        }
    }
}