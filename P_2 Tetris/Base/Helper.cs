namespace P_2_Tetris.Base;

public static class Helper
{
    //Helper function to copy an array
    public static bool[,] CopyArray(bool[,] original)
    {
        int rows = original.GetLength(0);
        int columns = original.GetLength(1);
        bool[,] copy = new bool[rows, columns];
        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < columns; y++)
            {
                copy[x, y] = original[x, y];
            }
        }
        return copy;
    }
}