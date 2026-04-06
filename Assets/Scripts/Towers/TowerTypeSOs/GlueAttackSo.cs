using UnityEngine;

[CreateAssetMenu(fileName = "GlueAttack", menuName = "TD/Attacks/Glue")]
public class GlueAttackSO : AttackSO
{
    [Header("Dart Stats")]
    public float speed = 50f;
    public int pierce = 2;
    public float hitRadius = 0.3f;
    public float slowAmount = 0.5f;

    public override void Attack(Tower tower)
    {
        Enemy enemy = tower.Target.GetComponentInParent<Enemy>();
        if (enemy == null) return;

        Projectile glue = ProjectilePool.Instance.Get();

        Vector3 aimPoint = tower.PredictEnemyPosition(enemy, tower.FirePoint.position, tower.RuntimeProjSpeed);
        Vector3 dir = (aimPoint - tower.FirePoint.position);
        dir.y = 0f;
        dir.Normalize();

        tower.RotationPoint.rotation = Quaternion.LookRotation(dir);
        glue.transform.SetPositionAndRotation(tower.FirePoint.position, Quaternion.LookRotation(dir));
        glue.Setup(dir, tower.RuntimeProjSpeed, tower.RuntimePierce, hitRadius, tower.RuntimeDamage, slowAmount);
        glue.gameObject.SetActive(true);
    }
}
