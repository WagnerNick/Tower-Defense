using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private PathDataSO path;
    public float speed = 1f;
    [HideInInspector] public int target;
    private void OnEnable()
    {
        transform.position = path.cell[0];
        target = 1;
    }

    private void Update()
    {
        if (target <= path.cell.Count - 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, path.cell[target], speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, path.cell[target]) < 0.0001f)
            {
                target++;
            }
        }
        else
        {
            EnemyPool.Instance.ReturnToPool(this);
            EnemyPool.Instance.aliveEnemies--;
        }
    }
}

