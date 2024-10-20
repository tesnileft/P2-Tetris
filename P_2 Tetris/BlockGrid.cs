using System;
using System.Collections.Generic;
using Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using P_2_Tetris.Base;

namespace P_2_Tetris;

public class BlockGrid : GameObject
{
    public GridCell[,] Grid;
    private Texture2D _tileSprite;
    public int TileSize = 32;
    public Point offset;
    private Point _blockOffset;
    Texture2D gridTexture;
    
    private Block _currentBlock;
    private Block _nextBlock;
    private bool _readyToSpawn = false;
    List<Block> _blocksBag = new List<Block>();
    
    private InputHandler _input;
    
    private KeyDown dropKeyDown = new();
    private KeyDown leftKeyDown = new();
    private KeyDown rightKeyDown = new();
    
    List<KeyDown> keyDowns = new List<KeyDown>();

    //Struct to keep track of keys being held down
    class KeyDown
    {
        public bool Down;
        public double MsTime;
        public double MsTimeSinceTick;

        private double _msTimeThreshold = 300;
        private double _msTickThreshold = 100;
        
        public Action Method;
        public KeyDown()
        {
            Down = false;
            MsTime = 0;
            MsTimeSinceTick = 0;
        }

        public void Increment(double ms)
        {
            if (!Down)
            {
                return;
            }
            MsTime += ms;
            if (MsTime >= _msTimeThreshold)
            {
                MsTimeSinceTick += ms;
                if (MsTimeSinceTick >= _msTickThreshold)
                {
                    MsTimeSinceTick = 0;
                    Method.Invoke();
                }
            }
        }
        public void Reset()
        {
            Down = false;
            MsTime = 0;
            MsTimeSinceTick = 0;
        }
    }
    public class GridCell
    {
        public Texture2D Sprite;
        public Color Color;
        public GridCell(Color c, Texture2D s)
        {
            Color = c;
            Sprite = s;
        }
    }

    public BlockGrid(int x, int y, Texture2D sprite, Texture2D gridTexture)
    {
        this.gridTexture = gridTexture;
        offset = new Point(0, TileSize * 3);
        
        _tileSprite = sprite;
        Grid = new GridCell[x,y];
        _input = new InputHandler();
        
        ResetBag();
        _nextBlock = TakeBlock();
        SpawnBlock();
        _input.KeyPress += HandleInput;
        
        dropKeyDown.Method = () => _currentBlock.Tick();
        leftKeyDown.Method = () => _currentBlock.Move(false);
        rightKeyDown.Method = () => _currentBlock.Move(true);
        
        keyDowns.Add(dropKeyDown);
        keyDowns.Add(leftKeyDown);
        keyDowns.Add(rightKeyDown);
        
    }

    public override void Update(GameTime gameTime)
    {
        /* Spawn blocks
            Move blocks down
            Check block collision
            Place blocks into grid
            Reset bag
        */
        
        _input.Update();
        
        foreach (KeyDown kd in keyDowns)
        {
            kd.Increment(gameTime.ElapsedGameTime.TotalMilliseconds);
        }
        
        _currentBlock.Update(gameTime);
    }
    
    void ResetBag()
    {
        _blocksBag = new List<Block>();
        foreach (Block.BlockShape shape in Enum.GetValues(typeof(Block.BlockShape)))
        {
            _blocksBag.Add(new Block(shape, this, _tileSprite));
        }
    }

    public void SpawnBlock()
    {
        _currentBlock = _nextBlock;
        _nextBlock = TakeBlock();
        _currentBlock.Position = new Point(Grid.GetLength(0)/2, -3);
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
    
    
    void HandleInput(object o, InputHandler.KeyEventArgs e)
    {
        foreach (Keys k in e.DownKeys)
        {
            switch (k)
            {
                case GameSettings.RotateKey:
                    //Rotate
                    _currentBlock.Rotate();
                    break;
                case GameSettings.DropKey:
                    //Drop down (With DAS)
                    _currentBlock.Tick();
                    dropKeyDown.Down = true;
                    break;
                //Moving left & right with DAS
                case GameSettings.LeftKey:
                    _currentBlock.Move(false);
                    leftKeyDown.Down = true;
                    break;
                case GameSettings.RightKey:
                    _currentBlock.Move(true);
                    rightKeyDown.Down = true;
                    break;
                
            }
        }

        foreach (Keys k in e.UpKeys)
        {
            switch (k)
            {
                case GameSettings.DropKey:
                    dropKeyDown.Reset();
                    break;
                case GameSettings.LeftKey:
                    leftKeyDown.Reset();
                    break;
                case GameSettings.RightKey:
                    rightKeyDown.Reset();
                    break;
                
            }
        }
    }
    
    //Insert a block into the grid after it's detected to have gotten stuck
    public void InsertBlock(Block b)
    {
        for (int x = 0; x < b.Definition.Shape.GetLength(0); x++)
        {
            for (int y = 0; y < b.Definition.Shape.GetLength(1); y++)
            {
                if (b.Definition.Shape[y, x])
                {
                    //Get the coordinate of where we need to insert a new gridcell
                    Point coord = b.Position + new Point(x, y);
                    Grid[coord.X, coord.Y] = new GridCell(b.Color, _tileSprite);
                }
            }
        }

        bool[] clearLines = CheckClearLine();
        if (clearLines.Length > 0)
        {
            ClearLine(clearLines);
        }
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(gridTexture, new Rectangle(offset, new Point(TileSize * 12, TileSize * 22)), Color.White);
        //loop over all cells in the grid to render them if there's something present there
        for(int x = 0; x < Grid.GetLength(0); x++)
        {
            for (int y = 0; y < Grid.GetLength(1); y++)
            {
                GridCell cell = Grid[x, y];
                if (cell != null)
                {
                    spriteBatch.Draw(cell.Sprite, new Rectangle(new Point(x,y) * new Point(TileSize) + offset, new Point(TileSize)), cell.Color);
                }
            }
        }
        _currentBlock.Draw(spriteBatch);
    }

    //Check if any lines can be cleared, middle step for animating something nice to happen
    private bool[] CheckClearLine()
    {
        bool[] lines = new bool[Grid.GetLength(1)];
        
        for (int y = Grid.GetLength(1) -1; y >= 0 ; y--)
        {
            for (int x = 0; x < Grid.GetLength(0); x++)
            {
                if (Grid[x, y] == null) //If a line is missing a cell, it's not a full line, so just skip checking the rest
                {
                    break;
                }
                //If the last cell is filled, it's a full line so mark for clearing
                if (x == Grid.GetLength(0) -1)
                {
                    lines[y] = true;
                }
            }
        }
        return lines;
    }
    //Clear an array of lines, and compact the blocks again and add score for what lines got cleared
    private void ClearLine(bool[] lines)
    {
        int consecutiveLines = 0;
        int totalLines = 0;
        for (int y = Grid.GetLength(1) - 1; y >= 0; y--)
        {
            if (lines[y])
            {
                consecutiveLines++;
                totalLines++;
                //Clear the line
                for (int x = 0; x < Grid.GetLength(0); x++)
                {
                    Grid[x, y] = null;
                }
            }
            else
            {
                //TODO Score
                consecutiveLines = 0;
                
                //Move the line down by the amount of total lines counted that have been cleared
                if (totalLines > 0)
                {
                    for (int x = 0; x < Grid.GetLength(0); x++)
                    {
                        Grid[x, y + totalLines] = Grid[x, y];
                        Grid[x, y] = null;
                    }
                }
            }
        }
    }
}