using System;
using System.Collections.Generic;
using UnityEngine;

public class GridData
{
    Dictionary<Vector3Int, PlacementData> placedObjects = new();

    public void AddObjectAt(Vector3Int cellPos, Vector2Int objectSize, int ID, int placedObjectIndex)
    {
        List<Vector3Int> posToOccupy = CalculatePos(cellPos, objectSize);
        PlacementData data = new PlacementData(posToOccupy, ID, placedObjectIndex);
        foreach (var pos in posToOccupy)
        {
            if (placedObjects.ContainsKey(pos))
                throw new Exception($"Dictionary already contains this cell pos {pos}");
            placedObjects[pos] = data;
        }
    }

    private List<Vector3Int> CalculatePos(Vector3Int cellPos, Vector2Int objectSize)
    {
        List<Vector3Int> returnVal = new();
        for (int x = 0; x < objectSize.x; x++)
        {
            for (int y = 0; y < objectSize.y; y++)
            {
                returnVal.Add(cellPos + new Vector3Int(x, 0, y));
            }
        }
        return returnVal;
    }

    public bool CanPlaceObjectAt(Vector3Int cellPos, Vector2Int objectSize)
    {
        List<Vector3Int> posToOccupy = CalculatePos(cellPos, objectSize);
        foreach (var pos in posToOccupy)
        {
            if (placedObjects.ContainsKey(pos))
                return false;
        }
        return true;
    }

    internal int GetRepIndex(Vector3Int gridPos)
    {
        if (!placedObjects.ContainsKey(gridPos))
            return -1;

        return placedObjects[gridPos].PlacedObjectIndex;
    }

    internal void RemoveObjectAt(Vector3Int gridPos)
    {
        foreach (var pos in placedObjects[gridPos].occupiedCells)
        {
            placedObjects.Remove(pos);
        }
    }
}

public class PlacementData
{
    public List<Vector3Int> occupiedCells;

    public int ID { get; private set; }
    public int PlacedObjectIndex { get; private set; }

    public PlacementData(List<Vector3Int> occupiedCells, int iD, int placedObjectIndex)
    {
        this.occupiedCells = occupiedCells;
        ID = iD;
        PlacedObjectIndex = placedObjectIndex;
    }
}
