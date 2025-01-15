using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Core.Scripts.Components;
using MonoGame.Data;
using MonoGame.Data.Drawing.GUI;
using MonoGame.Data.Events;

namespace MonoGame.Core.Scripts.Systems;

public class PauseManagement(Game game) : GameSystem<PauseScreen>(game)
{
    public override void OnInitialise()
    {
        On("MatchEnded", Enable);
        Paused = true;
    }

    public override void Initialise(PauseScreen component)
    {
        var resumeButton = component.Entity.Parent.GetChild("ResumeButton").GetComponent<Button>();
        var quitButton = component.Entity.Parent.GetChild("QuitButton").GetComponent<Button>();
            
        resumeButton.Click += Resume;
        quitButton.Click += Quit;

        component.Initialised = true;
    }

    private void Enable(GameSystemEvent gameSystemEvent)
    {
        Paused = false;
    }
    
    private void Resume(object sender, MouseState mouseState)
    {
        Paused = true;
        Enabled = false;
        Notify("Restart");
    }

    private void Quit(object sender, MouseState mouseState)
    {
        Game.Exit();
    }
}