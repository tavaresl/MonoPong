using Microsoft.Xna.Framework.Input;
using MonoGame.Data.Drawing.GUI;
using MonoGame.Data.Scripting;

namespace MonoGame.Core.Scripts.Components;

public class GameOver : ScriptableComponent
{
    public override void Initialise()
    {
        if (Entity.TryGetChild("RestartButton", out var restart))
            if (restart.TryGetComponent<Button>(out var restartButton))
                restartButton.Release += HandleRestartButtonRelease;
    
        if (Entity.TryGetChild("QuitButton", out var quit))
            if(quit.TryGetComponent<Button>(out var quitButton))
                quitButton.Release += HandleQuitButtonRelease;
    }

    private void HandleRestartButtonRelease(object _, MouseState mouseState)
    {
        Entity.Enabled = false;
    }

    private void HandleQuitButtonRelease(object _, MouseState mouseState)
    {
        Game.Exit();
    }
}