using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    private string SaveKey => $"Save_Level_{SceneManager.GetActiveScene().buildIndex}";

    private void Awake() => Instance = this;

    public bool HasSave() => PlayerPrefs.HasKey(SaveKey);

    public void Save(int completedWaveIndex)
    {
        SaveData data = new SaveData
        {
            waveIndex = completedWaveIndex,
            money = PlayerMoney.Instance.money,
            health = PlayerHealth.Instance.CurrentHealth,
        };

        foreach (var entry in PlacementSystem.Instance.GetPlacedTowerData())
            data.towers.Add(entry);

        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();
    }

    public SaveData Load()
    {
        if (!HasSave()) return null;
        string json = PlayerPrefs.GetString(SaveKey);
        return JsonUtility.FromJson<SaveData>(json);
    }

    public void DeleteSave()
    {
        PlayerPrefs.DeleteKey(SaveKey);
        PlayerPrefs.Save();
    }

}
