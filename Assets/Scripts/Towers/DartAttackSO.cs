using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CreateAssetMenu(fileName = "DartAttack", menuName = "TD/Attacks/Dart")]
public class DartAttackSO : AttackSO
{
    [Header("Dart Stats")]
    public float speed = 50f;
    public int pierce = 2;
    public float hitRadius = 0.3f;

    public override void Attack(Tower tower)
    {
        Enemy enemy = tower.Target.GetComponentInParent<Enemy>();
        if (enemy == null) return;

        Projectile dart = ProjectilePool.Instance.Get();

        Vector3 aimPoint = tower.PredictEnemyPosition(enemy, tower.FirePoint.position, speed);
        Vector3 dir = (aimPoint - tower.FirePoint.position);
        dir.y = 0f;
        dir.Normalize();

        tower.RotationPoint.rotation = Quaternion.LookRotation(dir);

        dart.transform.SetPositionAndRotation(tower.FirePoint.position, Quaternion.LookRotation(dir));
        dart.Setup(dir, speed, pierce, hitRadius, damage);
        dart.gameObject.SetActive(true);
    }
}
