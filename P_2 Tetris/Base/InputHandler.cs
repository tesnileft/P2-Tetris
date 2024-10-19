using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;

namespace P_2_Tetris.Base;

public class InputHandler
{
    private KeyboardState _lastKeyboardState = Keyboard.GetState();
    public EventHandler KeyPress = new EventHandler();
    HashSet<Keys> _pressedKeys = new();
    
    public void Update()
    {
        KeyboardState current = Keyboard.GetState();
        if (_lastKeyboardState != current)
        {
            //Keyboard did something!
            HashSet<Keys> newPressedKeys = new();
            HashSet<Keys> newUpKeys = new();
            
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
            KeyPress.Invoke(null, new KeyEventArgs(newPressedKeys, newUpKeys));
            
            
        }
        _lastKeyboardState = Keyboard.GetState();
        
    }

    public class KeyEventArgs : EventArgs
    {
        public HashSet<Keys> UpKeys;
        public HashSet<Keys> DownKeys;
        public KeyEventArgs(HashSet<Keys> keyDown, HashSet<Keys> keyUp)
        {
            DownKeys = keyDown;
            UpKeys = keyUp;
            
        }
    }
    
}