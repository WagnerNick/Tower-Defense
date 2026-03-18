using System.Collections;
using TMPro;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private TMP_Text roundCounter;
    [SerializeField] private TMP_Text endRound;
    private int waveIndex = 0;
    private bool spawning = false;

    private void Update()
    {
        if (!spawning && EnemyManager.Instance.Enemies.Count == 0 && !GameManager.Instance.gameIsEnded)
        {
            PlayerMoney.Instance.ChangeMoney(100, true);
            StartCoroutine(SpawnWave());
        }
        else
            endRound.text = $"Round {waveIndex}";
    }

    IEnumerator SpawnWave()
    {
        spawning = true;
        roundCounter.text = $"{waveIndex + 1}/40";
        waveIndex++;

        for (int i = 0; i < waveIndex; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(0.5f);
        }

        spawning = false;
    }

    void SpawnEnemy()
    {
        Enemy enemy = EnemyPool.Instance.Get();
        enemy.gameObject.SetActive(true);
    }
}
