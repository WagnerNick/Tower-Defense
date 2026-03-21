using System.Collections.Generic;
using UnityEngine;
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
    public Transform Target { get; private set; }
    public TargetMode TargetMode => data.targetMode;

    public float Range { get; private set; }
    public float FireRate { get; private set; }
    public int RuntimeDamage { get; private set; }
    public int RuntimePierce { get; private set; }
    public float RuntimeProjSpeed { get; private set; }

    public int UpgradeLevel { get; private set; } = 0;
    public int MaxUpgrades => data.upgrades?.Count ?? 0;
    public bool CanUpgrade => UpgradeLevel < MaxUpgrades;
    public UpgradeSO NextUpgrade => CanUpgrade ? data.upgrades[UpgradeLevel] : null;

    private float fireCountdown;
    private bool isInfiniteRange;


    private void Awake()
    {
        Range = data.range;
    }

    private void Start()
    {
        FireRate = data.fireRate;
        RuntimeDamage = data.attack.damage;

        if (data.attack is DartAttackSO dart)
        {
            RuntimePierce = dart.pierce;
            RuntimeProjSpeed = dart.speed;
        }

        isInfiniteRange = data.attack is IInfiniteRange;

        fireCountdown = Random.Range(0f, 1f / FireRate);
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
                fireCountdown = 1f / FireRate;
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
        if (!CanUpgrade) return;

        UpgradeSO upgrade = data.upgrades[UpgradeLevel];

        if (PlayerMoney.Instance.money < upgrade.cost) return;

        PlayerMoney.Instance.ChangeMoney(upgrade.cost, false);

        Range += upgrade.rangeBonus;
        FireRate += upgrade.fireRateBonus;
        RuntimeDamage += upgrade.damageBonus;
        RuntimePierce += upgrade.pierceBonus;
        RuntimeProjSpeed += upgrade.projectileSpeed;

        UpgradeLevel++;
        TowerUI.Instance.Refresh();
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
            if (!isInfiniteRange && dist > Range) continue;

            switch (data.targetMode)
            {
                case TargetMode.First:
                    if (enemy.pathProgress > bestValue)
                    { bestValue = enemy.pathProgress; bestEnemy = enemy; }
                    break;
                case TargetMode.Last:
                    if (bestEnemy == null || enemy.pathProgress < bestValue)
                    { bestValue = enemy.pathProgress; bestEnemy = enemy; }
                    break;
                case TargetMode.Close:
                    if (bestEnemy == null || dist < bestValue)
                    { bestValue = dist; bestEnemy = enemy; }
                    break;
                case TargetMode.Strong:
                    if (bestEnemy == null || enemy.damage > bestValue)
                    { bestValue = enemy.damage; bestEnemy = enemy; }
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
        if (rotationPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(rotationPoint.position, data != null ? Range : 1f);
    }
}
