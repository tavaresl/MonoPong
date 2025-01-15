using System;
using Microsoft.Xna.Framework.Input;

namespace MonoGame.Data.Drawing.GUI;

public abstract class GuiComponent : DrawableComponent
{
}

public abstract class InteractiveGuiComponent : GuiComponent
{
    public event EventHandler<MouseState> MouseEnter;
    public event EventHandler<MouseState> MouseLeave;
    public event EventHandler<MouseState> Click;

    public bool Hovered { get; internal set; }
    public bool Clicked { get; internal set; }
    
    internal virtual void OnMouseEnter(MouseState e) => MouseEnter?.Invoke(this, e);
    internal virtual void OnMouseLeave(MouseState e) => MouseLeave?.Invoke(this, e);
    internal virtual void OnClick(MouseState e) => Click?.Invoke(this, e);
}