using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace P_2_Tetris.Base;

public class InputHandler
{
    private KeyboardState _lastKeyboardState = Keyboard.GetState();
    HashSet<Keys> _pressedKeys = new();
    
    public void Update()
    {
        KeyboardState current = Keyboard.GetState();
        if (_lastKeyboardState != current)
        {
            //Keyboard did something!
            HashSet<Keys> newPressedKeys = new();
            HashSet<Keys> newUpKeys = new();
            KeyEventArgs e = new();
            
            foreach (Keys k in _pressedKeys)
            {
                if (!current.IsKeyDown(k))
                {
                    newUpKeys.Add(k);
                    _pressedKeys.Remove(k);
                }
            }
            
            foreach (Keys k in current.GetPressedKeys())
            {
                if (!_pressedKeys.Contains(k))
                {
                    newPressedKeys.Add(k);
                    _pressedKeys.Add(k);
                }
            }
            e.DownKeys = newPressedKeys;
            e.UpKeys = newUpKeys;
            
            EventHandler<KeyEventArgs> handler = KeyPress;
            if (handler != null)
            {
                handler(this, e);
            }
            
        }
        _lastKeyboardState = current;

    }
    public event EventHandler<KeyEventArgs> KeyPress;

    public class KeyEventArgs : EventArgs
    {
        public HashSet<Keys> UpKeys;
        public HashSet<Keys> DownKeys;
    }
    
}