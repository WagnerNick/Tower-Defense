using UnityEngine;

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

        Vector3 aimPoint = tower.PredictEnemyPosition(enemy, tower.FirePoint.position, tower.RuntimeProjSpeed);
        Vector3 dir = (aimPoint - tower.FirePoint.position);
        dir.y = 0f;
        dir.Normalize();

        tower.RotationPoint.rotation = Quaternion.LookRotation(dir);
        dart.transform.SetPositionAndRotation(tower.FirePoint.position, Quaternion.LookRotation(dir));
        dart.Setup(dir, tower.RuntimeProjSpeed, tower.RuntimePierce, hitRadius, tower.RuntimeDamage);
        dart.gameObject.SetActive(true);
    }
}
