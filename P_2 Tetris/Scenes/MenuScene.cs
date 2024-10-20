using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Base;
using Base.Ui;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace P_2_Tetris.Scenes;
public class MenuScene : Scene
{
        private Ui menuUi;
        private int windowX;
        private int windowY;
        TetrisGame Game = TetrisGame.Instance;
        public MenuScene()
        {
            windowX = Game.Window.ClientBounds.Width;
            windowY = Game.Window.ClientBounds.Height;
        }

        public override void LoadContent(ContentManager Content)
        {
            List<UiElement> uiElements = new ();
            
            
            SpriteFont font = Content.Load<SpriteFont>("Ui/Font/FontSmaller");
            TextElement instructionsText = new TextElement(
                "arrow keys for movement\n" +
                "Z for rotation",
                font,
                new Rectangle(50, windowY/2 - 40 ,20 ,20 )
            );
                
            
            uiElements.Add(instructionsText);
            
            Texture2D splash = Content.Load<Texture2D>("UI/TetrisSplash");
            ImageElement splashImage = new ImageElement(splash, new Rectangle(50, 80, 700, 120));
            uiElements.Add(splashImage);
            
            Texture2D highlightSpriteLarge = Content.Load<Texture2D>(@"UI/BigButtonHighLight");
            //1 Player start button
            Texture2D onePlayerSprite = Content.Load<Texture2D>("UI/1P_Play");
            
            Button onepButton = new (
                new Vector2(windowX/2 - 300, 400),
                new Vector2(200),
                onePlayerSprite,
                highlightSpriteLarge
            );
            onepButton.ButtonDown += (obj, args) =>
            {
                Game.ChangeScene(new GameScene());
            };
            uiElements.Add(onepButton);
            
            //2 Player start button
            Texture2D twoPlayerSprite = Content.Load<Texture2D>("UI/2P_Play");
            
            Button twopButton = new (
                new Vector2(windowX/2-50, 400),
                new Vector2(200),
                twoPlayerSprite,
                highlightSpriteLarge
                );
            twopButton.ButtonDown += (obj, args) =>
            {
                Game.ChangeScene(new GameScene());
            };
            uiElements.Add(twopButton);
            
            //Git link button
            Texture2D highlightSpriteSmall = Content.Load<Texture2D>(@"UI/Highlight_small");
            Texture2D gitSprite = Content.Load<Texture2D>(@"UI/Git");
            Button gitButton = new(
                new Vector2(windowX - 40, windowY - 40), //Bottom right corner
                new Vector2(40),
                gitSprite,
                highlightSpriteSmall
            );
            gitButton.ButtonDown += (sender, args) =>
            {
                string url = "https://github.com/tesnileft/P2-Tetris";
                try
                {
                    Process.Start(url);
                }
                catch
                {
                    // hack because of this: https://github.com/dotnet/corefx/issues/10361
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    {
                        url = url.Replace("&", "^&");
                        Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
                    }
                    else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    {
                        Process.Start("xdg-open", url);
                    }
                    else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                    {
                        Process.Start("open", url);
                    }
                    else
                    {
                        throw;
                    }
                }
            };
            uiElements.Add(gitButton);
            
            //Exit button
            Texture2D exitSprite = Content.Load<Texture2D>(@"UI/X");
            Button exitButton = new(
                new Vector2(windowX - 40, 0), //Top right corner
                new Vector2(40),
                exitSprite,
                highlightSpriteSmall
            );
            exitButton.ButtonDown += (sender, args) =>
            {
                Game.Exit(); //Bah bye!! -Famous plumber Mario Mario
            };
            uiElements.Add(exitButton);
            
            //Settings button
            Texture2D settingsSprite = Content.Load<Texture2D>(@"UI/Settings");
            Button settingsButton = new(
                new Vector2(windowX - 120, windowY - 40), //Top right corner
                new Vector2(40),
                settingsSprite,
                highlightSpriteSmall
            );
            settingsButton.ButtonDown += (sender, args) =>
            {
                Game.ChangeScene(new SettingsScene());
            };
            uiElements.Add(settingsButton);
            menuUi = new (uiElements.ToArray());
        }
        
        public override void Init()
        {
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            menuUi.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            //Handle controls
            menuUi.Update(gameTime);
            base.Update(gameTime);
        }
    }