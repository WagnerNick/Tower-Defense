using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private Transform rotationPoint;
    private Transform target;

    public float range = 15f;
    public float turnSpeed = 10f;
    public int targetMode = 0;

    public float fireRate = 1f;
    private float fireCountdown = 0f;

    private void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        switch (targetMode)
        {
            case 0:
                Close(enemies);
                break;
            case 1:
                First(enemies);
                break;
        }
    }

    private void Update()
    {
        if (target == null)
            return;

        //Target lock on
        Vector3 dir = target.position - rotationPoint.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(rotationPoint.rotation, lookRotation, turnSpeed * Time.deltaTime).eulerAngles;
        rotationPoint.rotation = Quaternion.Euler(0f, rotation.y, 0f);

        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;
    }

    private void Shoot()
    {
        Debug.Log("Shoot");
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
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }

    private void First(GameObject[] enemies)
    {
        int highestWaypoint = 0;
        GameObject firstEnemy = null;
        foreach (GameObject enemy in enemies)
        {
            int currentWaypoint = enemy.GetComponentInParent<Enemy>().target;
            if (currentWaypoint > highestWaypoint)
            {
                highestWaypoint = currentWaypoint;
                firstEnemy = enemy;
            }
        }
        if (firstEnemy != null && Vector3.Distance(rotationPoint.position, firstEnemy.transform.position) <= range)
        {
            target = firstEnemy.transform;
        }
        else
        {
            target = null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
