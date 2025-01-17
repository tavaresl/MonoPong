using System;
using Microsoft.Xna.Framework.Input;

namespace MonoGame.Core.Drawing.GUI;

public abstract class GuiComponent : DrawableComponent
{
}

public abstract class InteractiveGuiComponent : GuiComponent
{
    public event EventHandler<MouseState> MouseEnter;
    public event EventHandler<MouseState> MouseLeft;
    public event EventHandler<MouseState> Press;
    public event EventHandler<MouseState> Release;

    public bool Hovered { get; internal set; }
    public bool Pressed { get; internal set; }
    
    internal virtual void OnMouseEnter(MouseState e) => MouseEnter?.Invoke(this, e);
    internal virtual void OnMouseLeave(MouseState e) => MouseLeft?.Invoke(this, e);
    internal virtual void OnPress(MouseState e) => Press?.Invoke(this, e);
    internal virtual void OnRelease(MouseState e) => Release?.Invoke(this, e);
}