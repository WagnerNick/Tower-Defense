using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    private int waveIndex = 0;
    private bool spawning = false;

    private void Update()
    {
        if (!spawning && EnemyManager.Instance.Enemies.Count == 0)
        {
            StartCoroutine(SpawnWave());
        }
    }

    IEnumerator SpawnWave()
    {
        spawning = true;

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
