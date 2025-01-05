using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Core.Scripts.Scenes;

namespace MonoGame.Core;

public class Game1 : Game
{
    public IScene ActiveScene { get; set; }
    public GraphicsSettings GraphicsSettings { get; private set; }
    public Queue<Action> DrawActions { get; } = new ();

    public Game1()
    {
        GraphicsSettings = new GraphicsSettings(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        ActiveScene = new Gameplay(this);
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        ActiveScene.Initialise(this);
        base.Initialize();
    }

    protected override void LoadContent()
    {
        // TODO: use this.Content to load your game content here
        ActiveScene.LoadContent(this);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        
        // TODO: Add your update logic here
        ActiveScene.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        ActiveScene.Draw();

        foreach (var drawAction in DrawActions)
        {
            drawAction.Invoke();            
        }
        
        DrawActions.Clear();
        base.Draw(gameTime);
    }
}