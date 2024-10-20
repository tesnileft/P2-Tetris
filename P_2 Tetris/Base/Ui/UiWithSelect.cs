using P_2_Tetris.Base;

namespace Base.Ui;

public class UiWithSelect : Ui
{
    InputHandler _input;
    public UiWithSelect()
    {
        _input = new InputHandler();
        _input.KeyPress += KeypressHandler;
    }

    void KeypressHandler(object? sender, InputHandler.KeyEventArgs e)
    {
        switch (e.DownKeys)
        {
            
        }
    }
    
    
}