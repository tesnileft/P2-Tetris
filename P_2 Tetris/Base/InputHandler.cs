using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace P_2_Tetris.Base;

public class InputHandler
{
    private KeyboardState _lastKeyboardState = new KeyboardState();
    public EventHandler KeyPress;
    public void Update()
    {
        if (_lastKeyboardState != Keyboard.GetState())
        {
            //Keyboard did something!
            
            
            KeyPress.Invoke(null, new KeyEventArgs());
            
        }
        _lastKeyboardState = Keyboard.GetState();
        
    }

    class KeyEventArgs : EventArgs
    {
        public List<Keys> UpKeys = new();
        public List<Keys> DownKeys = new();
        public KeyEventArgs(Keys key)
        {
            
        }
    }
    
}