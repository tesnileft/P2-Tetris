using System.Collections.Generic;
using System.Numerics;
using Microsoft.Xna.Framework;


namespace P_2_Tetris;

public class Block
{
    Point Position;
    
    static Dictionary<BlockShape, Matrix> blocks = new Dictionary<BlockShape, Matrix>();
    enum BlockShape
    {
        Square,
        Long
    }

    struct BlockDefinition
    {
        private Color Color;
        

    }
    Block(BlockShape blockShape)
    {
        
    }

    void Rotate()
    {
        
    }
    
}