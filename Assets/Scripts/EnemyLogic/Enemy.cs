using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] public PathDataSO path;
    public Transform center;
    public float speed = 1f;

    [HideInInspector] public int target;
    [HideInInspector] public float pathProgress;

    private Vector3 lastPos;

    private void OnEnable()
    {
        transform.position = path.cell[0];
        target = 1;
        pathProgress = 0f;

        lastPos = transform.position;

        EnemyGrid.Instance.AddEnemy(this);
        EnemyManager.Instance.RegisterEnemy(this);
    }

    private void OnDisable()
    {
        EnemyGrid.Instance.RemoveEnemy(this);
        EnemyManager.Instance.UnregisterEnemy(this);
    }

    private void Update()
    {
        if (target <= path.cell.Count - 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, path.cell[target], speed * Time.deltaTime);

            float moved = Vector3.Distance(transform.position, lastPos);
            pathProgress += moved;
            lastPos = transform.position;

            if (Vector3.Distance(transform.position, path.cell[target]) < 0.0001f)
            {
                target++;
            }
        }
        else
        {
            EnemyPool.Instance.ReturnToPool(this);
        }
    }
}

