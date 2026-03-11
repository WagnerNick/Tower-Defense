using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum TargetMode
{
    First,
    Last,
    Close,
    Strong
}

public class Tower : MonoBehaviour
{
    [SerializeField] private Transform rotationPoint;
    [SerializeField] private Transform firePoint;

    public Transform target;

    [Header("Attributes")]
    public float range = 15f;
    public float fireRate = 1f;
    private float fireCountdown;

    public TargetMode targetMode;

    private void Start()
    {
        fireCountdown = Random.Range(0f, 1f / fireRate);
        InvokeRepeating("UpdateTarget", 0f, 0.25f);
    }

    private void Update()
    {
        fireCountdown -= Time.deltaTime;

        if (fireCountdown <= 0f)
        {
            if (target != null)
            {
                Shoot();
                fireCountdown = 1f / fireRate;
            }
        }
    }

    void UpdateTarget()
    {
        var enemies = EnemyGrid.Instance.GetNearbyEnemies(transform.position);
        target = GetTarget(enemies);
    }

    private void Shoot()
    {
        if (target == null || !target.gameObject.activeInHierarchy)
        {
            target = null;
            return;
        }

        Enemy enemy = target.GetComponentInParent<Enemy>();

        if (enemy == null || enemy.center == null)
        {
            target = null;
            return;
        }

        Dart dart = DartPool.Instance.Get();
        float projectileSpeed = dart.speed;

        Vector3 aimPoint = PredictEnemyPosition(enemy, firePoint.position, projectileSpeed);

        Vector3 dir = aimPoint - firePoint.position;
        dir.y = 0f;
        dir.Normalize();

        rotationPoint.rotation = Quaternion.LookRotation(dir);

        dart.transform.SetPositionAndRotation(firePoint.position, Quaternion.LookRotation(dir));
        dart.Fire(dir);
        dart.gameObject.SetActive(true);
        Debug.DrawLine(firePoint.position, aimPoint, Color.red, 1f);
    }

    Vector3 PredictEnemyPosition(Enemy enemy, Vector3 firePos, float projectileSpeed)
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

    Transform GetTarget(List<Enemy> enemies)
    {
        Enemy bestEnemy = null;
        float bestValue = 0f;

        foreach (Enemy enemy in enemies)
        {
            float dist = Vector3.Distance(firePoint.position, enemy.center.position);

            if (dist > range)
                continue;

            switch (targetMode)
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
                    if (bestEnemy == null || enemy.speed > bestValue)
                    {
                        bestValue = enemy.speed;
                        bestEnemy = enemy;
                    }
                    break;
            }
        }
        return bestEnemy != null ? bestEnemy.center.transform : null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
