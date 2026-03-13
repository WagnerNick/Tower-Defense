using System.Collections.Generic;
using UnityEngine;

public class Dart : MonoBehaviour
{
    public float speed = 70f;
    public int maxPierce = 2;
    private int currentPierce = 0;
    public float hitRadius = 0.3f;

    private Vector3 direction;
    private Vector3 lastPosition;

    private List<Enemy> hitEnemies = new List<Enemy>();

    public void Fire(Vector3 dir)
    {
        direction = dir.normalized;
        transform.forward = direction;
        currentPierce = maxPierce;
        hitEnemies.Clear();
        lastPosition = transform.position;
    }

    private void Update()
    {
        Vector3 newPos = transform.position + direction * speed * Time.deltaTime;
        newPos.y = transform.position.y;
        CheckHits(transform.position, newPos);
        transform.position = newPos;
        lastPosition = newPos;

        if (IsOutsideCameraView())
            DartPool.Instance.ReturnToPool(this);
    }

    void CheckHits(Vector3 start, Vector3 end)
    {
        var enemies = EnemyGrid.Instance.GetNearbyEnemies(transform.position);

        foreach (Enemy enemy in enemies)
        {
            if (hitEnemies.Contains(enemy))
                continue;

            float dist = DistancePointToSegment(enemy.center.position, start, end);

            if (dist <= hitRadius)
                HitEnemy(enemy);
        }
    }

    void HitEnemy(Enemy enemy)
    {
        hitEnemies.Add(enemy);

        PopFx popFx = PopPool.Instance.Get();

        popFx.transform.SetPositionAndRotation(transform.position, popFx.transform.rotation);
        popFx.gameObject.SetActive(true);

        IDamageable damageable = enemy.GetComponentInParent<IDamageable>();
        if (damageable != null)
        {
            damageable.Damage(1);
        }

        currentPierce--;

        if (currentPierce <= 0)
            DartPool.Instance.ReturnToPool(this);
    }

    bool IsOutsideCameraView()
    {
        Vector3 vp = Camera.main.WorldToViewportPoint(transform.position);
        return vp.x < 0f || vp.x > 1f || vp.y < 0f || vp.y > 1f;
    }

    float DistancePointToSegment(Vector3 point, Vector3 a, Vector3 b)
    {
        Vector3 ab = b - a;

        float t = Vector3.Dot(point - a, ab) / Vector3.Dot(ab, ab);
        t = Mathf.Clamp01(t);

        Vector3 closest = a + t * ab;
        return Vector3.Distance(point, closest);
    }
}
