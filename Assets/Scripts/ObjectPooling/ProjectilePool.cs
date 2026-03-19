using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    public static ProjectilePool Instance;

    [SerializeField] private Projectile projectilePrefab;
    private Queue<Projectile> pool = new();

    void Awake() => Instance = this;

    public Projectile Get()
    {
        if (pool.Count == 0)
        {
            AddProjectile(1);
        }
        return pool.Dequeue();
    }

    private void AddProjectile(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var prefab = Instantiate(projectilePrefab);
            prefab.gameObject.SetActive(false);
            pool.Enqueue(prefab);
        }
    }

    public void ReturnToPool(Projectile projectile)
    {
        projectile.gameObject.SetActive(false);
        pool.Enqueue(projectile);
    }
}
