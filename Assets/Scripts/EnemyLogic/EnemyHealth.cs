using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private MeshRenderer balloonRenderer;

    private BalloonTypeSO initialType;
    private BalloonTypeSO currentType;
    private int currentHealth;
    private static int totalPops = 0;
    private MaterialPropertyBlock propertyBlock;

    private Enemy enemy;

    private void Awake()
    {
        propertyBlock = new MaterialPropertyBlock();
        enemy = GetComponent<Enemy>();
    }

    private void OnEnable()
    {
        if (initialType != null)
            ApplyType(initialType);
    }

    public void Damage(float damageAmount)
    {
        float overflow = damageAmount - currentHealth;
        currentHealth -= Mathf.RoundToInt(damageAmount);
        PlayerMoney.Instance.ChangeMoney(1, true);

        if (currentHealth <= 0)
        {
            Pop(overflow);
        }
    }

    public void Heal(float healAmount)
    {
        currentHealth = Mathf.Min(currentHealth + Mathf.RoundToInt(healAmount), currentType.health);
    }

    void Pop(float overflowDamage)
    {
        if (currentType.popsInto != null)
        {
            ApplyType(currentType.popsInto);
            if (overflowDamage > 0)
                Damage(overflowDamage);
        }
        else
        {
            totalPops++;
            GameEvents.EnemyPopped(totalPops);
            EnemyPool.Instance.ReturnToPool(enemy);
        }
    }

    public void SetInitialType(BalloonTypeSO type)
    {
        initialType = type;
    }

    void ApplyType(BalloonTypeSO type)
    {
        currentType = type;
        currentHealth = type.health;

        enemy.speed = type.speed;
        enemy.damage = type.damage;

        if (balloonRenderer != null)
        {
            balloonRenderer.GetPropertyBlock(propertyBlock);
            propertyBlock.SetColor("_Color", type.color);
            balloonRenderer.SetPropertyBlock(propertyBlock);

            balloonRenderer.transform.localScale = Vector3.one * type.scale;
        }
    }
}
