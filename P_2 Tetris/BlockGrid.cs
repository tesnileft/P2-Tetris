using System;
using Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace P_2_Tetris;

public class BlockGrid : GameObject
{
    public GridCell[,] Grid;
    private Texture2D tileSprite;
    public int RenderOffset = 0;
    public int TileSize = 20;
    public class GridCell
    {
        public Texture2D Sprite;
        public Color Color;
        public Point Position;

        public GridCell(Color c, Texture2D s, Point point)
        {
            Position = point;
            Color = c;
            Sprite = s;
        }
    }

    public BlockGrid(int x, int y, Texture2D sprite)
    {
        tileSprite = sprite;
        Grid = new GridCell[x,y];
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
                    Point coord = b.Position + new Point(x, y);
                    Console.WriteLine($"Inserting at coord {coord}");
                    Grid[coord.X, coord.Y] = new GridCell(b.Color, tileSprite, coord);
                }
                
            }

        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        foreach (GridCell cell in Grid)
        {
            if (cell != null)
            {
                spriteBatch.Draw(cell.Sprite, new Rectangle(cell.Position * new Point(TileSize), new Point(TileSize)), cell.Color);
            }
        }
    }


    private void ClearLine()
    {
        
    }
}