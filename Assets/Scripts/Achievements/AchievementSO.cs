using UnityEngine;

[CreateAssetMenu(menuName = "TD/Achievement")]
public class AchievementSO : ScriptableObject
{
    public string achievementName;
    [TextArea] public string description;
    public AchievementType type;
    public int targetValue;
}

public enum AchievementType
{
    TotalPops,
    WavesCompleted,
    TowersPlaced,
    Victory
}
