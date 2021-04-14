using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonMap
{
    private MapCell[,] _map;
    private Point2D _startingCell = new Point2D(15, 15);
    private List<Point2D> _mainPath = new List<Point2D>();
    private List<Point2D> _additionalPath = new List<Point2D>();

    public MapCell[,] Map => _map;

    public DungeonMap(MapInfo mapInfo)
    {
        // generate base map
        _map = GetStartMap();

        // generate main path
        for (var i = 0; i < mapInfo.MainPathsNumber; i++)
        {
            var roomCount = Random.Range(mapInfo.MINCellInMainPath, mapInfo.MAXCellInMainPath);
            var lastRoom = _startingCell;
            
            for (var j = 0; j < roomCount; j++)
            {
                var randomLowestCost = GetLowestCostCell(lastRoom);

                _map[randomLowestCost.X, randomLowestCost.Y].RoomType = RoomType.Simple;
                _map[randomLowestCost.X, randomLowestCost.Y].ExtraCost = 5;

                _map[lastRoom.X, lastRoom.Y].RoomDirections.Add(GetVectorDirection(new Vector2(randomLowestCost.X, randomLowestCost.Y) - new Vector2(lastRoom.X, lastRoom.Y)));
                _map[randomLowestCost.X, randomLowestCost.Y].RoomDirections.Add(GetVectorDirection(new Vector2(lastRoom.X, lastRoom.Y) - new Vector2(randomLowestCost.X, randomLowestCost.Y)));
                
                _mainPath.Add(randomLowestCost);
                lastRoom = randomLowestCost;

                // checks for last iteration
                if (j != roomCount - 1) continue;
                
                // change cost for neighbours of last cell in path so additionals does not start there
                _map[randomLowestCost.X, randomLowestCost.Y].ExtraCost = 6;
                var neighbours = GetCellNeighbours(randomLowestCost);
                
                foreach (var cell in neighbours)
                {
                    _map[cell.X, cell.Y].ExtraCost = 4;
                }
                
                // check for last last iteration
                if (i != mapInfo.MainPathsNumber - 1) continue;
                
                // spawn boss room
                _map[randomLowestCost.X, randomLowestCost.Y].RoomType = RoomType.Boss;
            }
        }
        
        // generate additional paths
        for (int i = 0; i < mapInfo.AdditionalPathsNumber; i++)
        {
            var sortedMainCells = _mainPath.OrderBy(CellSort).ToList();
            
            var roomCount = Random.Range(mapInfo.MINCellInAdditionalPath, mapInfo.MAXCellInAdditionalPath);
            var randomStartRoom = sortedMainCells[i];
            var lastRoom = randomStartRoom;
            
            for (var j = 0; j < roomCount; j++)
            {
                var randomLowestCost = GetLowestCostCell(lastRoom);

                _map[randomLowestCost.X, randomLowestCost.Y].RoomType = RoomType.Simple;
                _map[randomLowestCost.X, randomLowestCost.Y].ExtraCost = 5;

                _map[lastRoom.X, lastRoom.Y].RoomDirections.Add(GetVectorDirection(new Vector2(randomLowestCost.X, randomLowestCost.Y) - new Vector2(lastRoom.X, lastRoom.Y)));
                _map[randomLowestCost.X, randomLowestCost.Y].RoomDirections.Add(GetVectorDirection(new Vector2(lastRoom.X, lastRoom.Y) - new Vector2(randomLowestCost.X, randomLowestCost.Y)));

                
                // Debug.DrawLine(new Vector3(randomLowestCost.X + .4f, randomLowestCost.Y + .4f), new Vector3(lastRoom.X + .4f, lastRoom.Y + .4f), Color.magenta, 5);
                _additionalPath.Add(randomLowestCost);
                lastRoom = randomLowestCost;
                
                // check for last last iteration and places loot room there
                if (i == mapInfo.AdditionalPathsNumber - 1 && j == roomCount - 1)
                    _map[randomLowestCost.X, randomLowestCost.Y].RoomType = RoomType.Loot;
            }
        }
    }

    private MapCell[,] GetStartMap()
    {
        var map = new MapCell[30, 30];

        for (int i = 0; i < 30; i++)
        {
            for (int j = 0; j < 30; j++)
            {
                if (i == _startingCell.X && j == _startingCell.Y)
                    map[i, j] = new MapCell(RoomType.Start);
                else
                    map[i, j] = new MapCell();
            }
        }

        return map;
    }
    private int GetCellCost(Point2D cellPos)
    {
        var neighbours = GetCellNeighbours(cellPos);
        int baseCost = 0;
        
        foreach (var point2D in neighbours)
        {
            if (_map[point2D.X, point2D.Y].RoomType != RoomType.None)
                baseCost++;
        }

        return baseCost + _map[cellPos.X, cellPos.Y].ExtraCost;
    }
    private Point2D[] GetCellNeighbours(Point2D cellPos)
    {
        var neighbours = new List<Point2D>();
        
        neighbours.Add(new Point2D(cellPos.X - 1, cellPos.Y));
        neighbours.Add(new Point2D(cellPos.X + 1, cellPos.Y));
        neighbours.Add(new Point2D(cellPos.X, cellPos.Y - 1));
        neighbours.Add(new Point2D(cellPos.X, cellPos.Y + 1));

        return neighbours.ToArray();
    }

    private Point2D GetLowestCostCell(Point2D cellPos)
    {
        var neighbours = GetCellNeighbours(cellPos).ToList();

        neighbours.Shuffle();
        neighbours = neighbours.OrderBy(GetCellCost).ToList();

        for (var index = 0; index < neighbours.Count; index++)
        {
            var point2D = neighbours[index];
            if (_map[point2D.X, point2D.Y].RoomType != RoomType.None)
                neighbours.Remove(point2D);
        }

        return neighbours[0];
    }

    private Direction GetVectorDirection(Vector2 vector2)
    {
        if (vector2 == Vector2.up)
            return Direction.Top;
        else if (vector2 == Vector2.down)
            return Direction.Bottom;
        else if (vector2 == Vector2.left)
            return Direction.Left;
        else if (vector2 == Vector2.right)
            return Direction.Right;

        return Direction.Top;
    }

    private Direction GetReversedDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.Bottom:
                return Direction.Top;
            case Direction.Top:
                return Direction.Bottom;
            case Direction.Left:
                return Direction.Right;
            case Direction.Right:
                return Direction.Left;
        }
        
        return default;
    }
    
    private int CellSort(Point2D point2D)
    {
        var neighbour = GetCellNeighbours(point2D);
        var sum = 0;
        
        foreach (var cell in neighbour)
        {
            sum += GetCellCost(cell);
        }

        Debug.Log(sum);
        return sum;
    }
}
