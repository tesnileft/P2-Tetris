using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Base.Ui;

/// <summary>
/// Button. Handles being hovered over and can be subscribed to with <c>ButtonDown</c>
/// </summary>
public class Button : UiElement
{
    bool _buttonHover;
    private bool _buttonClickable = true;
    private string _textureName;
    Texture2D _buttonTexture;
    Texture2D _highlightTexture;
    MouseState _mousePrevious = Mouse.GetState();
    public event EventHandler ButtonDown;
    public Button(Vector2 positionV, Vector2 sizeV, Texture2D texture, Texture2D highlightTexture) : this(
        positionV,
        sizeV, texture)
    {

        this._highlightTexture = highlightTexture;
    }

    public Button(Vector2 positionV, Vector2 sizeV, Texture2D texture)
    {
        this.Position = positionV.ToPoint();
        this.Size = sizeV.ToPoint();
        this._buttonTexture = texture;
    }

    public Button(Point position, Point size, Texture2D texture)
    {
        this.Position = position;
        this.Size = size;
        _buttonTexture = texture;
    }
    public void Load(Game game)
    {
        _buttonTexture = game.Content.Load<Texture2D>(_textureName);
    }
    public override void Update(GameTime gameTime)
    {
        CheckHover(Mouse.GetState().Position);
        if (Mouse.GetState().LeftButton == ButtonState.Pressed &&
            _mousePrevious.LeftButton == ButtonState.Released && _buttonClickable && _buttonHover)
        {
            //Yay! button got clicked
            ButtonDown?.Invoke(this, EventArgs.Empty);
        }

        _mousePrevious = Mouse.GetState();
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_buttonTexture, new Rectangle(Position, Size), Color.White);
        if (_buttonHover && _highlightTexture != null)
        {
            spriteBatch.Draw(_highlightTexture, new Rectangle(Position, Size), Color.White);
        }
    }

    bool CheckHover(Point mousePos)
    {
        //Check if the mouse position is over the button
        if (mousePos.X > Position.X && 
            mousePos.X < Position.X + Size.X && 
            mousePos.Y > Position.Y &&
            mousePos.Y < Position.Y + Size.Y)
        {
            _buttonHover = true;
        }
        else
        {
            _buttonHover = false;
        }

        return _buttonHover;
    }
}