using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Base;
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
    public override void LoadContent(ContentManager content)
    {
        Texture2D weede = content.Load<Texture2D>("Weede");
        tileTexture = content.Load<Texture2D>("Game/Tile");
        Texture2D gridTexture = content.Load<Texture2D>("Game/Grid_10_20");
        grid = new BlockGrid(10, 20, tileTexture, gridTexture);
    }
    public override void Init()
    {
        _objects.Add(grid);
    }
    
    public override void Update(GameTime gameTime)
    {
        //grid.Update(gameTime);
        base.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        grid.Draw(spriteBatch);
    }

    
}