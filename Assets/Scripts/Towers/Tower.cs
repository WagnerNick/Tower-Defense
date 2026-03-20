using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.AnimationUtility;
using Random = UnityEngine.Random;

public enum TargetMode { First, Last, Close, Strong }

public class Tower : MonoBehaviour
{
    [SerializeField] private TowerDataSO data;
    [SerializeField] private Transform rotationPoint;
    [SerializeField] private Transform firePoint;
    [SerializeField] private ParticleSystem fireAnim;

    public Vector3Int GridPosition { get; private set; }
    public int PlacedObjectIndex { get; private set; }

    public Transform RotationPoint => rotationPoint;
    public Transform FirePoint => firePoint;
    public ParticleSystem FireAnim => fireAnim;
    public float Range => data.range;
    public TargetMode TargetMode => data.targetMode;
    public Transform Target { get; private set; }

    private float fireCountdown;
    private bool isInfiniteRange;

    private void Start()
    {
        isInfiniteRange = data.attack is IInfiniteRange;

        fireCountdown = Random.Range(0f, 1f / data.fireRate);
        InvokeRepeating("UpdateTarget", 0f, 0.25f);
    }

    private void Update()
    {
        fireCountdown -= Time.deltaTime;

        if (fireCountdown <= 0f)
        {
            bool canFire = isInfiniteRange || (Target != null && Target.gameObject.activeInHierarchy);
            if (canFire)
            {
                data.attack.Attack(this);
                fireCountdown = 1f / data.fireRate;
            }
        }
    }

    public void Init(Vector3Int gridPos, int index)
    {
        GridPosition = gridPos;
        PlacedObjectIndex = index;
    }

    public void Upgrade()
    {
        Debug.Log("Tower Upgraded");
    }

    public void Sell()
    {
        PlacementSystem.Instance.RemoveObject(this);
        PlayerMoney.Instance.ChangeMoney(data.cost * 3 / 4, true);
    }

    void UpdateTarget()
    {
        var enemies = EnemyGrid.Instance.GetNearbyEnemies(transform.position);
        Target = GetTarget(enemies);
    }

    Transform GetTarget(List<Enemy> enemies)
    {
        Enemy bestEnemy = null;
        float bestValue = 0f;

        foreach (Enemy enemy in enemies)
        {
            float dist = Vector3.Distance(firePoint.position, enemy.center.position);
            if (dist > data.range)
                continue;

            switch (data.targetMode)
            {
                case TargetMode.First:
                    if (enemy.pathProgress > bestValue)
                    {
                        bestValue = enemy.pathProgress;
                        bestEnemy = enemy;
                    }
                    break;
                case TargetMode.Last:
                    if (bestEnemy == null || enemy.pathProgress < bestValue)
                    {
                        bestValue = enemy.pathProgress;
                        bestEnemy = enemy;
                    }
                    break;
                case TargetMode.Close:
                    if (bestEnemy == null || dist < bestValue)
                    {
                        bestValue = dist;
                        bestEnemy = enemy;
                    }
                    break;
                case TargetMode.Strong:
                    if (bestEnemy == null || enemy.damage > bestValue)
                    {
                        bestValue = enemy.damage;
                        bestEnemy = enemy;
                    }
                    break;
            }
        }
        return bestEnemy != null ? bestEnemy.center : null;
    }

    public Vector3 PredictEnemyPosition(Enemy enemy, Vector3 firePos, float projectileSpeed)
    {
        Vector3 predicted = enemy.center.position;
        for (int i = 0; i < 5; i++)
        {
            float dist = Vector3.Distance(firePos, predicted);
            float travelTime = dist / projectileSpeed;
            predicted = PredictPathPosition(enemy, enemy.speed * travelTime);
        }
        return predicted;
    }

    Vector3 PredictPathPosition(Enemy enemy, float distance)
    {
        Vector3 centerOffset = enemy.center.position - enemy.transform.position;
        centerOffset.y = 0f;

        int waypoint = enemy.target;
        Vector3 currentPos = enemy.transform.position;
        float remaining = distance;

        while (waypoint < enemy.path.cell.Count)
        {
            Vector3 next = enemy.path.cell[waypoint];
            float segmentLength = Vector3.Distance(currentPos, next);

            if (remaining <= segmentLength)
            {
                Vector3 rootPos = Vector3.Lerp(currentPos, next, remaining / segmentLength);
                Vector3 finalPos = rootPos + centerOffset;
                finalPos.y = enemy.center.position.y;
                return finalPos;
            }

            remaining -= segmentLength;
            currentPos = next;
            waypoint++;
        }
        Vector3 endRoot = enemy.path.cell[^1];
        Vector3 endPos = endRoot + centerOffset;
        endPos.y = enemy.center.position.y;
        return endPos;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(rotationPoint.position, data != null ? data.range : 1f);
    }
}
