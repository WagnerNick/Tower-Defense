using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "SniperAttack", menuName = "TD/Attacks/Sniper")]
public class SniperAttackSO : AttackSO, IInfiniteRange
{
    public override void Attack(Tower tower)
    {
        Enemy target = GetBestEnemy(tower);
        if (target == null) return;

        Vector3 dir = target.center.position - tower.FirePoint.position;
        dir.y = 0f;
        if (dir != Vector3.zero)
            tower.RotationPoint.rotation = Quaternion.LookRotation(dir.normalized);

        if (tower.FireAnim != null)
            tower.FireAnim.Play();

        DamageEnemy(target);
    }

    private Enemy GetBestEnemy(Tower tower)
    {
        List<Enemy> enemies = EnemyManager.Instance.Enemies;

        Enemy best = null;
        float bestValue = 0f;

        foreach (Enemy e in enemies)
        {
            if (e == null || !e.gameObject.activeInHierarchy) continue;

            float dist = Vector3.Distance(tower.FirePoint.position, e.center.position);

            switch (tower.TargetMode)
            {
                case TargetMode.First:
                    if (e.pathProgress > bestValue)
                    {
                        bestValue = e.pathProgress;
                        best = e;
                    }
                    break;
                case TargetMode.Last:
                    if (best == null || e.pathProgress < bestValue)
                    {
                        bestValue = e.pathProgress;
                        best = e;
                    }
                    break;
                case TargetMode.Close:
                    if (best == null || dist < bestValue)
                    {
                        bestValue = dist;
                        best = e;
                    }
                    break;
                case TargetMode.Strong:
                    if (best == null || e.damage > bestValue)
                    {
                        bestValue = e.damage;
                        best = e;
                    }
                    break;

            }
        }
        return best;
    }

    private void DamageEnemy(Enemy enemy)
    {
        ShowPopFx(enemy.center.position);
        enemy.GetComponentInParent<IDamageable>()?.Damage(damage);
    }

    private void ShowPopFx(Vector3 position)
    {
        PopFx popFx = PopPool.Instance.Get();
        popFx.transform.SetPositionAndRotation(position, popFx.transform.rotation);
        popFx.gameObject.SetActive(true);
    }
}
