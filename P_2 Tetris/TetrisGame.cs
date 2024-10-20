using Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using P_2_Tetris.Scenes;

namespace P_2_Tetris;

public class TetrisGame : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    Scene CurrentScene;
    public static TetrisGame Instance;

    public TetrisGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferWidth = 800;
        _graphics.PreferredBackBufferHeight = 800;
        _graphics.ApplyChanges();
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        this.Window.AllowUserResizing = false;
    }

    protected override void Initialize()
    {
        Instance = this;
        base.Initialize();
    }

    protected override void LoadContent()
    {
        CurrentScene = new MenuScene();
        CurrentScene.LoadContent(Content);
        CurrentScene.Init();
        // ^This should all be handled by a scene changer but like who cares tbh
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        CurrentScene.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        GraphicsDevice.Clear(Color.Black);
        CurrentScene.Draw(_spriteBatch);
        _spriteBatch.End();
        base.Draw(gameTime);
    }

    public void ChangeScene(Scene newScene)
    {
        CurrentScene = newScene;
        newScene.LoadContent(Content);
        newScene.Init();
        
    }
}