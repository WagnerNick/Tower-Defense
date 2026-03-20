using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public static PlayerHealth Instance;

    [SerializeField] private TMP_Text hpTxt;
    [SerializeField] private int startHealth;
    [SerializeField] private int health;

    public int CurrentHealth => health;

    private void Awake() => Instance = this;

    private void Start()
    {
        if (SaveManager.Instance != null && SaveManager.Instance.HasSave())
        {
            SaveData data = SaveManager.Instance.Load();
            health = data.health;
        }
        else
        {
            health = startHealth;
        }
        hpTxt.text = health.ToString();
    }

    public void Damage(float damageAmount)
    {
        health -= Mathf.RoundToInt(damageAmount);
        hpTxt.text = health.ToString();

        if (health <= 0)
        {
            hpTxt.text = "0";
            GameManager.Instance.GameOver();
        }
    }

    public void Heal(float healAmount)
    {
        health += Mathf.RoundToInt(healAmount);
        hpTxt.text = health.ToString();
    }
}
