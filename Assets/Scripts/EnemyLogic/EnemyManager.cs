using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

    public List<Enemy> Enemies = new List<Enemy>();

    private void Awake()
    {
        Instance = this;
        Enemies = new List<Enemy>();
    }

    public void RegisterEnemy(Enemy enemy)
    {
        Enemies.Add(enemy);
    }

    public void UnregisterEnemy(Enemy enemy)
    {
        Enemies.Remove(enemy);
    }
}
