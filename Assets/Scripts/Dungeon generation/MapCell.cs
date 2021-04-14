using System.Collections.Generic;

public class MapCell
{
    private RoomType _roomType;
    private int _extraCost;
    private List<Direction> _roomDirections = new List<Direction>();
    
    public MapCell(RoomType roomType = RoomType.None, int extraCost = 0)
    {
        _roomType = roomType;
        _extraCost = extraCost;
    }

    public List<Direction> RoomDirections => _roomDirections;

    public RoomType RoomType
    {
        get => _roomType;
        set => _roomType = value;
    }

    public int ExtraCost
    {
        get => _extraCost;
        set => _extraCost = value;
    }
}