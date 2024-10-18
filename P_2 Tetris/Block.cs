using System;
using System.Collections.Generic;
using System.Numerics;
using Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace P_2_Tetris;

public class Block : GameObject
{
    public Point Position; //Position in the grid 0,0 is top left
    public BlockDefinition Definition;
    public BlockGrid container;
    public Texture2D texture;
    public Color Color = Color.Aqua;
    
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
        
        public BlockDefinition(bool[,] shape, Color color)
        {
            Color = color;
            Shape = shape;  
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        int tileSize = container.TileSize;
        
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
                        new Rectangle(posX , posY, tileSize, tileSize), //Size and placement of the square
                        Definition.Color
                        );
                }
            }
        }
    }
    public Block(BlockShape blockShape, BlockGrid grid, Texture2D tex)
    {
        texture = tex;
        bool[,] shape = BlockDict[blockShape];
        container = grid;
        Definition = new BlockDefinition(shape, Color.Aqua);
    }

    //Move block left/right
    public void Move()
    {
        
    }

    //Drop it down fast n stuff
    public void Drop()
    {
        
    }
    
    //Weee spinny
    void Rotate(bool counterClockwise = false)
    {
        //Temporary rotated definition
        bool[,] rotated = new bool[Definition.Shape.GetLength(0), Definition.Shape.GetLength(1)];

        for (int x = 0; x < Definition.Shape.GetLength(0); x++)
        {
            for (int y = 0; y < Definition.Shape.GetLength(1); y++)
            {
                rotated[y, Definition.Shape.GetLength(0) - x - 1] = Definition.Shape[x, y];
            }
        }
        //Set the real rotation to the translated shape
        Definition.Shape = rotated;
        //
    }

    //Cool collision code
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

                    if (tileY <= 0)
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
    
    
    //Attempt to move the block down
    public void Tick()
    {
        Position.Y += 1;
    }

   


}