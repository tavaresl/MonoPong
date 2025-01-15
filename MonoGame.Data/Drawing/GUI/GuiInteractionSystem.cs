using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoGame.Data.Drawing.GUI;

public class GuiInteractionSystem(Game game) : GameSystem<InteractiveGuiComponent>(game)
{
    public override void Update(InteractiveGuiComponent component, GameTime gameTime)
    {
        var mouseState = Mouse.GetState();

        if (component.Bounds.Contains(mouseState.Position))
        {
            Mouse.SetCursor(MouseCursor.Hand);
            if (!component.Hovered)
            {
                component.OnMouseEnter(mouseState);
                component.Hovered = true;
            }

            if (mouseState.LeftButton == ButtonState.Pressed && !component.Clicked)
            {
                component.OnClick(mouseState);
                component.Clicked = true;
            }

            if (mouseState.LeftButton == ButtonState.Released && component.Clicked)
            {
                component.Clicked = false;
            }
        }
        else
        {
            if (component.Hovered)
            {
                Mouse.SetCursor(MouseCursor.Arrow);
                component.OnMouseLeave(mouseState);
                component.Hovered = false;
            }
        }

        if (component.Clicked && mouseState.LeftButton == ButtonState.Released)
        {
            component.Clicked = false;
        }
    }
}