using Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace P_2_Tetris;

public class BlockGrid : GameObject
{
    public GridCell[,] Grid;

    public class GridCell
    {
        public Texture2D sprite;
        public Color color;
    }
    //Insert a block into the grid after it's detected to have gotten stuck
    public void InsertBlock(Block block)
    {
        
    }

    
    private void ClearLine()
    {
        
    }
}