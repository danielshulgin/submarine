public class Square
{
    public int[] values = new int[4];
    public Point[] points = new Point[8];
    /// <summary>
    ///  starts clockwise from bottom left corner
    /// </summary>
    public int Configuration {
        get => values[0] * 1000 +
                    values[1] * 100 +
                    values[2] * 10 +
                    values[3];
    }

    public Square(int[] values, Point[] positions)
    {
        this.values = values;
        this.points = positions;
    }

    public Square()
    {
    }
}
