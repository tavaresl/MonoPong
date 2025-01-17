using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoGame.Core.Drawing.GUI;

public class GuiInteractionSystem(Game game) : GameSystem<InteractiveGuiComponent>(game)
{
    public override void Update(InteractiveGuiComponent component, GameTime gameTime)
    {
        var mouseState = Mouse.GetState();

        if (component.Bounds.Contains(mouseState.Position))
        {
            if (!component.Hovered)
            {
                component.OnMouseEnter(mouseState);
                component.Hovered = true;
            }

            switch (mouseState.LeftButton)
            {
                case ButtonState.Pressed when !component.Pressed:
                    component.OnPress(mouseState);
                    component.Pressed = true;
                    break;
                case ButtonState.Released when component.Pressed:
                    component.OnRelease(mouseState);
                    component.Pressed = false;
                    break;
            }
        }
        else
        {
            if (component.Hovered)
            {
                component.OnMouseLeave(mouseState);
                component.Hovered = false;
            }
        }

        if (component.Pressed && mouseState.LeftButton == ButtonState.Released)
        {
            component.OnRelease(mouseState);
            component.Pressed = false;
        }
    }
}