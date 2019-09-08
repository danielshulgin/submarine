public static class CellularAutomaton 
{
    public static int[,] ConvertCellToPointArray(CellType[,] cells)
    {
        var result = new int[cells.GetLength(0), cells.GetLength(1)];
        for (int x = 0; x < cells.GetLength(0); x++)
        {
            for (int y = 0; y < cells.GetLength(1); y++)
            {
                if ((cells[x, y] == CellType.Live) || (cells[x, y] == CellType.StrictlyLive))
                {
                    result[x, y] = 1;
                }
                else
                {
                    result[x, y] = 0;
                }
            }
        }
        return result;
    }

    public static CellType[,] CellFilter(int cellFilterCycles, CellType[,] initialCells)
    {
        for (int i = 0; i < cellFilterCycles; i++)
        {
            for (int row = 0; row <= initialCells.GetLength(1) - 1; row++)
            {
                for (int column = 0; column <= initialCells.GetLength(0) - 1; column++)
                {
                    initialCells[column, row] = PlaceWallLogic(column, row, initialCells);
                }
            }
        }
        return initialCells;
    }

    private static CellType PlaceWallLogic(int x, int y, CellType[,] cells)
    {
        int numWalls = GetAdjacentWalls(x, y, 1, 1, cells);

        if (cells[x, y] == CellType.StrictlyDead)
        {
            return CellType.StrictlyDead;
        }
        if (cells[x, y] == CellType.StrictlyLive)
        {
            return CellType.StrictlyLive;
        }

        if (cells[x, y] == CellType.Live)
        {
            if (numWalls >= 4)
            {
                return CellType.Live;
            }
            if (numWalls < 2)
            {
                return CellType.Dead;
            }

        }
        else
        {
            if (numWalls >= 5)
            {
                return CellType.Live;
            }
        }
        return CellType.Dead;
    }

    private static int GetAdjacentWalls(int x, int y, int scopeX, int scopeY, CellType[,] cells)
    {
        int startX = x - scopeX;
        int startY = y - scopeY;
        int endX = x + scopeX;
        int endY = y + scopeY;

        int iX = startX;
        int iY = startY;

        int wallCounter = 0;

        for (iY = startY; iY <= endY; iY++)
        {
            for (iX = startX; iX <= endX; iX++)
            {
                if (!(iX == x && iY == y))
                {
                    if (IsWall(iX, iY, cells))
                    {
                        wallCounter += 1;
                    }
                }
            }
        }
        return wallCounter;
    }

    private static bool IsWall(int x, int y, CellType[,] cells)
    {
        // Consider out-of-bound a wall
        if (IsOutOfBounds(x, y, cells))
        {
            return true;
        }

        if (cells[x, y] == CellType.Live || cells[x, y] == CellType.StrictlyLive)
        {
            return true;
        }

        if (cells[x, y] == CellType.Dead || cells[x, y] == CellType.StrictlyDead)
        {
            return false;
        }
        return false;
    }

    private static bool IsOutOfBounds(int x, int y, CellType[,] cells)
    {
        if (x < 0 || y < 0)
        {
            return true;
        }
        else if (x > cells.GetLength(0) - 1 || y > cells.GetLength(1) - 1)
        {
            return true;
        }
        return false;
    }
}


