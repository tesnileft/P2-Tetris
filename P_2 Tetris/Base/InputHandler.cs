using System;
using Microsoft.Xna.Framework.Input;

namespace P_2_Tetris.Base;

public class InputHandler
{
    private KeyboardState lastKeyboardState = new KeyboardState();
    public void Update()
    {
        if (lastKeyboardState != Keyboard.GetState())
        {
            
        }
        lastKeyboardState = Keyboard.GetState();
    }
}