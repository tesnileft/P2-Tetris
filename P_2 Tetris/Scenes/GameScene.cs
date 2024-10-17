using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using P_2_Tetris.Base;

namespace P_2_Tetris.Scenes;

public class GameScene : Scene
{
    BlockGrid grid;
    Texture2D tileTexture;
    private Block _currentBlock;
    private Block _nextBlock;
    private bool _readyToSpawn = false;
    List<Block> _blocksBag = new List<Block>();
    private double timeSinceLastTick;
    private double millisecondsPerTick = 1000;
    private InputHandler _input;
    public override void LoadContent(ContentManager content)
    {
        Texture2D weede = content.Load<Texture2D>("Weede");
        tileTexture = weede; //content.Load<Texture2D>("block");
    }
    public override void Init()
    {
        _input = new InputHandler();
        
        grid = new BlockGrid(6, 8, tileTexture);
        _objects.Add(grid);
        
        ResetBag();
        _nextBlock = TakeBlock();
        _currentBlock = TakeBlock();
    }

    public override void Update(GameTime gameTime)
    {
        /* Spawn blocks
            Move blocks down
            Check block collision
            Place blocks into grid
            Reset bag
        */
        timeSinceLastTick += gameTime.ElapsedGameTime.TotalMilliseconds;
        
        //Should be moved to the grid class tbh
        if (timeSinceLastTick >= millisecondsPerTick)
        {
            _currentBlock.Tick();
            timeSinceLastTick = 0;
            if (_currentBlock.CheckCollision(grid))
            {
                //Move up the tetromino by one to not insert it overlapping other blocks
                _currentBlock.Position.Y -= 1;
                grid.InsertBlock(_currentBlock);
                _currentBlock = _nextBlock;
                _nextBlock = TakeBlock();
            }
        }
        //Control stuff
        base.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        _currentBlock.Draw(spriteBatch);
        grid.Draw(spriteBatch);
    }

    void ResetBag()
    {
        _blocksBag = new List<Block>();
        foreach (Block.BlockShape shape in Enum.GetValues(typeof(Block.BlockShape)))
        {
            _blocksBag.Add(new Block(shape, grid, tileTexture));
        }
    }

    void SpawnBlock()
    {
        _currentBlock = _nextBlock;
        _nextBlock = TakeBlock();
        _currentBlock.Position = new Point(4, 0);
    }

    Block TakeBlock()
    {
        if (_blocksBag.Count <= 0)
        {
            ResetBag();
        }
        Random r = new(_blocksBag.Count);
        int randomIndex = r.Next(_blocksBag.Count);
        Block take = _blocksBag[randomIndex];
        _blocksBag.RemoveAt(randomIndex);
        return take;
    }
}