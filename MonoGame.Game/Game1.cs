using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Core;
using MonoGame.Core.Collision;
using MonoGame.Core.Drawing;
using MonoGame.Core.Drawing.GUI;
using MonoGame.Core.Drawing.Sprites;
using MonoGame.Core.Scripting;
using MonoGame.Core.Utils.Extensions;
using MonoGame.Game.Scripts.Systems;

namespace MonoGame.Game;

public class Game1 : Microsoft.Xna.Framework.Game
{
    public Scene ActiveScene { get; private set; }
    public GraphicsSettings GraphicsSettings { get; private set; }

    private readonly List<KeyValuePair<float, string>> _layers = [
        new (0f, "Main"),
        new (1f, "Overlay Scene"),
        new (2f, "Overlay Scene GUI")
    ];

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

        foreach (var (layer, _) in _layers.OrderBy(kvp => kvp.Key))
        {
            DrawingGameSystem.Layer = layer;
            base.Draw(gameTime);   
        } 
    }

    public void OpenScene(Scene scene)
    {
        scene.Game = this;
        if (!scene.Started) scene.Start();
        ActiveScene.Stop();
        ActiveScene = scene;
    }

    private void LoadSystems()
    {
        var collisionRegions = new CollisionRegions<Collider>();
        
        Components.Add(new CollisionRegionsSystem(this) { Regions = collisionRegions });
        Components.Add(new ScriptableComponentRunner(this));
        Components.Add(new MatchController(this));
        Components.Add(new BallController(this));
        Components.Add(new EnemyController(this));
        Components.Add(new PlayerController(this));
        Components.Add(new PaddleCollisionController(this));
        Components.Add(new PauseScreenController(this));
        Components.Add(new GuiInteractionSystem(this));
        Components.Add(new CollisionDetector(this) { Regions = collisionRegions });
        Components.Add(new TextureDrawingSystem(this));
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