using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public static PlayerHealth Instance;

    [SerializeField] private TMP_Text hp;
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;

    private void Awake()
    {
        Instance = this;
        currentHealth = maxHealth;
        hp.text = currentHealth.ToString();
    }

    public void Damage(float damageAmount)
    {
        currentHealth -= Mathf.RoundToInt(damageAmount);
        hp.text = currentHealth.ToString();
    }

    public void Heal(float healAmount)
    {
        currentHealth += Mathf.RoundToInt(healAmount);
        hp.text = currentHealth.ToString();
    }
}
