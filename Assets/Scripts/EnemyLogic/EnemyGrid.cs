using System.Collections.Generic;
using UnityEngine;

public class EnemyGrid : MonoBehaviour
{
    public static EnemyGrid Instance;

    public float cellSize = 10f;

    private Dictionary<Vector2Int, List<Enemy>> grid = new Dictionary<Vector2Int, List<Enemy>>();

    private void Awake()
    {
        Instance = this;
    }

    Vector2Int GetCell(Vector3 position)
    {
        int x = Mathf.FloorToInt(position.x / cellSize);
        int z = Mathf.FloorToInt(position.z / cellSize);
        return new Vector2Int(x, z);
    }

    public void AddEnemy(Enemy enemy)
    {
        Vector2Int cell = GetCell(enemy.transform.position);

        if (!grid.ContainsKey(cell))
            grid[cell] = new List<Enemy>();

        grid[cell].Add(enemy);
        enemy.currentCell = cell;
    }

    public void RemoveEnemy(Enemy enemy)
    {
        if (grid.ContainsKey(enemy.currentCell))
            grid[enemy.currentCell].Remove(enemy);
    }

    public void UpdateEnemy(Enemy enemy)
    {
        Vector2Int newCell = GetCell(enemy.transform.position);

        if (newCell == enemy.currentCell)
            return;

        if (grid.ContainsKey(enemy.currentCell))
            grid[enemy.currentCell].Remove(enemy);

        if (!grid.ContainsKey(newCell))
            grid[newCell] = new List<Enemy>();

        grid[newCell].Add(enemy);
        enemy.currentCell = newCell;
    }

    public List<Enemy> GetNearbyEnemies(Vector3 position)
    {
        List<Enemy> result = new List<Enemy>();

        Vector2Int center = GetCell(position);

        for (int x = -1; x <= 1; x++)
        {
            for (int z = -1; z <= 1; z++)
            {
                Vector2Int cell = new Vector2Int(center.x + x, center.y + z);

                if (grid.ContainsKey(cell))
                    result.AddRange(grid[cell]);
            }
        }
        return result;
    }
}
