using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Base;
using Base.Ui;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using P_2_Tetris.Base;

namespace P_2_Tetris.Scenes;

public class GameScene : Scene
{
    BlockGrid grid;
    Texture2D tileTexture;
    private Ui _pauseUi;
    private Ui _loseUi;
    private Ui _currentUi;
    bool paused = false;

    private TetrisGame Game;
    private SpriteFont font;
    
    public override void LoadContent(ContentManager content)
    {
        Game = TetrisGame.Instance;
        tileTexture = content.Load<Texture2D>("Game/Tile");
        Texture2D gridTexture = content.Load<Texture2D>("Game/Grid_10_20");
        grid = new BlockGrid(10, 20, tileTexture, gridTexture);
        
        grid.GameOver += GameOver;
        
        
        List<UiElement> elementsPaused = new ();
        
        
        
        
        List<UiElement> elementsLose = new();
        
        //End of game UI
        Rectangle window = Game.Window.ClientBounds;
        
        //"win" text
        font = content.Load<SpriteFont>("UI/Font/FontSmall");
        string endText = "GAME OVER!";
        Rectangle gameOverMessageRect = new(
            new Point(window.Width / 2 - (int)font.MeasureString(endText).X / 2, window.Height / 2 - 100),
            Point.Zero);
        TextElement textGameOver = new(endText, font, gameOverMessageRect);
        elementsLose.Add(textGameOver);
        
        //Back home button
        Texture2D homeSprite = content.Load<Texture2D>("UI/Home");
        Texture2D highlightSpriteSmall = content.Load<Texture2D>("UI/Highlight_small");
        Button goHomeButton = new(
                new Vector2(200,  window.Height / 2), 
                new Vector2(40),
                homeSprite,
                highlightSpriteSmall
            );
        goHomeButton.ButtonDown += (sender, args) =>
        {
            Game.ChangeScene(new MenuScene());
        };
        elementsLose.Add(goHomeButton);
        
        //Exit button
        Texture2D exitSprite = content.Load<Texture2D>("UI/X");
        Button exitButton = new(
            new Vector2(window.Width - 240, window.Height / 2), 
            new Vector2(40),
            exitSprite,
            highlightSpriteSmall
        );
        exitButton.ButtonDown += (sender, args) =>
        {
            
            Game.Exit();
        };
        elementsLose.Add(exitButton);
        
        //Reset/try button
        Texture2D retrySprite = content.Load<Texture2D>("UI/Retry");
        Button retryButton = new(
            new Vector2(window.Width - 240, window.Height / 2 + 80), 
            new Vector2(40),
            retrySprite,
            highlightSpriteSmall
        );
        retryButton.ButtonDown += (sender, args) =>
        {
            Game.ChangeScene(new GameScene());
        };
        elementsLose.Add(retryButton);
        _loseUi = new Ui(elementsLose.ToArray());
        


    }
    public override void Init()
    {
        _objects.Add(grid);
    }

    void GameOver(Object sender, EventArgs e)
    {
        _currentUi = _loseUi;
    }
    public override void Update(GameTime gameTime)
    {
        if (_currentUi != null)
        {
            _currentUi.Update(gameTime);
        }
        
        if (paused)
        {
            return;
        }
        
        
        grid.Update(gameTime);
        //base.Update(gameTime);
    }
    
    

    public override void Draw(SpriteBatch spriteBatch)
    {
        grid.Draw(spriteBatch);
        if (_currentUi != null)
        {
            _currentUi.Draw(spriteBatch);
        }
        
    }

    
}