using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Base.Ui;

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