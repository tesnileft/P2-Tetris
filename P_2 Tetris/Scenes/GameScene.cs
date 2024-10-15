using System;
using System.Collections.Concurrent;
using Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace P_2_Tetris.Scenes;

public class GameScene : Scene
{
    BlockGrid grid;
    Texture2D blockTexture;
    private Block _currentBlock;
    private Block _nextBlock;
    private bool _readyToSpawn = false;
    ConcurrentBag<Block> _blocksBag = new ConcurrentBag<Block>();
    private double timeSinceLastTick;
    private double millisecondsPerTick = 1000;
    public void LoadContent(ContentManager content)
    {
        Texture2D weede = content.Load<Texture2D>("Weede");
        blockTexture = weede; //content.Load<Texture2D>("block");
    }
    public override void Init()
    {
        grid = new BlockGrid();
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

        if (timeSinceLastTick >= millisecondsPerTick)
        {
            _currentBlock.Tick();
            timeSinceLastTick = 0;
            if (_currentBlock.CheckCollision(grid))
            {
                //Move up the tetromino by one to not insert it overlapping other blocks
                _currentBlock.Position.X += 1;
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
    }

    void ResetBag()
    {
        _blocksBag = new ConcurrentBag<Block>();
        foreach (Block.BlockShape shape in Enum.GetValues(typeof(Block.BlockShape)))
        {
            _blocksBag.Add(new Block(shape, grid));
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
        Block take = _blocksBag.TryTake(out Block block) ? block : null;
        if (take == null)
        {
            ResetBag();
            return TakeBlock();
        }
        return take;
    }
}