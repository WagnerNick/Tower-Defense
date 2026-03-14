using UnityEngine;

public abstract class AttackSO : ScriptableObject
{
    [Header("Base Stats")]
    public int damage = 1;

    public abstract void Attack(Tower tower);
}
