using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager Instance;

    [SerializeField] private List<AchievementSO> achievements;
    [SerializeField] private AchievementToast toast;

    private HashSet<string> unlocked = new();
    private int towersPlaced = 0;

    private void Awake() => Instance = this;

    private void OnEnable()
    {
        GameEvents.OnEnemyPopped += CheckPops;
        GameEvents.OnWaveCompleted += CheckWaves;
        GameEvents.OnTowerPlaced += CheckTowers;
        GameEvents.OnVictory += CheckVictory;
    }

    private void OnDisable()
    {
        GameEvents.OnEnemyPopped -= CheckPops;
        GameEvents.OnWaveCompleted -= CheckWaves;
        GameEvents.OnTowerPlaced -= CheckTowers;
        GameEvents.OnVictory -= CheckVictory;
    }

    void CheckPops(int total)
    {
        foreach (var a in achievements)
            if (a.type == AchievementType.TotalPops && total >= a.targetValue)
                Unlock(a);
    }

    void CheckWaves(int waveIndex)
    {
        foreach (var a in achievements)
            if (a.type == AchievementType.WavesCompleted && waveIndex >= a.targetValue)
                Unlock(a);
    }

    void CheckTowers()
    {
        towersPlaced++;
        foreach (var a in achievements)
            if (a.type == AchievementType.TowersPlaced && towersPlaced >= a.targetValue)
                Unlock(a);
    }

    void CheckVictory()
    {
        foreach (var a in achievements)
            if (a.type == AchievementType.Victory)
                Unlock(a);
    }

    void Unlock(AchievementSO achievement)
    {
        if (unlocked.Contains(achievement.achievementName)) return;
        unlocked.Add(achievement.achievementName);
        toast?.Show(achievement);
        Debug.Log($"Unlocked achievement: {achievement.achievementName}");
    }
}
