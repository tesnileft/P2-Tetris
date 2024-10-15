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
    private BlockDefinition definition;
    public BlockGrid container;
    public Texture2D texture;
    
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
    struct BlockDefinition 
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
        int blocksize = 40;
        
        for (int x = 0; x < definition.Shape.GetLength(0); x++)
        {
            for (int y = 0; y < definition.Shape.GetLength(1); y++)
            {
                if (definition.Shape[x, y]) //Draw the square if it's in the block definition, otherwise obviously don't
                {
                    spriteBatch.Draw(
                        texture,
                        new Rectangle(x * blocksize + Position.X, y * blocksize + Position.Y, blocksize, blocksize), //Size and placement of the square
                        definition.Color
                        );
                }
            }
        }
    }
    public Block(BlockShape blockShape, BlockGrid grid)
    {
        bool[,] shape = new bool [4, 4];
        container = grid;
        definition = new BlockDefinition(shape, Color.Aqua);
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
        bool[,] rotated = new bool[definition.Shape.GetLength(0), definition.Shape.GetLength(1)];

        for (int x = 0; x < definition.Shape.GetLength(0); x++)
        {
            for (int y = 0; y < definition.Shape.GetLength(1); y++)
            {
                rotated[y, definition.Shape.GetLength(0) - x - 1] = definition.Shape[x, y];
            }
        }
        //Set the real rotation to the translated shape
        definition.Shape = rotated;
        //
    }

    //Cool collision code
    public bool CheckCollision(BlockGrid grid)
    {
        for (int x = 0; x < definition.Shape.GetLength(0); x++)
        { 
            for (int y = 0; y < definition.Shape.GetLength(1); y++)
            {
                if (definition.Shape[x, y] == true)
                {
                    if (grid.Grid[Position.X + x, Position.Y + y] != null)
                    {
                        
                    }
                    return true;
                }
            }
        }
        return false;
    }
    
    
    //Attempt to move the block down
    public void Tick()
    {
        
    }

   


}