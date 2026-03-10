using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    private int waveIndex = 0;
    private void Update()
    {
        if (EnemyPool.Instance.aliveEnemies == 0)
        {
            StartCoroutine(SpawnWave());
        }
    }

    IEnumerator SpawnWave()
    {
        waveIndex++;

        for (int i = 0; i < waveIndex; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(0.5f);
        }
    }

    void SpawnEnemy()
    {
        Enemy enemy = EnemyPool.Instance.Get();
        enemy.gameObject.SetActive(true);
        EnemyPool.Instance.aliveEnemies++;
    }
}
