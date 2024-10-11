using System.Collections.Generic;
using System.Numerics;
using Microsoft.Xna.Framework;


namespace P_2_Tetris;

public class Block
{
    public Point Position; //Position in the grid 0,0 is top left
    private BlockDefinition definition; 
    
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
        private Color Color;
        bool[,] Shape;
        
        public BlockDefinition(bool[,] shape, Color color)
        {
            Color = color;
            Shape = shape;
        }

    }
    public Block(BlockShape blockShape)
    {
        bool[,] shape = new bool [4, 4];
        
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
        
        
    }

    //Cool collision code
    public void CheckCollision()
    {
        
    }
    //Attempt to move the block down
    public void Tick()
    {
        
    }
    
    
}