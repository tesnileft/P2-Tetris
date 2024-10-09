using System.Collections.Generic;
using System.Numerics;
using Microsoft.Xna.Framework;


namespace P_2_Tetris;

public class Block
{
    Point Position; //Position in the grid
    private BlockDefinition definition;
    
    static Dictionary<BlockShape, Matrix> blocks = new Dictionary<BlockShape, Matrix>();
    public enum BlockShape
    {
        Square,
        Long
    }

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
    Block(BlockShape blockShape)
    {
        bool[,] shape = new bool [4, 4];
        
        definition = new BlockDefinition(shape, Color.Aqua);
    }

    public Block MakeBlock(BlockShape blockShape)
    {
        return new Block(blockShape);
    }

    public void Move()
    {
        //Move block left/right
    }

    public void Drop()
    {
        //Drop it down fast n stuff
    }
    void Rotate(bool counterClockwise = false)
    {
        //Weee spinny
    }

    public void CheckCollision()
    {
        //Cool collision code
    }
    
    
}