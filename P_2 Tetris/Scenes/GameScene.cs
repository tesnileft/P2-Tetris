using Base;

namespace P_2_Tetris.Scenes;

public class GameScene : Scene
{
    BlockGrid grid;

    public override void Init()
    {
        grid = new BlockGrid();
        
    }
}