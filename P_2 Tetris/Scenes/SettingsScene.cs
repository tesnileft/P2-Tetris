using System.Collections.Generic;
using Base;
using Base.Ui;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace P_2_Tetris.Scenes;

public class SettingsScene : Scene
{
    TetrisGame Game = TetrisGame.Instance;
    private Ui _settings;
    public SettingsScene()
    {
        
    }

    public override void Init()
    {
        
    }

    public override void LoadContent(ContentManager content)
    {
        List<UiElement> elements = new();
        
        SpriteFont font = content.Load<SpriteFont>(@"UI\Font\FontFile");
        TextElement elem = new(
            "here is where i would put my settings menu",
            font,
            new Rectangle(new Point(200), Point.Zero)
        );
        Rectangle window = Game.Window.ClientBounds;
        Texture2D homeSprite = content.Load<Texture2D>("Sprites/UI/Home");
        Texture2D highlightSpriteSmall = content.Load<Texture2D>("Sprites/UI/Highlight_small");
        Button goHomeButton = new(
            new Vector2(0,  0), 
            new Vector2(40),
            homeSprite,
            highlightSpriteSmall
        );
        goHomeButton.ButtonDown += (sender, args) =>
        {
            Game.ChangeScene(new MenuScene());
        };
        elements.Add(goHomeButton);
        
        elements.Add(elem);
        _settings = new(elements.ToArray());
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
        _settings.Draw(spriteBatch);
    }

    public override void Update(GameTime gameTime)
    {
        _settings.Update(gameTime);
    }
}