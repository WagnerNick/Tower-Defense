using System;

public static class GameEvents
{
    public static event Action<int> OnEnemyPopped;
    public static event Action<int> OnWaveCompleted;
    public static event Action OnVictory;
    public static event Action OnTowerPlaced;

    public static void EnemyPopped(int totalPops) => OnEnemyPopped?.Invoke(totalPops);
    public static void WaveCompleted(int waveNumber) => OnWaveCompleted?.Invoke(waveNumber);
    public static void TowerPlaced() => OnTowerPlaced?.Invoke();
    public static void Victory() => OnVictory?.Invoke();
}
