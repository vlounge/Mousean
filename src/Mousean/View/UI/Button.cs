using Microsoft.Xna.Framework;
using Mousean.Controller;

namespace Mousean.View.UI;

public class Button {
    public float Size = Constants.DefaultButtonSize;
    public Sprite Sprite;
    public bool Hide = true;
    public bool Pressed = false;
    public ButtonType Type;
    public Rectangle ActiveRect;
    public Rectangle HoverRect;
    public Rectangle PressedRect;
    public ButtonState State;
    
    public Button(ButtonType type)
    {
        //_spriteList = new List<Sprite>();
        Type = type;
        State = ButtonState.Active;
    }
}