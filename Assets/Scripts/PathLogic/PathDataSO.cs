using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TD/Path Data")]
public class PathDataSO : ScriptableObject
{
    public List<Vector3Int> cell = new();
}
