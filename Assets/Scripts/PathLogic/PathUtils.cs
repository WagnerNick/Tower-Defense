using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PathUtils
{
    static Vector3Int[] directions =
    {
        Vector3Int.forward,
        Vector3Int.back,
        Vector3Int.left,
        Vector3Int.right
    };

    public static List<Vector3Int> OrderPath(HashSet<Vector3Int> nodes, Vector3Int start)
    {
        List<Vector3Int> ordered = new();
        HashSet<Vector3Int> visited = new();

        Vector3Int current = start;

        ordered.Add(current);
        visited.Add(current);

        while (true)
        {
            bool foundNext = false;

            foreach (var dir in directions)
            {
                Vector3Int next = current + dir;

                if (nodes.Contains(next) && !visited.Contains(next))
                {
                    ordered.Add(next);
                    visited.Add(next);
                    current = next;
                    foundNext = true;
                    break;
                }
            }
            if (!foundNext)
                break;
        }
        return ordered;
    }
}
