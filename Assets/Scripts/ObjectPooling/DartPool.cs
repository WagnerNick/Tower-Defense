using System.Collections.Generic;
using UnityEngine;

public class DartPool : PersistentSingleton<DartPool>
{
    [SerializeField] private Dart dartPrefab;
    private Queue<Dart> pool = new();

    public Dart Get()
    {
        if (pool.Count == 0)
        {
            AddDart(1);
        }
        return pool.Dequeue();
    }

    private void AddDart(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var prefab = Instantiate(dartPrefab);
            prefab.gameObject.SetActive(false);
            pool.Enqueue(prefab);
        }
    }

    public void ReturnToPool(Dart dart)
    {
        dart.gameObject.SetActive(false);
        pool.Enqueue(dart);
    }
}
