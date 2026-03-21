using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerData", menuName = "TD/Tower Data")]
public class TowerDataSO : ScriptableObject
{
    [Header("Identity")]
    public string towerName;
    public int cost;

    [Header("Stats")]
    public float range = 15f;
    public float fireRate = 1f;

    [Header("Attack")]
    public TargetMode targetMode;
    public AttackSO attack;

    [Header("Upgrades")]
    public List<UpgradeSO> upgrades = new();
}
