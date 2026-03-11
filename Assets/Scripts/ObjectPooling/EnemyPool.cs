using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : PersistentSingleton<EnemyPool>
{
    [SerializeField] private Enemy enemyPrefab;
    private Queue<Enemy> pool = new();

    public Enemy Get()
    {
        if (pool.Count == 0)
        {
            AddEnemy(1);
        }
        return pool.Dequeue();
    }

    private void AddEnemy(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var prefab = Instantiate(enemyPrefab);
            prefab.gameObject.SetActive(false);
            pool.Enqueue(prefab);
        }
    }

    public void ReturnToPool(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
        pool.Enqueue(enemy);
    }
}
