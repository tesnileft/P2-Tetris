using System;
using System.Collections.Generic;
using Base;
using Base.Ui;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using P_2_Tetris.Base;

namespace P_2_Tetris;

public partial class BlockGrid : GameObject
{
    public GridCell[,] Grid;
    private Texture2D _tileSprite;
    public int TileSize = 32;
    public Point Offset;
    public Point TileOffset;
    Texture2D gridTexture;
    private Random r = new(DateTime.Now.Millisecond);
    
    private Block _currentBlock;
    private Block _nextBlock;
    List<Block> _blocksBag = new List<Block>();
    
    public bool GameOverBool = false;
    public EventHandler GameOver;
    
    private InputHandler _input;
    
    private KeyDown dropKeyDown = new();
    private KeyDown leftKeyDown = new();
    private KeyDown rightKeyDown = new();
    
    List<KeyDown> keyDowns = new List<KeyDown>();

    private Ui _gameUi;
    //Struct to keep track of keys being held down
    class KeyDown
    {
        public bool Down;
        public double MsTime;
        public double MsTimeSinceTick;

        private double _msTimeThreshold = 200;
        private double _msTickThreshold = 50;
        
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
        Offset = new Point(0, 0);
        TileOffset = new Point(TileSize * 6, TileSize) + Offset;
        
        _tileSprite = sprite;
        Grid = new GridCell[x,y + 4];
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
        
        
        List<UiElement> gameUiElements = new ();
    }

    public override void Update(GameTime gameTime)
    {
        if (GameOverBool)
        {
            DoDeath();
            return;
        }
        _input.Update();
        
        foreach (KeyDown kd in keyDowns)
        {
            kd.Increment(gameTime.ElapsedGameTime.TotalMilliseconds);
        }
        _currentBlock.Update(gameTime);
    }
    
    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(gridTexture, new Rectangle(Offset, new Point(TileSize * (12 + 5), TileSize * (22 + 5) ) ), Color.White);
        //loop over all cells in the grid to render them if there's something present there
        for(int x = 0; x < Grid.GetLength(0); x++)
        {
            for (int y = 0; y < Grid.GetLength(1); y++)
            {
                GridCell cell = Grid[x, y];
                if (cell != null)
                {
                    spriteBatch.Draw(cell.Sprite, new Rectangle((new Point(x,y)) * new Point(TileSize) + TileOffset , new Point(TileSize)), cell.Color);
                }
            }
        }

        if (_nextBlock != null)
        {
            _nextBlock.Draw(spriteBatch);
        }

        if (_currentBlock != null)
        {
            _currentBlock.Draw(spriteBatch);
        }
    }

    void DoDeath()
    {
        
    }
    
    
}