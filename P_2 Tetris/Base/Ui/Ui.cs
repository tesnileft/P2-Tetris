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

        public Ui()
        {
            Elements = new UiElement[0];
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
}


