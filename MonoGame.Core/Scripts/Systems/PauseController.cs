using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Core.Scripts.Components;
using MonoGame.Data;
using MonoGame.Data.Drawing.GUI;

namespace MonoGame.Core.Scripts.Systems;

public class PauseManagement(Game game) : GameSystem<PauseScreen>(game)
{
    public override void Initialise(PauseScreen component)
    {
        if (component.Entity.TryGetChild("ResumeButton", out var resume))
            if (resume.TryGetComponent<Button>(out var resumeButton))
                resumeButton.Click += GetOnResumeHandler(component.Entity);
        
        if (component.Entity.TryGetChild("QuitButton", out var quit))
            if(quit.TryGetComponent<Button>(out var quitButton))
                quitButton.Click += Quit;

        component.Initialised = true;
    }

    private EventHandler<MouseState> GetOnResumeHandler(IEntity pauseScreen)
    {
        return (o, state) =>
        {
            Console.WriteLine("Resume button clicked");
            pauseScreen.Enabled = false;
        };
    }

    private void Quit(object sender, MouseState mouseState)
    {
        Game.Exit();
    }
}