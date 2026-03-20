using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    public int waveIndex;
    public int money;
    public int health;
    public List<PlacedTowerData> towers = new();
}

[Serializable]
public class PlacedTowerData
{
    public int towerID;
    public Vector3IntSerializable gridPos;
}

// Vector3Int isn't serializable by JsonUtility this is the fix
[Serializable]
public struct Vector3IntSerializable
{
    public int x, y, z;
    public Vector3IntSerializable(Vector3Int v) { x = v.x; y = v.y; z = v.z; }
    public Vector3Int ToVector3Int() => new Vector3Int(x, y, z);
}
