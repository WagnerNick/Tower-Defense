using System.Collections;
using TMPro;
using UnityEngine;

public class AchievementToast : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descText;

    public void Show(AchievementSO achievement)
    {
        StopAllCoroutines();
        nameText.text = achievement.achievementName;
        descText.text = achievement.description;
        StartCoroutine(DisplayRoutine());
    }

    IEnumerator DisplayRoutine()
    {
        panel.SetActive(true);
        yield return new WaitForSecondsRealtime(3f);
        panel.SetActive(false);
    }
}
