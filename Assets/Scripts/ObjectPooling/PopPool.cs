using System.Collections.Generic;
using UnityEngine;

public class PopPool : MonoBehaviour
{
    public static PopPool Instance;
    [SerializeField] private PopFx popPrefab;
    private Queue<PopFx> pool = new();

    void Awake() => Instance = this;

    public PopFx Get()
    {
        if (pool.Count == 0)
        {
            AddPop(1);
        }
        return pool.Dequeue();
    }

    private void AddPop(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var prefab = Instantiate(popPrefab);
            prefab.gameObject.SetActive(false);
            pool.Enqueue(prefab);
        }
    }

    public void ReturnToPool(PopFx pop)
    {
        pop.gameObject.SetActive(false);
        pool.Enqueue(pop);
    }
}
