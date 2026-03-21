using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade", menuName = "TD/Upgrade")]
public class UpgradeSO : ScriptableObject
{
    [Header("Identity")]
    public string upgradeName = "Upgrade";
    public int cost = 100;

    [Header("Stat Deltas (0 = no change)")]
    public float rangeBonus = 0f;
    public float fireRateBonus = 0f;
    public int damageBonus = 0;
    public int pierceBonus = 0;
    public float projectileSpeed = 0f;
}
