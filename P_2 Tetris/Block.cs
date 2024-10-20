using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices.Marshalling;
using Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using P_2_Tetris.Base;


namespace P_2_Tetris;

public class Block : GameObject
{
    public Point Position; //Position in the grid 0,0 is top left
    public BlockDefinition Definition;
    public Point Offset = new Point(0, 0);
    public BlockGrid container;
    public Texture2D texture;
    public Color Color = Color.Aqua;
    private int _floorkicks = 0;
    
    private double timeSinceLastTick;
    private double millisecondsPerTick = 1000;
    
    ///Dictionary that holds all the different Tetromino shapes
    static Dictionary<BlockShape, bool[,]> BlockDict = new ()
    {
        {BlockShape.O, new bool[,]
            {
                {false, false, false, false},
                {false, true, true, false},
                {false, true, true, false},
                {false, false, false, false}
            }
        },
        { BlockShape.I, new bool[,]
            { 
                {false, true, false, false},
                {false, true, false, false},
                {false, true, false, false},
                {false, true, false, false}
            }
        },
        { BlockShape.L, new bool[,]
            { 
                {false, true, false},
                {false, true, false},
                {false, true, true}
            }
        },
        { BlockShape.J, new bool[,]
            { 
                {false, true, false},
                {false, true, false},
                {true, true, false}
            }
        },
        { BlockShape.Z, new bool[,]
            { 
                {true, true, false},
                {false, true, true},
                {false, false, false}
            }
        },
        { BlockShape.S, new bool[,]
            { 
                {false, true, true},
                {true, true, false},
                {false, false, false}
            }
        },
        { BlockShape.T, new bool[,]
            { 
                {true, true, true},
                {false, true, false},
                {false, false, false}
            }
        }
    };
    //Dictionary storing the colors for the blocks
    private static Dictionary<BlockShape, Color> BlockColorDict = new()
    {
        { BlockShape.O, new Color(249, 249, 30) },
        { BlockShape.I, Color.Aqua},
        { BlockShape.J, new Color(91, 110, 225)},
        { BlockShape.S, new Color(140, 233, 77)},
        { BlockShape.L, new Color(240, 140, 38)},
        { BlockShape.T, new Color(240, 100 ,240)},
        { BlockShape.Z, new Color(220, 70 ,70)}
    };
    
    //Enum for all the different shapes the blocks can have
    public enum BlockShape
    {
        O,
        I,
        L,
        J,
        T,
        S,
        Z
    }
    //Stores color and shape of the block
    public struct BlockDefinition 
    {
        public Color Color;
        public bool[,] Shape;
        
        public BlockDefinition(bool[,] shape)
        {
            Shape = shape;  
        }
    }
    public override void Update(GameTime gameTime)
    {
        timeSinceLastTick += gameTime.ElapsedGameTime.TotalMilliseconds;
        if (timeSinceLastTick >= millisecondsPerTick)
        {
            //Every 1 second move the block down automatically
            Tick();
        }
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
        int tileSize = container.TileSize;
        Point offset = this.Offset;
        
        for (int x = 0; x < Definition.Shape.GetLength(0); x++)
        {
            for (int y = 0; y < Definition.Shape.GetLength(1); y++)
            {
                if (Definition.Shape[y, x]) //Draw the square if it's in the block definition, otherwise obviously don't
                {
                    int posX = (x + Position.X) * tileSize;
                    int posY = (y + Position.Y) * tileSize;
                    spriteBatch.Draw(
                        texture,
                        new Rectangle(posX + offset.X, posY + offset.Y, tileSize, tileSize), //Size and placement of the square
                        Color
                        );
                }
            }
        }
    }
    public Block(BlockShape blockShape, BlockGrid grid, Texture2D tex)
    {
        texture = tex;
        bool[,] shape = Helper.CopyArray(BlockDict[blockShape]);
        container = grid;
        Color = BlockColorDict[blockShape];
        Definition = new BlockDefinition(shape);
    }

    //Move block left/right
    public void Move(bool right)
    {
        if (right)
        {
            Position.X += 1;
            if (CheckCollision(container))
            {
                Position.X -= 1;
            }
        }
        else
        {
            Position.X -= 1;
            if (CheckCollision(container))
            {
                Position.X += 1;
            } 
        }
    }
    
    //Weee spinny
    public void Rotate(bool counterClockwise = false)
    {
        //Temporary rotated definition
        bool[,] rotated = new bool[Definition.Shape.GetLength(0), Definition.Shape.GetLength(1)];
        
        //Store original in case we need to un-do the rotation
        bool[,] original = Definition.Shape;
        Point originalPoint = new(Position.X, Position.Y);
        
        for (int x = 0; x < Definition.Shape.GetLength(0); x++)
        {
            for (int y = 0; y < Definition.Shape.GetLength(1); y++)
            {
                rotated[y, Definition.Shape.GetLength(0) - x - 1] = Definition.Shape[x, y];
            }
        }
        //Set the real rotation to the translated shape
        Definition.Shape = rotated;
        //Test if rotation can even happen, or if a wall- / floorkick is needed
        bool kicked = false;
        int[] xDirection = {0, 1, -1, 2, -2};
        foreach (int xOffset in xDirection)   //Wallkick loop
        {
            //Change point to check if we can wallkick to there
            Position.X = originalPoint.X + xOffset;
            if (!CheckCollision(container))
            {
                Position.X = originalPoint.X + xOffset;
                kicked = true;
                break;
            }
        }
        //TODO floorkick
        if (!kicked)
        {
            //Wall or floor kick didn't work, so we cannot change the rotation
            Position = originalPoint;
            Definition.Shape = original;
        }
    }

    //Cool collision code
    public bool CheckCollision()
    {
        return CheckCollision(container);
    }
    public bool CheckCollision(BlockGrid grid)
    {
        //Loop over all the squares of the tetromino
        for (int x = 0; x < Definition.Shape.GetLength(0); x++)
        { 
            for (int y = 0; y < Definition.Shape.GetLength(1); y++)
            {
                if (Definition.Shape[y, x] == true)
                {
                    //Temporary variables to hold important numbers for readability
                    int gridX = grid.Grid.GetLength(0);
                    int gridY = grid.Grid.GetLength(1);
                    int tileX = Position.X + x;
                    int tileY = Position.Y + y;
                    
                    if (gridX <= tileX || tileX < 0)
                    {
                        //Collision with wall
                        return true;
                    }
                    if (tileY < 0)
                    {
                        //Ignore, it's allowed to be "above" the grid
                        continue;
                    }
                    if (tileY >= gridY)
                    {
                        //Collision with the ground
                        return true; 
                    }
                    try
                    {
                        if (grid.Grid[tileX, tileY] != null)
                        {
                            //Return true if the tetromino is inside an existing square in the grid
                            return true;
                        }
                    }
                    catch
                    {
                        //Debug
                        Console.WriteLine($"Collision undefined. At: {tileX}, {tileY}");
                        return true;
                    }
                }
            }
        }
        return false;
    }
    
    
    //Attempt to move the block down, if it collides, insert it there, return true if inserted
    public bool Tick()
    {
        if (container.GameOverBool)
        {
            return false;
        }
        Position.Y += 1;
        timeSinceLastTick = 0;
        if (CheckCollision(container))
        {
            //Move up the tetromino by one to not insert it overlapping other blocks
            Position.Y -= 1;
            container.InsertBlock(this);
            //After it's been inserted, summon a new block
            container.SpawnBlock();
            return true;
        }
        return false;
    }

   


}