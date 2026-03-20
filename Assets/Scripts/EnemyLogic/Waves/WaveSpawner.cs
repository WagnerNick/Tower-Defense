using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private WaveDataSO waveData;
    [SerializeField] private TMP_Text roundCounter;
    [SerializeField] private TMP_Text endRound;

    private int waveIndex = 0;
    private bool spawning = false;

    private void Start()
    {
        if (SaveManager.Instance.HasSave())
        {
            SaveData data = SaveManager.Instance.Load();
            waveIndex = data.waveIndex;
        }
    }

    private void Update()
    {
        if (spawning || GameManager.Instance.gameIsEnded) return;

        endRound.text = $"Round {waveIndex}";

        if (EnemyManager.Instance.Enemies.Count == 0)
        {
            if (waveIndex >= waveData.waves.Count)
            {
                SaveManager.Instance.DeleteSave();
                GameManager.Instance.Victory();
                return;
            }
            StartCoroutine(SpawnWave(waveData.waves[waveIndex]));
            waveIndex++;
        }
    }

    IEnumerator SpawnWave(WaveDef wave)
    {
        spawning = true;
        roundCounter.text = $"{waveIndex + 1}/{waveData.waves.Count}";

        foreach (var group in wave.groups)
            StartCoroutine(SpawnGroup(group));

        yield return new WaitForSeconds(GetWaveDuration(wave) + 0.1f);
        yield return new WaitUntil(() => EnemyManager.Instance.Enemies.Count == 0);

        if (GameManager.Instance.gameIsEnded)
        {
            spawning = false;
            yield break;
        }

        PlayerMoney.Instance.ChangeMoney(100 + waveIndex, true);
        SaveManager.Instance.Save(waveIndex);

        spawning = false;
    }

    IEnumerator SpawnGroup(GroupDef group)
    {
        yield return new WaitForSeconds(group.startDelay);

        for (int i = 0; i < group.count; i++)
        {
            SpawnEnemy(group.balloonType);
            yield return new WaitForSeconds(group.interval);
        }
    }

    float GetWaveDuration(WaveDef wave)
    {
        float max = 0f;
        foreach (var group in wave.groups)
        {
            float groupEnd = group.startDelay + group.interval * (group.count - 1);
            if (groupEnd > max) max = groupEnd;
        }
        return max;
    }

    void SpawnEnemy(BalloonTypeSO type)
    {
        Enemy enemy = EnemyPool.Instance.Get();
        enemy.GetComponent<EnemyHealth>().SetInitialType(type);
        enemy.gameObject.SetActive(true);
    }
}
