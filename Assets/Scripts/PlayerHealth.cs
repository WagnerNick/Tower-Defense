using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public static PlayerHealth Instance;

    [SerializeField] private TMP_Text hpTxt;
    [SerializeField] private int startHealth;
    [SerializeField] private int health;

    private void Awake()
    {
        Instance = this;
        health = startHealth;
        hpTxt.text = health.ToString();
    }

    public void Damage(float damageAmount)
    {
        health -= Mathf.RoundToInt(damageAmount);
        hpTxt.text = health.ToString();

        if (health <= 0)
        {
            GameManager.Instance.EndGame();
        }
    }

    public void Heal(float healAmount)
    {
        health += Mathf.RoundToInt(healAmount);
        hpTxt.text = health.ToString();
    }
}
