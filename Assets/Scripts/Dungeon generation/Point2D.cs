public struct Point2D
{
    private int _x;
    private int _y;

    public Point2D(int x, int y)
    {
        _x = x;
        _y = y;
    }

    public int X
    {
        get => _x;
        set => _x = value;
    }
    public int Y
    {
        get => _y;
        set => _y = value;
    }
}