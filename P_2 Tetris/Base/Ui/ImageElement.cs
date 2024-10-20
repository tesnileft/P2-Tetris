using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Base.Ui;

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