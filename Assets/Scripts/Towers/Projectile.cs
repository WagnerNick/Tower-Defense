using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    private int currentPierce;
    public float hitRadius;
    public int damage;

    private Vector3 direction;
    private List<Enemy> hitEnemies = new List<Enemy>();

    public void Setup(Vector3 dir, float speed, int pierce, float hitRadius, int damage)
    {
        this.speed = speed;
        this.hitRadius = hitRadius;
        this.damage = damage;
        currentPierce = pierce;
        direction = dir.normalized;
        transform.forward = direction;
        hitEnemies.Clear();
    }

    private void Update()
    {
        Vector3 newPos = transform.position + direction * speed * Time.deltaTime;
        newPos.y = transform.position.y;
        CheckHits(transform.position, newPos);
        transform.position = newPos;

        if (IsOutsideCameraView())
            ProjectilePool.Instance.ReturnToPool(this);
    }

    void CheckHits(Vector3 start, Vector3 end)
    {
        foreach (Enemy enemy in EnemyGrid.Instance.GetNearbyEnemies(transform.position))
        {
            if (hitEnemies.Contains(enemy)) continue;
            if (DistancePointToSegment(enemy.center.position, start, end) <= hitRadius)
                HitEnemy(enemy);
        }
    }

    void HitEnemy(Enemy enemy)
    {
        hitEnemies.Add(enemy);

        PopFx popFx = PopPool.Instance.Get();
        popFx.transform.SetPositionAndRotation(transform.position, popFx.transform.rotation);
        popFx.gameObject.SetActive(true);

        enemy.GetComponentInParent<IDamageable>()?.Damage(damage);

        currentPierce--;
        if (currentPierce <= 0)
            ProjectilePool.Instance.ReturnToPool(this);
    }

    bool IsOutsideCameraView()
    {
        Vector3 vp = Camera.main.WorldToViewportPoint(transform.position);
        return vp.x < 0f || vp.x > 1f || vp.y < 0f || vp.y > 1f;
    }

    float DistancePointToSegment(Vector3 point, Vector3 a, Vector3 b)
    {
        Vector3 ab = b - a;
        float t = Mathf.Clamp01(Vector3.Dot(point - a, ab) / Vector3.Dot(ab, ab));
        return Vector3.Distance(point, a + t * ab);
    }
}
