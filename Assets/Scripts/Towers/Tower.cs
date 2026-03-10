using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private Transform rotationPoint;
    [SerializeField] private ParticleSystem ps;
    private Transform target;
    private Enemy targetEnemy;

    [Header("Tower Settings")]
    public float range = 15f;
    public float fireRate = 1f;
    private float fireCountdown = 0f;
    public int targetMode = 0;
    public float turnSpeed = 10f;

    [Header("Projectile Settings")]
    public float projectileSpeed = 30f;
    public float aimTolerance = 5f;

    private void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        switch (targetMode)
        {
            case 0: Close(enemies); break;
            case 1: First(enemies); break;
        }
    }

    private void Update()
    {
        if (target == null || targetEnemy == null)
            return;

        Vector3 aimPoint = GetPredictedPosition();

        // Rotate toward predicted point
        Vector3 dir = aimPoint - rotationPoint.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(rotationPoint.rotation, lookRotation, turnSpeed * Time.deltaTime).eulerAngles;
        rotationPoint.rotation = Quaternion.Euler(0f, rotation.y, 0f);

        float angleToTarget = Mathf.Abs(Mathf.DeltaAngle(rotationPoint.eulerAngles.y, lookRotation.eulerAngles.y));

        fireCountdown -= Time.deltaTime;

        if (fireCountdown <= 0f && angleToTarget <= aimTolerance)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }
    }

    private Vector3 GetPredictedPosition()
    {
        if (targetEnemy == null || targetEnemy.path == null || targetEnemy.path.cell == null)
            return targetEnemy?.center != null ? targetEnemy.center.position : target.position;

        var waypoints = targetEnemy.path.cell;
        int waypointIndex = targetEnemy.target;
        float enemySpeed = targetEnemy.speed;

        Vector3 enemyCenter = targetEnemy.center != null ? targetEnemy.center.position : target.position;

        Vector3 predictedPos = enemyCenter;
        float estimatedTime = Vector3.Distance(rotationPoint.position, predictedPos) / projectileSpeed;

        for (int i = 0; i < 5; i++)
        {
            float travelDist = enemySpeed * estimatedTime;
            Vector3 newPos = GetPositionAlongPath(enemyCenter, waypointIndex, waypoints, travelDist);
            float newTime = Vector3.Distance(rotationPoint.position, newPos) / projectileSpeed;

            predictedPos = newPos;

            if (Mathf.Abs(newTime - estimatedTime) < 0.001f)
                break;

            estimatedTime = newTime;
        }

        return predictedPos;
    }

    private Vector3 GetPositionAlongPath(Vector3 currentPos, int nextWaypointIndex, List<Vector3Int> waypoints, float distToWalk)
    {
        Vector3 pos = currentPos;
        int index = nextWaypointIndex;

        while (distToWalk > 0f)
        {
            if (index >= waypoints.Count)
                return waypoints[waypoints.Count - 1];

            Vector3 nextWaypoint = waypoints[index];
            float segmentDist = Vector3.Distance(pos, nextWaypoint);

            if (distToWalk <= segmentDist)
                return Vector3.MoveTowards(pos, nextWaypoint, distToWalk);

            distToWalk -= segmentDist;
            pos = nextWaypoint;
            index++;
        }

        return pos;
    }

    private void Shoot()
    {
        ps.Emit(1);
    }

    private void Close(GameObject[] enemies)
    {
        float shortestDist = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distToEnemy = Vector3.Distance(rotationPoint.position, enemy.transform.position);
            if (distToEnemy < shortestDist)
            {
                shortestDist = distToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDist <= range)
        {
            if (target != nearestEnemy.transform)
                fireCountdown = 1f / fireRate;
            target = nearestEnemy.transform;
            targetEnemy = nearestEnemy.GetComponentInParent<Enemy>();
        }
        else
        {
            target = null;
            targetEnemy = null;
        }
    }

    private void First(GameObject[] enemies)
    {
        int highestWaypoint = 0;
        GameObject firstEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            Enemy e = enemy.GetComponentInParent<Enemy>();
            if (e == null) continue;

            int currentWaypoint = e.target;
            if (currentWaypoint > highestWaypoint && Vector3.Distance(rotationPoint.position, enemy.transform.position) <= range)
            {
                highestWaypoint = currentWaypoint;
                firstEnemy = enemy;
            }
        }

        if (firstEnemy != null)
        {
            if (target != firstEnemy.transform)
                fireCountdown = 1f / fireRate;
            target = firstEnemy.transform;
            targetEnemy = firstEnemy.GetComponentInParent<Enemy>();
        }
        else
        {
            target = null;
            targetEnemy = null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(rotationPoint.position, range);

        if (Application.isPlaying && targetEnemy != null)
        {
            Gizmos.color = Color.yellow;
            Vector3 aim = GetPredictedPosition();
            Gizmos.DrawSphere(aim, 0.3f);
            Gizmos.DrawLine(rotationPoint.position, aim);
        }
    }
}