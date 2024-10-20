using System;
using System.Collections.Generic;
using Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using P_2_Tetris.Base;

namespace P_2_Tetris;

public partial class BlockGrid
{
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
        if (GameOverBool)
        {
            return;
        }
        
        _currentBlock = _nextBlock;
        _nextBlock = TakeBlock();
        
        _currentBlock.Offset = TileOffset;
        _nextBlock.Position = new Point(1, 5);
        
        _nextBlock.Offset = TileOffset + new Point(-6 * TileSize, 4 * TileSize);
        _currentBlock.Position = new Point(Grid.GetLength(0)/2-2, 1);
    }

    Block TakeBlock()
    {
        if (_blocksBag.Count <= 0)
        {
            ResetBag();
        }
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
                    b.Definition.Shape[y, x] = false;
                    Point coord = b.Position + new Point(x, y);

                    if (coord.Y < 5)
                    {
                        //Trying to place outside of the grid!! Meaning you lose!
                        LoseGame(b);
                        break;
                    }
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

    void LoseGame(Block b)
    {
        Console.WriteLine("Game Over");
        GameOver.Invoke(this, EventArgs.Empty);
        GameOverBool = true;
        _nextBlock = null;

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