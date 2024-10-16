using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Base.Ui
{
    public class Ui
    {
        protected UiElement[] Elements;
        public Ui(UiElement[] elements)
        {
            Elements = elements;
        }
        public void Update(GameTime gameTime)
        {
            foreach (UiElement b in Elements)
            {
                b.Update(gameTime);
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (UiElement b in Elements)
            {
                b.Draw(spriteBatch);
            }
        }
    }
    /// <summary>
    /// Base class for UI elements, like buttons, text, etc.
    /// </summary>
    public abstract class UiElement
        {
            public Point Position;
            public Point Size;

            void SetPos(Point newPosition)
            {
                Position = newPosition;
            }
            public abstract void Draw(SpriteBatch spriteBatch);

            public virtual void Update(GameTime gameTime)
            {
                
            }
        }
        
        /// <summary>
        /// Displays a string in the given font
        /// </summary>
        public class TextElement : UiElement
        {
            public string Text { get; set; }
            public SpriteFont Font;
            public TextElement(string text, SpriteFont font, Rectangle rect)
            {
                Text = text;
                Font = font;
                Size = rect.Size;
                Position = rect.Location;
            }
            public override void Draw(SpriteBatch spriteBatch)
            {
                spriteBatch.DrawString(Font, Text, Position.ToVector2(), Color.White);
            }
        }
        public class ImageElement : UiElement
        {
            public Texture2D Texture;
            public ImageElement(Texture2D img, Rectangle rect)
            {
                Texture = img;
                Size = rect.Size;
                Position = rect.Location;
            }
            public override void Draw(SpriteBatch spriteBatch)
            {
                spriteBatch.Draw(Texture, new Rectangle(Position, Size), Color.White);
            }
        }
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
        /// <summary>
        /// TODO
        /// </summary>
        public class Toggle : UiElement
        {
            public override void Draw(SpriteBatch spriteBatch)
            {
                throw new NotImplementedException();
            }
            public override void Update(GameTime gameTime)
            {
                throw new NotImplementedException();
            }
        }
}


