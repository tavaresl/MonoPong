using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Core;
using MonoGame.Core.Drawing.GUI;
using MonoGame.Game.Scripts.Components;

namespace MonoGame.Game.Scripts.Systems;

public class PauseScreenController(Microsoft.Xna.Framework.Game game) : GameSystem<PauseScreen>(game)
{
    public override void Initialise(PauseScreen component)
    {
        if (component.Entity.TryGetChild("ResumeButton", out var resume))
            if (resume.TryGetComponent<Button>(out var resumeButton))
                resumeButton.Release += GetOnResumeHandler(component.Entity);
        
        if (component.Entity.TryGetChild("QuitButton", out var quit))
            if(quit.TryGetComponent<Button>(out var quitButton))
                quitButton.Release += Quit;

        component.Initialised = true;
    }

    private EventHandler<MouseState> GetOnResumeHandler(IEntity pauseScreen)
    {
        return (o, state) => pauseScreen.Enabled = false;
    }

    private void Quit(object sender, MouseState mouseState)
    {
        Game.Exit();
    }
}